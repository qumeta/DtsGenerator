using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScript
{
    public class TypeScriptEnumModel
    {
        public TypeScriptEnumModel()
        {
            Members = new List<TypeScriptEnumMemberModel>();
        }

        public string Name { get; set; }

        public List<TypeScriptEnumMemberModel> Members { get; set; }
    }
}
