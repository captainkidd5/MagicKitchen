using Globals.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOEngine.Classes
{
    public class SettingsFile
    {
        public bool MuteMusic { get; set; } = false;
        public float MusicVolume { get; set; } = 1f;

        public bool FullScreen { get; set; } = false;

        public  bool EnablePlayerDeath { get; set; } = false;
        public  bool EnablePlayerHurtSounds { get; set; } = true;
        public bool IsNightTime { get; set; } = true;

        public int CurrentScreenWidth { get; set; } = (int)Settings.NativeWidth;
        public int CurrentScreenHeight { get; set; } = (int)Settings.NativeHeight;

        public bool DebugVelcro { get; set; }
        public bool DebugGrid { get; set; }
        public static bool ShowEntityPaths { get; set; }

    }
}
