using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.Helpers
{
    public static class TypeHelper
    {
        public static bool IsSupportedType(ITypeSymbol typeSymbol)
        {
            var assemblyName = typeSymbol.ContainingAssembly.Name;

            //Type t;
            //t.IsPrimitive

            // netcoreapp3.0
            // netstandard兼容
            //if (assemblyName == "mscorlib" || assemblyName.StartsWith("System"))
            if (assemblyName == "netstandard" || assemblyName == "mscorlib" || assemblyName.StartsWith("System"))
            {
                if (typeSymbol.MetadataName == SupportedDotNetTypes.KeyValuePair)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsNullable(INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T;
        }
    }
}
