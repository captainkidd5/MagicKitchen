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

namespace TiledEngine.Classes.Procedural
{
    internal class ProceduralPlacer
    {
        private PoissonSampler s_poissonSampler;

        private List<PoissonData> _poissonData;
        public ProceduralPlacer()
        {
        }

        public void Load(ContentManager content)
        {
            s_poissonSampler = new PoissonSampler(4, 12);

            string basePath = content.RootDirectory + "/maps/proceduraldata";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());


            var files = Directory.GetFiles(basePath);
            string jsonString = string.Empty;
            foreach (var file in files)
                if (file.EndsWith("PoissonData.json"))
                {
                    jsonString = File.ReadAllText(file);
                    _poissonData = JsonSerializer.Deserialize<List<PoissonData>>(jsonString, options);


                }

        }

        public void AddPoissonTiles(TileManager tileManager)
        {
            foreach(PoissonData poissonData in _poissonData)
            {
                s_poissonSampler.Generate(poissonData, DataModels.Enums.Layers.foreground, tileManager);
            }
        }
    }
}
