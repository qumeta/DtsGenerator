using DtsGenerator.CSharp;
using DtsGenerator.Emitters;
using DtsGenerator.TypeScript;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.Mappers
{
    public static class TypeMapper
    {
        public static CSharpSpecialType MapSpecialType(SpecialType specialType)
        {
            return (CSharpSpecialType)specialType;
        }

        public static CSharpTypeKind MapTypeKind(TypeKind typeKind)
        {
            return (CSharpTypeKind)typeKind;
        }

        public static string MapTypeScriptTypeToLiteral(TypeScriptBasicType type)
        {
            switch (type)
            {
                case TypeScriptBasicType.Any:
                    return EmittedTypeName.Any;
                case TypeScriptBasicType.Boolean:
                    return EmittedTypeName.Boolean;
                case TypeScriptBasicType.Number:
                    return EmittedTypeName.Number;
                case TypeScriptBasicType.String:
                    return EmittedTypeName.String;
                case TypeScriptBasicType.Array:
                    return EmittedTypeName.Array;
                case TypeScriptBasicType.Tuple:
                    return EmittedTypeName.Tuple;
                case TypeScriptBasicType.Enum:
                    return EmittedTypeName.Enum;
                case TypeScriptBasicType.Void:
                    return EmittedTypeName.Void;
                case TypeScriptBasicType.Null:
                    return EmittedTypeName.Null;
                case TypeScriptBasicType.Undefined:
                    return EmittedTypeName.Undefined;
                case TypeScriptBasicType.Never:
                    return EmittedTypeName.Never;
                case TypeScriptBasicType.Date:
                    return EmittedTypeName.Date;
                default:
                    return EmittedTypeName.Any;
            }
        }
    }
}
