using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScript
{
    public class TypeScriptPropertyModel
    {
        public TypeScriptPropertyModel()
        {
            Type = new TypeScriptTypeModel();
        }

        public string Name { get; set; }

        public bool IsOptional { get; set; }

        public TypeScriptTypeModel Type { get; set; }
    }
}
