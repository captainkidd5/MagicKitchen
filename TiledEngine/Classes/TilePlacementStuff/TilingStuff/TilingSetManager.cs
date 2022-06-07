using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TilePlacementStuff.TilingStuff
{
    internal class TilingSetManager
    {
        internal Dictionary<string, Dictionary<int, int>> TilingSets { get; private set; }

      
        public TilingSetManager()
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

        private bool GidDictionaryMatch(Dictionary<int, int> dict, Tile tile, int alteredX, int alteredY)
        {
            if (dict.Values.Any(x => x == tile.TileManager.GetTileFromPoint(
                new Point(alteredX, alteredY), tile.IndexLayer).GID))
                return true;
            return false;
        }

        public void WangSorroundingTiles(Tile tile)
        {

            for (int i = tile.X - 1; i <= tile.X + 1; i++)
            {
                for (int j = tile.Y - 1; j <= tile.Y + 1; j++)
                {
                    if (tile.TileManager.X_IsValidIndex(i) && tile.TileManager.Y_IsValidIndex(j))
                    {
                       

                            Tile neighborTile = tile.TileManager.GetTileFromPoint(new Point(i, j), tile.IndexLayer);
                            int newGid = WangTile(neighborTile);
                            if (newGid != neighborTile.GID)
                                TileUtility.SwitchGid(neighborTile, tile.IndexLayer, newGid);
                        

                    }
                }
            }
        }
        public int WangTile(Tile tile)
        {

            if(string.IsNullOrEmpty(tile.GetProperty("tilingSet")))
                return tile.GID;

            Dictionary<int, int> tDictionary = TilingSets[tile.GetProperty("tilingSet")];

            if (!GidDictionaryMatch(tDictionary, tile, tile.X, tile.Y))
                return tile.GID;

            int keyToCheck = 0;

            if (tile.TileManager.Y_IsValidIndex(tile.Y - 1))
            {
                if (GidDictionaryMatch(tDictionary, tile, tile.X, tile.Y - 1))
                    keyToCheck += 1;
            }

            if (tile.TileManager.Y_IsValidIndex(tile.Y + 1))
            {
                if (GidDictionaryMatch(tDictionary, tile, tile.X, tile.Y + 1))
                    keyToCheck += 8;
            }



            //looking at rightmost tile
            if (tile.TileManager.X_IsValidIndex(tile.X + 1))
            {
                if (GidDictionaryMatch(tDictionary, tile, tile.X + 1, tile.Y))
                    keyToCheck += 4;
            }

            if (tile.TileManager.X_IsValidIndex(tile.X - 1))
            {
                if (GidDictionaryMatch(tDictionary, tile, tile.X - 1, tile.Y))
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
