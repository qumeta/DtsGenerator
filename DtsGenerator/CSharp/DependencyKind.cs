using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.CSharp
{
    public enum DependencyKind
    {
        Model,
        Enum,
        Service // 本次只转换DTO，不转换服务，所以目前不用
    }
}
