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

        public static bool Mute { get { return SettingsFile.MuteMusic; } set { SettingsFile.MuteMusic = value; } }
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
        }

        public static void SaveSettings()
        {
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
