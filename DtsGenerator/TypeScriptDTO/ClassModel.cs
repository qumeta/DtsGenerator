using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScriptDTO
{
    public class ClassModel
    {
        public string Name { get; set; }
        public string BaseClass { get; set; }
        public string[] Decorators { get; set; }
        public string[] TypeParameters { get; set; }
        public ImportModel[] Imports { get; set; }
        public PropertyModel[] Properties { get; set; }
        public ConstructorModel ConstructorDef { get; set; }
        public MethodModel[] Methods { get; set; }
    }
}
