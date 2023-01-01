using DataModels;
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
using TiledSharp;

namespace TiledEngine.Classes
{
    internal class TileLootManager
    {
        private Dictionary<int, TileLootData> s_foreGroundTileLootData;
        private Dictionary<int, TileLootData> s_backGroundTileLootData;
        private readonly TileSetPackage _tileSetPackage;

        public TileLootManager(TileSetPackage tileSetPackage)
        {
            s_foreGroundTileLootData = new Dictionary<int, TileLootData>();
            s_backGroundTileLootData = new Dictionary<int, TileLootData>();
            _tileSetPackage = tileSetPackage;
        }

        public void LoadContent(ContentManager content, TileSetPackage exteriorPackage)
        {

            string basePath = content.RootDirectory + "/Items";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var files = AssetLocator.GetFiles(basePath);
            string jsonString = string.Empty;

            List<TileLootData> foreGroundTileLootData = new List<TileLootData>();
            List<TileLootData> backGroundTileLootData = new List<TileLootData>();

            foreach (var file in files)
            {
                using (var stream = TitleContainer.OpenStream($"{AssetLocator.GetStaticFileDirectory(basePath)}{file}"))
                {
                    using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                    if (file.Contains("ForegroundTileLootData.json"))
                    {
                        var str = reader.ReadToEnd();

                        foreGroundTileLootData = JsonSerializer.Deserialize<List<TileLootData>>(str, options);

                    }
                    else if (file.Contains("BackgroundTileLootData.json"))
                    {
                        var str = reader.ReadToEnd();

                        backGroundTileLootData = JsonSerializer.Deserialize<List<TileLootData>>(str, options);

                    }
                }
            }

            //Offset foreground GID here to make it easy to fetch correct loot for GID at runtime

            s_foreGroundTileLootData = foreGroundTileLootData.ToDictionary(x => exteriorPackage.OffSetBackgroundGID(x.TileId), x => x);

            s_backGroundTileLootData = backGroundTileLootData.ToDictionary(x => x.TileId, x => x);
        }
        internal bool HasLootData(int tileId)
        {
            return HasBackgroundLootData(tileId) || HasForeGroundLootData(tileId);
        }
        internal bool HasForeGroundLootData(int tileId)
        {
            return s_foreGroundTileLootData.ContainsKey(tileId);
        }
        internal bool HasBackgroundLootData(int tileId)
        {
            return s_backGroundTileLootData.ContainsKey(tileId);
        }
        internal TileLootData GetLootData(int tileId)
        {
            if (HasBackgroundLootData(tileId))
                return s_backGroundTileLootData[tileId];
            else if (HasForeGroundLootData(tileId))
                return s_foreGroundTileLootData[tileId];


            //This section will check to see if tile is a non-central wanged tiled (left side of wall) has a central tile.
            //This is to avoid having to add loot for every single wanged version of a tile. This will just
            //grab the loot from the central tile instead
            TmxTilesetTile tmxTile = _tileSetPackage.GetTmxTileSetTile(tileId);
            if(tmxTile != null)
            {
                string property = null;
                tmxTile.Properties.TryGetValue("tilingSet", out property);
                if (!string.IsNullOrEmpty(property))
                {
                    int centralWangedGid = _tileSetPackage.WangManager.WangSets[property].GetWeightedvalue(15);
                    return GetLootData(centralWangedGid);
                }

            }
                throw new Exception($"No loot exists for tile with id {tileId}");

        }
    }
}
