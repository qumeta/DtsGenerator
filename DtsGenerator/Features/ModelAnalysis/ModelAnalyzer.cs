using DataStructures.Graphs;
using DtsGenerator.CSharp;
using DtsGenerator.CSharp.Aggregation;
using DtsGenerator.Features.Common;
using DtsGenerator.Models;
using DtsGenerator.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using System.Collections.Generic;
using System.Linq;

namespace DtsGenerator.Features.ModelAnalysis
{
    public class ModelAnalyzer
    {
        private readonly IAnalysisContext _context;
        private readonly TypeAnalyzer _typeAnalyzer;

        public ModelAnalyzer(IAnalysisContext context)
        {
            _context = context;
            _typeAnalyzer = new TypeAnalyzer(_context);
        }

        public AnalysisResult<CSharpDataModels> Analyze(string path)
        {
            var models = new CSharpDataModels();

            _context.Init(path);

            var graph = BuildTypeDependencyGraph();

            var readable = graph.ToReadable();

            var types = graph.Vertices;

            foreach (var type in types)
            {
                if (type.IsExternal)
                {
                    continue;
                }

                var dependencies = graph.OutgoingEdges(type)
                    .Select(e => e.Destination)
                    .Where(d => d.TypeKind == TypeKind.Class || d.TypeKind == TypeKind.Enum)
                    .ToList();

                var semanticModel = type.SemanticModel;

                if (semanticModel == null)
                {
                    if (type.NamedTypeSymbol != null)
                    {
                        if (type.TypeKind == TypeKind.Enum)
                        {
                            models.Enums.Add(AnalyzeEnumSymbol(type.NamedTypeSymbol));
                        }
                    }
                }
                else
                {
                    var syntaxTree = semanticModel.SyntaxTree;

                    if (type.TypeKind == TypeKind.Class)
                    {
                        models.Classes.Add(AnalyzeClassSymbol(type.NamedTypeSymbol));
                    }
                    else if (type.TypeKind == TypeKind.Enum)
                    {
                        models.Enums.Add(AnalyzeEnumSymbol(type.NamedTypeSymbol));
                    }
                }
            }

            return new AnalysisResult<CSharpDataModels>()
            {
                Value = models,
                Success = true
            };
        }

        public CSharpClassModel AnalyzeClassSymbol(INamedTypeSymbol classSymbol)
        {
            var classModel = new CSharpClassModel() { Name = classSymbol.Name };

            var members = classSymbol.GetMembers().ToList();

            HandleInheritance(classModel, classSymbol);
            HandleGenerics(classModel, classSymbol);
            HandleDependencies(classModel, classSymbol);

            var properties = members.Where(m => m.Kind == SymbolKind.Property).ToList();

            foreach (var property in properties)
            {
                var propertySymbol = property as IPropertySymbol;

                // for generic types we want to generate the interface instead of the specific, substituted property types
                if (!propertySymbol.OriginalDefinition.Equals(propertySymbol))
                {
                    propertySymbol = propertySymbol.OriginalDefinition;
                }

                var propertyModel = new CSharpPropertyModel()
                {
                    Name = propertySymbol.Name,
                    Type = _typeAnalyzer.AnalyzeType(propertySymbol.Type)
                };

                classModel.Properties.Add(propertyModel);
            }

            return classModel;
        }

        public CSharpEnumModel AnalyzeEnumSymbol(INamedTypeSymbol enumSymbol)
        {
            var enumModel = new CSharpEnumModel() { Name = enumSymbol.Name };

            var fields = enumSymbol.GetMembers()
                .Where(m => m.Kind == SymbolKind.Field)
                .ToList();

            foreach (var field in fields)
            {
                var fieldSymbol = field as IFieldSymbol;

                var member = new CSharpEnumMemberModel()
                {
                    Name = fieldSymbol.Name,
                    Value = fieldSymbol.HasConstantValue ? (int)fieldSymbol.ConstantValue : 0
                };

                enumModel.Members.Add(member);
            }

            return enumModel;
        }

        private DirectedSparseGraph<DependantType> BuildTypeDependencyGraph()
        {
            var graph = new DirectedSparseGraph<DependantType>();
            var processedTypes = new List<DependantType>();

            foreach (var syntaxTree in _context.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();
                var semanticModel = _context.Compilation.GetSemanticModel(syntaxTree);

                var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

                foreach (var classNode in classNodes)
                {
                    DependantType dependantType = null;

                    var classSymbol = semanticModel.GetDeclaredSymbol(classNode) as INamedTypeSymbol;
                    var bond_exsit = classSymbol.GetAttributes().Where(item => item.ToString().StartsWith("System.CodeDom.Compiler.GeneratedCode") ).Count() > 0;
                    if (bond_exsit == false)
                        continue;

                    var processedType = processedTypes.FirstOrDefault(t => t.Name == classSymbol.Name);
                    if (processedType != null)
                    {
                        // we already added this to the graph as a dependency earlier
                        dependantType = processedType;

                        dependantType.SemanticModel = semanticModel;
                        dependantType.NamedTypeSymbol = classSymbol;
                    }
                    else
                    {
                        dependantType = new DependantType()
                        {
                            Name = classSymbol.Name,
                            Namespace = classSymbol.ContainingNamespace.ToString(),
                            ContainingAssembly = classSymbol.ContainingAssembly?.Name,
                            IsExternal = false,
                            SemanticModel = semanticModel,
                            NamedTypeSymbol = classSymbol,
                            TypeKind = classSymbol.TypeKind
                        };

                        processedTypes.Add(dependantType);
                        graph.AddVertex(dependantType);
                    }

                    // 解决方案存在时
                    if (_context.Solution != null)
                    {
                        var referencesToClass = SymbolFinder.FindReferencesAsync(classSymbol, _context.Solution).Result;
                        // TODO 暂不考虑
                    }

                    var classDependencies = classNode.DescendantNodes()
                        .Select(node => semanticModel.GetTypeInfo(node).Type)
                        .Where(node => node != null)
                        .Distinct()
                        .ToList();

                    foreach (var dependency in classDependencies)
                    {
                        DependantType dep = null;

                        var processedDep = processedTypes.FirstOrDefault(t => t.Name == dependency.Name);

                        if (processedDep != null)
                        {
                            dep = processedDep;
                        }
                        else
                        {
                            dep = new DependantType()
                            {
                                Name = dependency.Name,
                                Namespace = dependency.ContainingNamespace.ToString(),
                                ContainingAssembly = dependency.ContainingAssembly?.Name,
                                NamedTypeSymbol = dependency as INamedTypeSymbol,
                                TypeKind = dependency.TypeKind,
                                IsExternal = dependency.ContainingAssembly?.Name == "mscorlib"
                            };

                            processedTypes.Add(dep);
                        }

                        if (!graph.HasVertex(dep))
                        {
                            graph.AddVertex(dep);
                        }

                        graph.AddEdge(dependantType, dep);
                    }
                }

                var enumNodes = root.DescendantNodes().OfType<EnumDeclarationSyntax>().ToList();

                foreach (var enumNode in enumNodes)
                {
                    DependantType dependantType = null;

                    var enumSymbol = semanticModel.GetDeclaredSymbol(enumNode) as INamedTypeSymbol;
                    var bond_exsit = enumSymbol.GetAttributes().Where(item => item.ToString().StartsWith("System.CodeDom.Compiler.GeneratedCode")).Count() > 0;
                    if (bond_exsit == false)
                        continue;

                    var processedType = processedTypes.FirstOrDefault(t => t.Name == enumSymbol.Name);
                    if (processedType != null)
                    {
                        // we already added this to the graph as a dependency earlier
                        dependantType = processedType;

                        dependantType.SemanticModel = semanticModel;
                        dependantType.NamedTypeSymbol = enumSymbol;
                    }
                    else
                    {
                        dependantType = new DependantType()
                        {
                            Name = enumSymbol.Name,
                            Namespace = enumSymbol.ContainingNamespace.ToString(),
                            ContainingAssembly = enumSymbol.ContainingAssembly.Name,
                            IsExternal = false,
                            SemanticModel = semanticModel,
                            NamedTypeSymbol = enumSymbol,
                            TypeKind = enumSymbol.TypeKind
                        };

                        processedTypes.Add(dependantType);
                        graph.AddVertex(dependantType);
                    }
                }
            }

            return graph;
        }

        private void HandleInheritance(CSharpClassModel model, INamedTypeSymbol classSymbol)
        {
            if (classSymbol.BaseType != null
                && classSymbol.BaseType.ContainingAssembly.Name != "mscorlib"
                 && classSymbol.BaseType.ContainingAssembly.Name != "System.Runtime")
            {
                //System.Runtime System.Object
                model.BaseClass = classSymbol.BaseType.Name;
            }
        }

        private void HandleGenerics(CSharpClassModel model, INamedTypeSymbol classSymbol)
        {
            if (classSymbol.IsGenericType && !classSymbol.TypeParameters.IsDefaultOrEmpty)
            {
                model.IsGeneric = true;

                foreach (var typeParameter in classSymbol.TypeParameters)
                {
                    model.TypeParameters.Add(new CSharpTypeParameterModel() { Name = typeParameter.Name });
                }
            }
        }

        private void HandleDependencies(CSharpClassModel model, INamedTypeSymbol classSymbol)
        {
            var depService = new DependencyService();
            // 依赖不会重复
            //var dependencies = depService.GetTypeDependencies(classSymbol).Distinct().ToList();
            var dependencies = depService.GetTypeDependencies(classSymbol);

            foreach (var dep in dependencies)
            {
                model.Dependencies.Add(new CSharpDependencyModel()
                {
                    Name = dep.Name,
                    Namespace = dep.ContainingNamespace.ToString(),
                    DependencyKind = dep.TypeKind == TypeKind.Class ? DependencyKind.Model : DependencyKind.Enum
                });
            }
        }
    }
}
