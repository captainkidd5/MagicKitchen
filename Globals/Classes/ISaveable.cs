using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Classes
{
    public interface ISaveable
    {
        public void Save(BinaryWriter writer)
        {
        }
        public void LoadSave(BinaryReader reader)
        { }
    }
}
