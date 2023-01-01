using DataModels.MapStuff;
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

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class FurnitureLoader
    {
        public Dictionary<FurnitureType, FurnitureData> FurnitureData { get; private set; }

        public FurnitureLoader()
        {
           
        }

        public void LoadContent(ContentManager content)
        {
            string basePath = content.RootDirectory + "/Maps";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string jsonString = string.Empty;
            string[] files = AssetLocator.GetFiles(basePath);
            var file = files.FirstOrDefault(x => x.EndsWith("FurnitureData.json"));
            if (file == null)
                throw new Exception($"Unable to find furniture data!");
            using (var stream = TitleContainer.OpenStream($"{AssetLocator.GetStaticFileDirectory(basePath)}{file}"))
            {
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                var str = reader.ReadToEnd();

                FurnitureData = JsonSerializer.Deserialize<List<FurnitureData>>(str, options).
                    ToDictionary(x => x.FurnitureType);
            }
               

        }
    }
}
