using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScriptDTO
{
    public class EnumModel
    {
        public string Name { get; set; }
        public EnumMemberModel[] Members { get; set; }
    }
}
