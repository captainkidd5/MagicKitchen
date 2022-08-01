using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes
{
    public static class Flags
    {
        //Will show "developed by" screen if true
        public static bool DisplaySplashScreens = true;
        public static bool ShowTileSelector { get; set; } = true;

        public static bool SpawnCharactersOnNewGame = true;
        public static bool Pause { get; set; }

        public static bool DebugVelcro { get; set; } = false;
        public static bool DisplayPlayAreaCollisions { get; set; } = false;
        public static bool DebugGrid { get; set; } = false;
        public static bool ShowEntityPaths { get; set; } = false;

        public static bool EnablePlayerDeath { get; set; } = false;

        public static bool DisplayMousePosition { get; set; } = false;
        public static bool DisplayFPS { get; set; } = false;


        public static bool IsNightTime {get;set;} = true;

        public static bool SpawnFlotsam { get; set; } = true;


        public static bool IsNewGame { get; set; }

        private static bool _firstBootUp = true;

        public static bool EnablePlayerHurtSounds { get; set; } = true;

        //Useful for things like registering commands, where commands should only be registered once ber game boot up, and then never again
        public static bool FirstBootUp { get { return _firstBootUp; } set 
            {
                if (value)
                { throw new Exception($"May not set first boot up to true"); }
                _firstBootUp = value; } }

    }
}
