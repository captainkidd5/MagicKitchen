using DataModels;
using DataModels.NPCStuff;
using DataModels.ScriptedEventStuff;
using EntityEngine.Classes.ScriptStuff;
using Globals.Classes;
using Globals.XPlatformHelpers;
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

namespace EntityEngine.Classes
{

    public static class EntityFactory
    {



        internal static Dictionary<string, NPCData> NPCData { get; set; }
        //used for npc spawning
        internal static List<IWeightable> WeightedNPCData { get; set; }


        private static ScriptManager _scriptManager;
        public static void Load(ContentManager content)
        {
           
            

           

            string basePath = content.RootDirectory + "/Entities/NPC";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            NPCData = new Dictionary<string, NPCData>();
            var files = AssetLocator.GetFiles(basePath);
            string jsonString = string.Empty;
            foreach(var file in files)
                if (file.EndsWith(".json"))
                {
                    using (var stream = TitleContainer.OpenStream($"{AssetLocator.GetStaticFileDirectory(basePath)}{file}"))
                    {
                        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        var str = reader.ReadToEnd();

                        var diction = JsonSerializer.Deserialize<List<NPCData>>(str, options).ToDictionary(x => x.Name);
                    foreach(KeyValuePair<string, NPCData> pair in diction)
                    {
                        if(pair.Value.NPCLightData != null)
                        {
                            pair.Value.NPCLightData.RadiusScale *= .01f;
                        }
                        NPCData.Add(pair.Key,pair.Value);
                    }
                    }

                }

            WeightedNPCData = NPCData.Values.ToList().Cast<IWeightable>().ToList();
            basePath = content.RootDirectory + "/Entities/Schedules";



           


            _scriptManager = new ScriptManager();
            _scriptManager.LoadScripts(content);
        }

        public static SubScript GetSubscript(string scriptName) => _scriptManager.GetSubscript(scriptName);
       

    }
}
