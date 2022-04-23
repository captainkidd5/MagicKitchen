using DataModels;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes
{
    internal class TileLootManager
    {
        private Dictionary<int, TileLootData> s_foreGroundTileLootData;
        private Dictionary<int, TileLootData> s_backGroundTileLootData;

        public TileLootManager()
        {
            s_foreGroundTileLootData = new Dictionary<int, TileLootData>();
            s_backGroundTileLootData = new Dictionary<int, TileLootData>();
        }

        public void LoadContent(ContentManager content, TileSetPackage exteriorPackage)
        {
            List<TileLootData> tileLootData = content.Load<List<TileLootData>>("Items/ForegroundTileLootData");
            s_foreGroundTileLootData = tileLootData.ToDictionary(x => x.TileId, x => x);
            tileLootData = content.Load<List<TileLootData>>("Items/BackgroundTileLootData");

            //Offset background GID here to make it easy to fetch correct loot for GID at runtime
            s_backGroundTileLootData = tileLootData.ToDictionary(x => exteriorPackage.OffSetBackgroundGID(x.TileId), x => x);
        }

        internal bool HasLootData(int tileId)
        {
            return s_foreGroundTileLootData.ContainsKey(tileId);
        }
        internal TileLootData GetLootData(int tileId)
        {
            if (!HasLootData(tileId))
                throw new Exception($"No loot exists for tile with id {tileId}");
            return s_foreGroundTileLootData[tileId];
        }
    }
}
