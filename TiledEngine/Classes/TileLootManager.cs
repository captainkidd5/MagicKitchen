using DataModels;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            List<TileLootData> tileLootData = content.Load<List<TileLootData>>("Items/ForegroundTileLootData");

            //Offset foreground GID here to make it easy to fetch correct loot for GID at runtime

            s_foreGroundTileLootData = tileLootData.ToDictionary(x => exteriorPackage.OffSetBackgroundGID(x.TileId), x => x);
            tileLootData = content.Load<List<TileLootData>>("Items/BackgroundTileLootData");

            s_backGroundTileLootData = tileLootData.ToDictionary(x => x.TileId, x => x);
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



            TmxTilesetTile tmxTile = _tileSetPackage.GetTmxTileSetTile(tileId);
            if(tmxTile != null)
            {
                string property = null;
                tmxTile.Properties.TryGetValue("tilingSet", out property);
                if (!string.IsNullOrEmpty(property))
                {
                    int centralWangedGid = _tileSetPackage.TilingSetManager.TilingSets[property][0];
                    return GetLootData(centralWangedGid);
                }

            }
                throw new Exception($"No loot exists for tile with id {tileId}");

        }
    }
}
