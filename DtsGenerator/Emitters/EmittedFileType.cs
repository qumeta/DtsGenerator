using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DtsGenerator.Emitters
{
    public enum EmittedFileType
    {
        [Description("model")]
        Model,

        [Description("enum")]
        Enum,

        [Description("service")]
        Service
    }
}
