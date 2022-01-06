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
        [JsonIgnore]
        public bool Completed { get; set; }
        /// <summary>
        /// Name of the quest, must be unique
        /// </summary>
        public string QuestName { get; set; }
        /// <summary>
        /// Name of the NPC to talk to to get
        /// </summary>
        public string AcquiredFrom { get; set; }
        /// <summary>
        /// Name of the NPC to talk to once conditions are met
        /// </summary>
        public string TurnInto { get; set; }
        public List<PreRequisite> PreRequisites { get; set; }

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
