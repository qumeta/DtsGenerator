using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScriptDTO
{
    public class ClassGenerationRequest
    {
        public string OutputPath { get; set; }
        public ClassModel DataModel { get; set; }
    }
}
