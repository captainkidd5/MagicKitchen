using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModels.QuestStuff
{
    
    public class Quest
    {
        public string Name { get; set; }
        public List<PreRequisite> PreRequisites { get; set; }

        public int CurrentStep { get; set; }
        public Dictionary<int, QuestStep> Steps { get; set; }

        public bool Completed => CurrentStep == Steps.Count;

        public static List<Quest> GetQuests(string characterSubDirectory)
        {
            List<Quest> quests = new List<Quest>();
            string[] allCharacterFiles = Directory.GetFiles(characterSubDirectory);
            string directoryName = new DirectoryInfo(characterSubDirectory).Name;
            foreach(string file in allCharacterFiles)
            {
                //All quest files begin with 'Q_'

                string fileNameOnly = file.Split(directoryName + "/")[1];
                if(fileNameOnly[0] == 'Q' && fileNameOnly[1] == '_')
                {
                    string jsonString = File.ReadAllText(file);

                    quests.Add(JsonSerializer.Deserialize<Quest>(jsonString));
                }
            }
            return quests;
        }

    }
}
