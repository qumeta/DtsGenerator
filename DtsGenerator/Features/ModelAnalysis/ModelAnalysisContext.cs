using Buildalyzer;
using Buildalyzer.Workspaces;
using DtsGenerator.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DtsGenerator.Features.ModelAnalysis
{
    public class ModelAnalysisContext : AnalysisContextBase
    {
        public ModelAnalysisContext() : base() { }

        public override void Init(string path)
        {
            var extension = Path.GetExtension(path);

            if (extension == DotNetFileExtension.Solution)
            {
                AnalyzerManager manager = new AnalyzerManager(path);
                _workspace = manager.GetWorkspace();
                _solution = _workspace.CurrentSolution;
                _project = _solution.Projects.FirstOrDefault(p => p.Name.Contains("Game.Models"));

                _compilation = _project.GetCompilationAsync().Result;
            }
            else if (extension == DotNetFileExtension.Project)
            {
                AnalyzerManager manager = new AnalyzerManager();
                ProjectAnalyzer analyzer = manager.GetProject(path);

                _workspace = analyzer.GetWorkspace();
                _solution = _workspace.CurrentSolution;
                _project = _solution.Projects.FirstOrDefault(p => p.Name.Contains("Game.Models"));

                _compilation = _project.GetCompilationAsync().Result;
            }
            else
            {
                // parse the syntax tree and build a compilation
                var text = File.ReadAllText(path);
                var tree = CSharpSyntaxTree.ParseText(text).WithFilePath(path);
                var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

                _compilation = CSharpCompilation.Create(
                    "DtsGeneratorCompilation",
                    syntaxTrees: new[] { tree },
                    references: new[] { mscorlib }
                );
            }
        }
    }
}
