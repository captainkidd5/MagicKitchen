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
    public class QuestManager 
    {
       

        private Dictionary<string, Quest> AllQuests { get; set; }
        public QuestManager()
        {
        }

        public void Load(ContentManager content)
        {
            string basePath = content.RootDirectory + "/Quests";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var files = Directory.GetFiles(basePath);
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
