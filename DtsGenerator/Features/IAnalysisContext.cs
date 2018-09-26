using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.Features
{
    public interface IAnalysisContext
    {
        //AdhocWorkspace Workspace { get; }

        Solution Solution { get; }

        //Project Project { get; }

        Compilation Compilation { get; }

        void Init(string path);
    }
}
