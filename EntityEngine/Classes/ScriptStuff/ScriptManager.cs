using DataModels.ScriptedEventStuff;
using Globals.XPlatformHelpers;
using Microsoft.Xna.Framework;
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
            string basePath = content.RootDirectory + "/Entities/Scripts";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string[] files = AssetLocator.GetFiles(basePath);
            foreach (string file in files)
            {
                using (var stream = TitleContainer.OpenStream($"{AssetLocator.GetStaticFileDirectory(basePath)}{file}"))
                {
                    using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    var str = reader.ReadToEnd();
            
                    ScriptedEvent sEvent = JsonSerializer.Deserialize<ScriptedEvent>(str, options);
                    Scripts.Add(sEvent.Name, sEvent);
                }
            }

            foreach(ScriptedEvent sa in Scripts.Values)
            {
                sa.ValidateScripts();
            }


        }
        public ScriptedEvent GetScript(string scriptName) => Scripts[scriptName];
        public SubScript GetSubscript(string scriptName)
        {
            return Scripts[scriptName].Subscripts.First();
        }
    }
}
