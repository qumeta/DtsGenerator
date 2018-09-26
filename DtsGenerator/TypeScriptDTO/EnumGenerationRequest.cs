using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.TypeScriptDTO
{
    public class EnumGenerationRequest
    {
        public string OutputPath { get; set; }
        public EnumModel DataModel { get; set; }
    }
}
