using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.CSharp
{
    public class CSharpTypeModel
    {
        public CSharpTypeModel()
        {
            TypeArguments = new List<CSharpTypeModel>();
        }

        public string Name { get; set; }

        public bool IsArray { get; set; } // []

        public bool IsCollection { get; set; } // enumerable types such as List<T>, IList<T> or IEnumerable<T>

        public bool IsNullable { get; set; }

        public bool IsDictionary { get; set; } // Dictionary<int,int>

        public CSharpSpecialType SpecialType { get; set; }

        public CSharpTypeKind TypeKind { get; set; }

        public CSharpTypeModel ElementType { get; set; }

        public List<CSharpTypeModel> TypeArguments { get; set; }
    }
}
