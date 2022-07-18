using DataModels;
using DataModels.NPCStuff;
using DataModels.ScriptedEventStuff;
using EntityEngine.Classes.ScriptStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static EntityEngine.Classes.CharacterStuff.Scheduler;

namespace EntityEngine.Classes
{

    public static class EntityFactory
    {



        internal static Dictionary<string, NPCData> NPCData { get; set; }
        //used for npc spawning
        internal static List<IWeightable> WeightedNPCData { get; set; }

        internal static Dictionary<string, List<Schedule>> Schedules;

        private static ScriptManager _scriptManager;
        public static void Load(ContentManager content)
        {
           
            

           

            string basePath = content.RootDirectory + "/entities/NPC";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            NPCData = new Dictionary<string, NPCData>();
            var files = Directory.GetFiles(basePath);
            string jsonString = string.Empty;
            foreach(var file in files)
                if (file.EndsWith(".json"))
                {
                     jsonString = File.ReadAllText(file);
                    var diction = JsonSerializer.Deserialize<List<NPCData>>(jsonString, options).ToDictionary(x => x.Name);
                    foreach(KeyValuePair<string, NPCData> pair in diction)
                    {
                        NPCData.Add(pair.Key,pair.Value);
                    }

                }

            WeightedNPCData = NPCData.Values.ToList().Cast<IWeightable>().ToList();
            basePath = content.RootDirectory + "/Entities/Schedules";



            Schedules = new Dictionary<string, List<Schedule>>();
            foreach(var file in Directory.GetFiles(basePath))
            {
                jsonString = File.ReadAllText(file);
                List<Schedule> schedules = JsonSerializer.Deserialize<List<Schedule>>(jsonString, options);

                foreach (Schedule sch in schedules)
                    sch.ConvertTimeString();

                schedules.Sort(0, schedules.Count, new ScheduleTimeComparer());
                Schedules.Add(Path.GetFileName(file).Split(".json")[0], schedules);

            }


            _scriptManager = new ScriptManager();
            _scriptManager.LoadScripts(content);
        }

        public static List<Schedule> GetSchedules(string entityName) => Schedules[entityName.ToLower()];
        public static SubScript GetSubscript(string scriptName) => _scriptManager.GetSubscript(scriptName);
       

    }
}
