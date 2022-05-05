using DataModels.MapStuff;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class FurnitureLoader
    {
        private Dictionary<FurnitureType, FurnitureData> _furnitureData;

        public FurnitureLoader()
        {
           
        }

        public void LoadContent(ContentManager content)
        {
            string basePath = content.RootDirectory + "/maps";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string jsonString = string.Empty;

            jsonString = File.ReadAllText(basePath + "/FurnitureData.json");


            _furnitureData = JsonSerializer.Deserialize<List<FurnitureData>>(jsonString, options).
                ToDictionary(x => x.FurnitureType);



                

        }
    }
}
