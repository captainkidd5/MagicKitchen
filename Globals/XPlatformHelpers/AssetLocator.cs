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

        public delegate string GetStaticDirectory(string path);
        public delegate string GetContentDirectory();

        //Set this in respective platform program.cs
        public static GetFilesDelegate GetFiles;


        //Set this in respective platform program.cs. This is for non xnb

        public static GetStaticDirectory GetStaticFileDirectory;


        public static GetContentDirectory GetContentFileDirectory;

    }
}
