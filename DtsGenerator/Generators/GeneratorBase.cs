﻿using DtsGenerator.CSharp;
using DtsGenerator.TypeScript;
using DtsGenerator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtsGenerator.Generators
{
    public abstract class GeneratorBase
    {
        protected string AutogeneratedHeader_CSharpStyle()
        {
            var sb = new StringBuilder();

            sb.AppendLine("// ------------------------------------------------------------------------------");
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine("// \tThis code was generated by DtsGenerator.");
            sb.AppendLine("//");
            sb.AppendLine("// \tChanges to this file may cause incorrect behavior and will be lost if the code is regenerated.");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine("// ------------------------------------------------------------------------------");
            sb.AppendLine();

            return sb.ToString();
        }

        protected string AutogeneratedHeader_Compact()
        {
            var sb = new StringBuilder();

            sb.AppendLine("// ---------------------------");
            sb.AppendLine("// auto-generated by DtsGenerator");
            sb.AppendLine("// ---------------------------");
            sb.AppendLine();

            return sb.ToString();
        }


        protected string GenerateTypeParameters(List<TypeScriptTypeParameterModel> typeParameters)
        {
            return string.Join(", ", typeParameters.Select(tp => tp.Name).ToList());
        }

        protected void GenerateImportDeclarations(List<TypeScriptImportModel> imports, StringBuilder sb)
        {
            if (imports.Count > 0)
            {
                foreach (var import in imports)
                {
                    import.FilePath = NameCaseConverter.ToKebabCase(import.Name);

                    sb.AppendLine("import { "
                        + import.Name
                        + " } from '"
                        + (import.DependencyKind == DependencyKind.Model ? "./" : "../enums/")
                        + import.FilePath
                        + (import.DependencyKind == DependencyKind.Model ? ".model" : ".enum")
                        + "';");
                }

                sb.AppendLine();
            }
        }
    }
}
