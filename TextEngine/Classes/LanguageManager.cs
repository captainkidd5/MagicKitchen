using DataModels.DialogueStuff;
using DataModels.JsonConverters;
using Globals.XPlatformHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var files = AssetLocator.GetFiles(basePath);
            string jsonString = string.Empty;

            string supportedLanguagesFile = files.FirstOrDefault(x => x.EndsWith("SupportedLanguages.json"));
            using(var stream = TitleContainer.OpenStream(basePath + "/" + supportedLanguagesFile))
            {
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                var str = reader.ReadToEnd();
                SupportedLanguages = JsonSerializer.Deserialize<List<string>>(str, options);

            }

            //jsonString = TitleContainer.OpenStream(basePath + "/" + supportedLanguagesFile)

            string currentLanguage = SupportedLanguages[0];

            ChangeLanguage(currentLanguage);
          
        }

        public static void ChangeLanguage(string language)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string path = s_content.RootDirectory + $"/UI/Fonts/{language}";
            var files = AssetLocator.GetFiles(path);

            string languageFile = files.FirstOrDefault(x => x.EndsWith("_Language.json"));
            using (var stream = TitleContainer.OpenStream(
                s_content.RootDirectory + $"/UI/Fonts/{language}/{languageFile}"))
            {
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                var str = reader.ReadToEnd();
                CurrentLanguage = JsonSerializer.Deserialize<Language>(str, options);
                LanguageChanged.Invoke();

            }
           
        }
    }
}
