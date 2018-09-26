using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.CSharp
{
    public class CSharpEnumModel
    {
        public CSharpEnumModel()
        {
            Members = new List<CSharpEnumMemberModel>();
        }

        public string Name { get; set; }

        public List<CSharpEnumMemberModel> Members { get; set; }
    }
}
