using DtsGenerator.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScript
{
    public class TypeScriptImportModel
    {
        public string Name { get; set; }

        public string FilePath { get; set; }

        public DependencyKind DependencyKind { get; set; }
    }
}
