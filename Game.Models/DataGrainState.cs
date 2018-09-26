using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Models
{
    public partial class DataGrainState
    {
        public int GetAttr(EnumItemType attr)
        {
            return attrs[(int)attr];
        }

        public int SetAttr(EnumItemType attr, int value)
        {
            return attrs[(int)attr] = value;
        }
    }
}
