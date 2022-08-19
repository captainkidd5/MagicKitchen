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
    public static class LanguageManager
    {
        public static List<string> SupportedLanguages { get; set; }
        public static Language CurrentLanguage = new Language() { Name = "English" };


        public static void Load(ContentManager content)
        {
            string basePath = content.RootDirectory + "/Languages";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var files = Directory.GetFiles(basePath);
            string jsonString = string.Empty;

            string supportedLanguagesFile = files.FirstOrDefault(x => x.EndsWith("SupportedLangauges"));


            jsonString = File.ReadAllText(supportedLanguagesFile);
            SupportedLanguages = JsonSerializer.Deserialize<List<string>>(jsonString, options);

            string currentLanguage = SupportedLanguages[0];

            basePath = content.RootDirectory + $"/UI/Fonts/{currentLanguage}";
            files = Directory.GetFiles(basePath);
            string languageFile = files.FirstOrDefault(x => x.EndsWith("_Language.json"));
            jsonString = File.ReadAllText(languageFile);
            CurrentLanguage = JsonSerializer.Deserialize<Language>(jsonString, options);
        }
    }
}
