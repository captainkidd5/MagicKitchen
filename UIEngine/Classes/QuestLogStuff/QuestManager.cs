using DataModels.QuestStuff;
using Globals.XPlatformHelpers;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;

namespace UIEngine.Classes.QuestLogStuff
{
    public class QuestManager
    {
        public Dictionary<string, Quest> AllQuests { get; private set; }
        public QuestManager()
        {
        }

        public void Load(ContentManager content)
        {
            string basePath = content.RootDirectory + "/Quests";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var files = AssetLocator.GetFiles(basePath);


            foreach (var file in files)
                if (file.EndsWith("Quests.Json"))
                {
                    using (var stream = TitleContainer.OpenStream($"{AssetLocator.GetStaticFileDirectory(basePath)}{file}"))
                    {
                        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        var str = reader.ReadToEnd();
                        AllQuests = JsonSerializer.Deserialize<List<Quest>>(str, options).ToDictionary(x => x.Name);

                    }


                }

        }
    }
}
