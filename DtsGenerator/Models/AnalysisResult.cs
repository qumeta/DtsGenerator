using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.Models
{
    public class AnalysisResult<T> where T : class
    {
        public T Value { get; set; }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
