using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.Features
{
    public abstract class AnalysisContextBase : IAnalysisContext
    {
        protected AdhocWorkspace _workspace;
        protected Solution _solution;
        protected Project _project;
        protected Compilation _compilation;

        //public AdhocWorkspace Workspace => _workspace;

        public Solution Solution => _solution;

        //public Project Project => _project;

        public Compilation Compilation => _compilation;

        public AnalysisContextBase()
        {
            _workspace = new AdhocWorkspace();
        }

        public virtual void Init(string path)
        {
        }
    }
}
