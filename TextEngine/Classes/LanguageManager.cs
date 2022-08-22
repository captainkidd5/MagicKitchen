using DataModels.DialogueStuff;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    public delegate void LanguageChanged();
    public static class LanguageManager
    {

        private static ContentManager s_content;

        public static List<string> SupportedLanguages { get; private set; }
        public static Language CurrentLanguage { get; private set; }
        public static LanguageChanged LanguageChanged;


        public static void Load(ContentManager content)
        {
            s_content = content;
            string basePath = content.RootDirectory + "/Languages";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var files = Directory.GetFiles(basePath);
            string jsonString = string.Empty;

            string supportedLanguagesFile = files.FirstOrDefault(x => x.EndsWith("SupportedLanguages.json"));


            jsonString = File.ReadAllText(supportedLanguagesFile);
            SupportedLanguages = JsonSerializer.Deserialize<List<string>>(jsonString, options);

            string currentLanguage = SupportedLanguages[0];

            ChangeLanguage(currentLanguage);
          
        }

        public static void ChangeLanguage(string language)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string path = s_content.RootDirectory + $"/UI/Fonts/{language}";
            var files = Directory.GetFiles(path);
            string languageFile = files.FirstOrDefault(x => x.EndsWith("_Language.json"));
            var jsonString = File.ReadAllText(languageFile);
            CurrentLanguage = JsonSerializer.Deserialize<Language>(jsonString, options);
            LanguageChanged.Invoke();
        }
    }
}
