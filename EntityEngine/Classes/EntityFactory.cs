using DataModels;
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
        internal static Texture2D HatTexture;
        internal static Texture2D HairTexture;
        internal static Texture2D EyesTexture;
        internal static Texture2D HeadTexture;
        internal static Texture2D ShouldersTexture;

        internal static Texture2D ArmsTexture;
        internal static Texture2D ShirtTexture;
        internal static Texture2D PantsTexture;
        internal static Texture2D ShoesTexture;

        internal static Texture2D NPCSheet;

        internal static Texture2D Props_1;

        internal static List<Color> SkinColors;

        internal static Dictionary<string, NPCData> NPCData;

        internal static Dictionary<string, List<Schedule>> Schedules;

        private static ScriptManager _scriptManager;
        public static void Load(ContentManager content)
        {
            HatTexture = content.Load<Texture2D>("Entities/Hats");
            HairTexture = content.Load<Texture2D>("Entities/Hair");

            EyesTexture = content.Load<Texture2D>("Entities/Eyes");

            HeadTexture = content.Load<Texture2D>("Entities/Heads");
            ShouldersTexture = content.Load<Texture2D>("Entities/Shoulders");

            ArmsTexture = content.Load<Texture2D>("Entities/Arms");

            ShirtTexture = content.Load<Texture2D>("Entities/Shirts");

            PantsTexture = content.Load<Texture2D>("Entities/Pants");

            ShoesTexture = content.Load<Texture2D>("Entities/Shoes");

            NPCSheet = content.Load<Texture2D>("Entities/NPC/NPCSheet");

            Props_1 = content.Load<Texture2D>("Entities/Props/Props_1");

            SkinColors = new List<Color>()
            {
                new Color(141, 85, 36),
                new Color(198, 134, 66),
                new Color(224, 172, 105),
                new Color(241, 194, 125),
                new Color(255, 219, 172),

            };

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
        public static Color GetRandomSkinTone()
        {
            return SkinColors[Settings.Random.Next(0, SkinColors.Count)];
        }

    }
}
