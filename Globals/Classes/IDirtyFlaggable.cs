using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Classes
{
    public interface IDirtyFlaggable
    {
        public bool FlaggedForRemoval { get; set; }
    }
}
