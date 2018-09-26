using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScript
{
    public class TypeScriptClassModel
    {
        public TypeScriptClassModel()
        {
            TypeParameters = new List<TypeScriptTypeParameterModel>();
            Imports = new List<TypeScriptImportModel>();
            Properties = new List<TypeScriptPropertyModel>();
        }

        public string Name { get; set; }

        public string BaseClass { get; set; }

        public bool IsGeneric { get; set; }

        public List<TypeScriptTypeParameterModel> TypeParameters { get; set; }

        public List<TypeScriptImportModel> Imports { get; set; }

        public List<TypeScriptPropertyModel> Properties { get; set; }
    }
}
