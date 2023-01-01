using DataModels.QuestStuff;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModels.QuestStuff
{
    public class QuestLoader 
    {
       

        public Dictionary<string, Quest> AllQuests { get; private set; }
        public QuestLoader()
        {
        }

        public void Load(ContentManager content)
        {
            string basePath = content.RootDirectory + "/Quests";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var files = AssetLocator.GetFiles(basePath);
            string jsonString = string.Empty;
            foreach (var file in files)
                if (file.EndsWith("Quests.Json"))
                {
                    jsonString = File.ReadAllText(file);
                    AllQuests = JsonSerializer.Deserialize<List<Quest>>(jsonString, options).ToDictionary(x => x.Name);


                }

        }

    }
}
