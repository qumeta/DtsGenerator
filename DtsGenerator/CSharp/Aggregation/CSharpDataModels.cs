using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.CSharp.Aggregation
{
    public class CSharpDataModels
    {
        public CSharpDataModels()
        {
            Classes = new List<CSharpClassModel>();
            Enums = new List<CSharpEnumModel>();
        }

        public List<CSharpClassModel> Classes { get; set; }

        public List<CSharpEnumModel> Enums { get; set; }
    }
}
