using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.CSharp
{
    public class CSharpPropertyModel
    {
        public CSharpPropertyModel()
        {
            Type = new CSharpTypeModel();
        }

        public string Name { get; set; }

        public CSharpTypeModel Type { get; set; }
    }
}
