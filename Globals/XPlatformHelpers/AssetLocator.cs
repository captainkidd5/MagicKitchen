using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.XPlatformHelpers
{
    public static class AssetLocator
    {
        public delegate string[] GetFilesDelegate(string path);

        public static GetFilesDelegate GetFiles;

    }
}
