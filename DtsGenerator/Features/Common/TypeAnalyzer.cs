﻿using DtsGenerator.CSharp;
using DtsGenerator.Helpers;
using DtsGenerator.Mappers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtsGenerator.Features.Common
{
    public class TypeAnalyzer
    {
        private readonly IAnalysisContext _context;

        public TypeAnalyzer(IAnalysisContext context)
        {
            _context = context;
        }

        public CSharpTypeModel AnalyzeType(ITypeSymbol typeSymbol)
        {
            var typeModel = new CSharpTypeModel();

            typeModel.Name = typeSymbol.Name;
            typeModel.TypeKind = TypeMapper.MapTypeKind(typeSymbol.TypeKind);

            if (typeSymbol.SpecialType != SpecialType.None)
            {
                // special type
                HandleSpecialType(typeSymbol, typeModel);
            }
            else if (typeSymbol is IArrayTypeSymbol)
            {
                // array
                HandleArray(typeSymbol as IArrayTypeSymbol, typeModel);
            }
            else if (typeSymbol is INamedTypeSymbol)
            {
                // named type
                HandleNamedType(typeSymbol as INamedTypeSymbol, typeModel);
            }
            else if (typeSymbol is ITypeParameterSymbol)
            {
                // generic type parameter
                HandleTypeParameter(typeSymbol as ITypeParameterSymbol, typeModel);
            }

            return typeModel;
        }


        private void HandleSpecialType(ITypeSymbol typeSymbol, CSharpTypeModel typeModel)
        {
            typeModel.SpecialType = TypeMapper.MapSpecialType(typeSymbol.SpecialType);
        }

        private void HandleArray(IArrayTypeSymbol arrayTypeSymbol, CSharpTypeModel typeModel)
        {
            typeModel.IsArray = true;
            typeModel.ElementType = CreateTypeModel(arrayTypeSymbol.ElementType);
        }

        private void HandleNamedType(INamedTypeSymbol namedTypeSymbol, CSharpTypeModel typeModel)
        {
            if (TypeHelper.IsNullable(namedTypeSymbol))
            {
                // nullable types
                typeModel.IsNullable = true;

                if (!namedTypeSymbol.TypeArguments.IsDefaultOrEmpty)
                {
                    var typeArgument = namedTypeSymbol.TypeArguments[0];
                    typeModel.TypeArguments.Add(CreateTypeModel(typeArgument));
                }
            }

            var IEnumerableTypeSymbol = _context.Compilation.GetTypeByMetadataName("System.Collections.Generic.IEnumerable`1");

            if (namedTypeSymbol != null
                && (namedTypeSymbol.ConstructedFrom.Equals(IEnumerableTypeSymbol) || namedTypeSymbol.AllInterfaces.Any(i => i.ConstructedFrom.Equals(IEnumerableTypeSymbol))))
            {
                if (namedTypeSymbol.TypeArguments.Count() == 2)
                {
                    //System.Collections.Generic.Dictionary<>
                    typeModel.IsDictionary = true;
                    for (var i = 0; i < namedTypeSymbol.TypeArguments.Count(); i++)
                    {
                        var typeArgument = namedTypeSymbol.TypeArguments[i];
                        typeModel.TypeArguments.Add(CreateTypeModel(typeArgument));
                    }
                }
                else
                {
                    // enumerable types such as List<T>, IList<T> or IEnumerable<T>
                    typeModel.IsCollection = true;

                    if (!namedTypeSymbol.TypeArguments.IsDefaultOrEmpty)
                    {
                        var typeArgument = namedTypeSymbol.TypeArguments[0];

                        typeModel.TypeArguments.Add(CreateTypeModel(typeArgument));
                    }
                }
            }
            else if (!namedTypeSymbol.TypeArguments.IsDefaultOrEmpty)
            {
                // TODO what condition use this?
                for (var i = 0; i < namedTypeSymbol.TypeArguments.Count(); i++)
                {
                    var typeArgument = namedTypeSymbol.TypeArguments[i];
                    typeModel.TypeArguments.Add(CreateTypeModel(typeArgument));
                }
            }
        }

        private void HandleTypeParameter(ITypeParameterSymbol typeParameterSymbol, CSharpTypeModel typeModel)
        {

        }


        private CSharpTypeModel CreateTypeModel(ITypeSymbol typeSymbol)
        {
            return new CSharpTypeModel()
            {
                Name = typeSymbol.Name,
                TypeKind = TypeMapper.MapTypeKind(typeSymbol.TypeKind),
                SpecialType = TypeMapper.MapSpecialType(typeSymbol.SpecialType)
            };
        }
    }
}
