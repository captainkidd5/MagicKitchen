using DataModels.QuestStuff;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModels
{
    public class CharacterData
    {

        public string Name { get; set; }

        public string StartingStage { get; set; }
        public int StartingTileX { get; set; }
        public int StartingTileY { get; set; }
        public List<Schedule> Schedules { get; set; }

        //Done in a separate file, too much clutter otherwise
        [JsonIgnore]
        public List<Quest> Quests { get; set; }

        public static CharacterData GetCharacterData(string characterSubDirectory, string npcName)
        {

            string npcDataName = characterSubDirectory + npcName + "data.json";
            string jsonString = File.ReadAllText(npcDataName);
            return JsonSerializer.Deserialize<CharacterData>(jsonString);

        }
    }
}
