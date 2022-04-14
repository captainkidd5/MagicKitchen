using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes
{
    public static class Flags
    {
        public static bool ShowTileSelector { get; set; } = true;

        public static bool Pause { get; set; }

        public static bool DebugVelcro { get; 
            set; } = false;

        public static bool DebugGrid { get; set; } = false;
        public static bool ShowEntityPaths { get; set; } = false;
        public static bool EnableShadows { get; set; } = false;

        public static bool DisplayMousePosition { get; set; } = false;

        public static bool IsNightTime {get;set;} = false;



        public static string StagePlayerIn { get; set; }

        public static bool IsNewGame { get; set; }

        public static bool IsBootUp = true;
        

    }
}
