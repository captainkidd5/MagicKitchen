using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOEngine.Classes
{
    public class FileLoadedEventArgs : EventArgs
    {
        public BinaryReader BinaryReader { get; set; }
    }
}
