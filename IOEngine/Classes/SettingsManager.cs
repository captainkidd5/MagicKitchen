using Globals.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IOEngine.Classes
{
    public static class SettingsManager
    {
        private static string _settingsPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Preferences\";
        private static string _fileName => "Settings.json";

        private static string _fullPath => _settingsPath + _fileName;

            public static SettingsFile SettingsFile { get; private set; }

        public static bool Mute { get { return SettingsFile.MuteMusic; } set { 
                SettingsFile.MuteMusic = value;
            } }
        public static bool FullScreen { get { return SettingsFile.FullScreen; } set {
                SettingsFile.FullScreen = value; 
                Settings.ToggleFullscreen(value);
            } }

        public static bool EnablePlayerDeath
        {
            get { return SettingsFile.EnablePlayerDeath; }
            set
            {
                SettingsFile.EnablePlayerDeath = value;
            }
        }
        public static bool EnablePlayerHurtSounds
        {
            get { return SettingsFile.EnablePlayerHurtSounds; }
            set
            {
                SettingsFile.EnablePlayerHurtSounds = value;
            }
        }
        public static bool IsNightTime
        {
            get { return SettingsFile.IsNightTime; }
            set
            {
                SettingsFile.IsNightTime = value;
            }
        }

        public static bool DebugVelcro
        {
            get { return SettingsFile.DebugVelcro; }
            set
            {
                SettingsFile.DebugVelcro = value;
            }
        }

        public static bool DebugGrid
        {
            get { return SettingsFile.DebugGrid; }
            set
            {
                SettingsFile.DebugGrid = value;
            }
        }
        public static bool ShowEntityPaths
        {
            get { return SettingsFile.ShowEntityPaths; }
            set
            {
                SettingsFile.ShowEntityPaths = value;
            }
        }

        public static Dictionary<string,bool> DebuggableCategories
        {
            get { return SettingsFile.DebuggableCategories; }
            set
            {
                SettingsFile.DebuggableCategories = value;
            }
        }

        public static float CameraZoom
        {
            get { return SettingsFile.CameraZoom; }
            set { SettingsFile.CameraZoom = value; Settings.CameraZoom = value; }
        }

        public static bool AllowNPCSpawning
        {
            get { return SettingsFile.AllowNPCSpawning; }
            set
            {
                SettingsFile.AllowNPCSpawning = value;
                Flags.AllowNPCSpawning = value;
            }
        }
        public static void LoadSettings()
        {
            SettingsFile = new SettingsFile();
            if (!Directory.Exists(_settingsPath))
            {
                Directory.CreateDirectory(_settingsPath);
            }
            if (!File.Exists(_fullPath))
            {
                SaveSettings();
            }
            using FileStream openStrean = File.OpenRead($"{_fullPath}");
                SettingsFile = JsonSerializer.Deserialize<SettingsFile>(openStrean);
            openStrean.Dispose();

            FullScreen = SettingsFile.FullScreen;
            Settings.CameraZoom = SettingsFile.CameraZoom;
            Flags.AllowNPCSpawning = SettingsFile.AllowNPCSpawning;
        }

        public static void SaveSettings()
        {
            //Save current screen width and height so when fullscreen is exited it is remembered
            SettingsFile.CurrentScreenWidth = Settings.ScreenWidth;
            SettingsFile.CurrentScreenHeight = Settings.ScreenHeight;
            SettingsFile.CameraZoom = Settings.CameraZoom;
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            using var stream = File.Create(_fullPath);
            JsonSerializer.Serialize(stream, SettingsFile, options);
            stream.Dispose();
        }
    }
}
