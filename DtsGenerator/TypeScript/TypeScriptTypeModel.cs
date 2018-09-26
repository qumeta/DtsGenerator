using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScript
{
    public class TypeScriptTypeModel
    {
        public TypeScriptTypeModel()
        {
            TypeArguments = new List<TypeScriptTypeModel>();
        }

        public string Name { get; set; }

        public bool IsNamedType { get; set; }
        public bool IsDictionary { get; set; }

        public TypeScriptBasicType PredefinedType { get; set; }

        public TypeScriptTypeModel ElementType { get; set; }

        public List<TypeScriptTypeModel> TypeArguments { get; set; }
    }
}
