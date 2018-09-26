using DtsGenerator.Emitters;
using DtsGenerator.Mappers;
using DtsGenerator.TypeScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtsGenerator.Generators
{
    public class TypeGenerator
    {
        public string GetEmittedType(TypeScriptTypeModel typeModel)
        {
            var emittedType = EmittedTypeName.Any;

            // name: string;
            // attrs: number[];
            // items: { [index: number]: number };

            if (typeModel.ElementType == null)
            {
                if (typeModel.IsDictionary)
                {
                    emittedType = $"{{[index: {GetTypeLiteral(typeModel.TypeArguments[0])}]: {GetTypeLiteral(typeModel.TypeArguments[1])}}}";
                }
                else
                {
                    if (typeModel.TypeArguments.Any())
                    {
                        emittedType = $"{GetTypeLiteral(typeModel)}<{GetTypeLiteral(typeModel.TypeArguments.First())}>";
                    }
                    else
                    {
                        emittedType = GetTypeLiteral(typeModel);
                    }
                }
            }
            else
            {
                emittedType = GetTypeLiteral(typeModel.ElementType);
                emittedType += EmittedTypeName.Array;
            }

            return emittedType;
        }

        private string GetTypeLiteral(TypeScriptTypeModel typeModel)
        {
            return typeModel.IsNamedType
                    ? typeModel.Name
                    : TypeMapper.MapTypeScriptTypeToLiteral(typeModel.PredefinedType);
        }
    }
}
