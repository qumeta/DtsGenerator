using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScriptDTO
{
    public class PropertyModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsPrivate { get; set; }
        public object InitialValue { get; set; }
    }
}
