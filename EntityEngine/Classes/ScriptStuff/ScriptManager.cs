using DataModels.ScriptedEventStuff;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityEngine.Classes.ScriptStuff
{
    internal class ScriptManager
    {
        private Dictionary<string, ScriptedEvent> Scripts;
        public ScriptManager()
        {
            Scripts = new Dictionary<string, ScriptedEvent>();
        }
        public void LoadScripts(ContentManager content)
        {
            string basePath = content.RootDirectory + "/entities/Scripts";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string[] files = Directory.GetFiles(basePath);
            foreach (string file in files)
            {
                string jsonString = File.ReadAllText(file);
                ScriptedEvent sEvent = JsonSerializer.Deserialize<ScriptedEvent>(jsonString, options);
                Scripts.Add(sEvent.Name, sEvent);
            }


               
        }
        public ScriptedEvent GetScript(string scriptName) => Scripts[scriptName];
        public SubScript GetSubscript(string scriptName)
        {
            return Scripts[scriptName].Subscripts.First();
        }
    }
}
