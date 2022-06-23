using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.TileAddons;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TilePlacementStuff.TilingStuff
{
    internal class WangManager
    {
        internal Dictionary<string, Dictionary<int, int>> TilingSets { get; private set; }

      
        public WangManager()
        {
            TilingSets = new Dictionary<string, Dictionary<int, int>>();
        }

        public void AddNewSet(string name, int gid)
        {
        
            if (TilingSets.ContainsKey(name))
                return;
            TilingSets[name] = FillTilingDictionary(gid);
        }
        private Dictionary<int, int> FillTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID -98},{1, centralGID + 3}, {2,  centralGID + 102 },  {3, centralGID + 101}, {4, centralGID + 2}, {5, centralGID + 99},{6,centralGID - 96},
                { 7, centralGID + 100}, {8, centralGID + 103}, {9, centralGID - 97}, {10, centralGID - 99}, {11, centralGID + 1},
                { 12,centralGID - 101}, {13,centralGID - 1}, {14,centralGID - 100}, {15, centralGID}
            };
        }

      
        public void WangSorroundingTiles(TileManager tileManager, TileData tileData)
        {

            for (int i = tileData.X - 1; i <= tileData.X + 1; i++)
            {
                for (int j = tileData.Y - 1; j <= tileData.Y + 1; j++)
                {
                    if (tileManager.X_IsValidIndex(i) && tileManager.Y_IsValidIndex(j))
                    {
                       
 
                            TileData neighborTile = tileManager.GetTileDataFromPoint(new Point(i, j), (Layers)tileData.Layer).Value;
                            int newGid = WangTile(tileManager, neighborTile);
                        if (newGid != neighborTile.GID)
                            tileManager.SwitchGID((ushort)newGid, neighborTile);
                        

                    }
                }
            }
        }
        private bool GidDictionaryMatch(Dictionary<int, int> dict,TileManager tileManager, TileData tile, int alteredX, int alteredY)
        {
            int gidToCheck = tileManager.GetTileDataFromPoint(
                new Point(alteredX, alteredY), (Layers)tile.Layer).Value.GID;

            if (dict.Values.Any(x => x == gidToCheck))
                return true;
            return false;
        }

        public int WangTile(TileManager tileManager, TileData tile)
        {

            if(string.IsNullOrEmpty(tile.GetProperty(tileManager.TileSetPackage, "tilingSet")))
                return tile.GID;
            string tilingSetValue = tile.GetProperty(tileManager.TileSetPackage, "tilingSet");
            if (tilingSetValue == "land")
                return tile.GID;
            Dictionary<int, int> tDictionary = TilingSets[tilingSetValue];

            if (!GidDictionaryMatch(tDictionary, tileManager, tile, tile.X, tile.Y))
                return tile.GID;

            int keyToCheck = 0;

            if (tileManager.Y_IsValidIndex(tile.Y - 1))
            {
                if (GidDictionaryMatch(tDictionary,tileManager, tile, tile.X, tile.Y - 1))
                    keyToCheck += 1;
            }

            if (tileManager.Y_IsValidIndex(tile.Y + 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X, tile.Y + 1))
                    keyToCheck += 8;
            }



            //looking at rightmost tile
            if (tileManager.X_IsValidIndex(tile.X + 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X + 1, tile.Y))
                    keyToCheck += 4;
            }

            if (tileManager.X_IsValidIndex(tile.X - 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X - 1, tile.Y))
                    keyToCheck += 2;
            }


           // if (keyToCheck < 15 && keyToCheck > 0)
           // {
                return tDictionary[keyToCheck];

           // }
           // return tile.GID;
        }
    }
}
