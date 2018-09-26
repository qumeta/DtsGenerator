using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.CSharp
{
    public class CSharpDependencyModel
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public DependencyKind DependencyKind { get; set; }
    }
}
