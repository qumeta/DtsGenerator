using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScriptDTO
{
    public class MethodModel
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public ParameterModel[] Parameters { get; set; }
    }
}
