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

namespace TiledEngine.Classes.Procedural
{
    internal class ProceduralPlacer
    {
        private PoissonSampler _poissonSampler;
        private ClusterSampler _clusterSampler;

        private List<TileGenerationData> _poissonData;
        public ProceduralPlacer()
        {
        }

        public void Load(ContentManager content)
        {
            _poissonSampler = new PoissonSampler();
            _clusterSampler = new ClusterSampler();

            string basePath = content.RootDirectory + "/Maps/ProceduralData";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());


            var files = AssetLocator.GetFiles(basePath);
            string jsonString = string.Empty;
            foreach (var file in files)
                if (file.EndsWith("PoissonData.json"))
                {
                    using (var stream = TitleContainer.OpenStream($"{AssetLocator.GetStaticFileDirectory(basePath)}{file}"))
                    {
                        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        var str = reader.ReadToEnd();
                        _poissonData = JsonSerializer.Deserialize<List<TileGenerationData>>(str, options);

                    }


                }

            foreach(TileGenerationData data in _poissonData)
            {
                if (data.MinDistance > data.MaxDistance)
                    throw new Exception($"procedural data with gid {data.GID} muts have max distance equal to or greater than min distance");
                data.GID++;
                if(data.LayersToPlace == DataModels.Enums.Layers.foreground)
                {
                    data.GID += 10000;
                }
            }

        }

        public void AddPoissonTiles(TileManager tileManager)
        {
            foreach(TileGenerationData poissonData in _poissonData)
            {
                _poissonSampler.Generate(poissonData, DataModels.Enums.Layers.foreground, tileManager);
            }
        }

        public void AddClusterTiles(TileManager tileManager)
        {
            foreach (TileGenerationData poissonData in _poissonData)
            {
                _clusterSampler.Generate(poissonData, DataModels.Enums.Layers.foreground, tileManager);
            }
        }
    }
}
