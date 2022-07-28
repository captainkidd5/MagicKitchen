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
        internal Dictionary<string,WangSet> WangSets { get; private set; }

        internal Dictionary<int, string> TilingPairs { get; private set; }

        public WangManager()
        {
            WangSets = new Dictionary<string,WangSet>();
            TilingPairs = new Dictionary<int, string>();

        }

        /// <summary>
        /// For use with tile property "TilingKey". Each tiling set should have only a single central tile with this property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="gid">The central key</param>
        public void AddTilingKey(string property, int gid)
        {
            string[] values = property.Split(',');
            string name = values[0];

            if (WangSets.ContainsKey(name))
                return;
            string fillType = values[2];
            if (fillType == "back")
                WangSets[name].FillBackTiling(gid);
            else if (fillType == "tall")
                WangSets[name].FillTall(gid);

            if (!string.IsNullOrEmpty(values[1]))
                TilingPairs.Add(gid, values[1]);
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
        public bool IsPartOfSet(TileManager tileManager, string setName, int gid)
        {
            if (WangSets[setName].Set.Values.Any(x => x.Any(y => y.GID == tileManager.TileSetPackage.GetAdjustedGID(gid))))
                return true;
            return false;
        }
        //so, water should only wang if touching land tiles

        public int WangTile(TileManager tileManager, TileData tile)
        {
            //if (tile.Layer == Layers.foreground && tile.GID < 10000)
            //    tile.GID = (ushort)tileManager.TileSetPackage.OffSetBackgroundGID(tile.GID);
            if (string.IsNullOrEmpty(tile.GetProperty(tileManager.TileSetPackage, "tilingSet")))
                return tile.GID;
            string tilingSetValue = tile.GetProperty(tileManager.TileSetPackage, "tilingSet");

            if (tilingSetValue == "land")
                return tile.GID;
            if (!WangSets.ContainsKey(tilingSetValue))
                return tile.GID;
            WangSet tDictionary = WangSets[tilingSetValue];

            if (!GidDictionaryMatch(tDictionary, tileManager, tile, tile.X, tile.Y))
                return tile.GID;

            int keyToCheck = 0;
            Dictionary<int, int> secondaryDict = null;
            if (TilingPairs.ContainsKey(tDictionary[15]))
                secondaryDict = WangSets[TilingPairs[tDictionary[15]]];
            if(tile.Layer == (byte)Layers.background)
             return BackgroundTileWang(tileManager, tile, tDictionary, ref keyToCheck, secondaryDict);
            else
                return ForegroundTileWang(tileManager, tile, tDictionary, ref keyToCheck, secondaryDict);


            // }
            // return tile.GID;
        }

        private int BackgroundTileWang(TileManager tileManager, TileData tile, Dictionary<int, int> tDictionary, ref int keyToCheck, Dictionary<int, int> secondaryDict)
        {
            if (tileManager.Y_IsValidIndex(tile.Y - 1))
            {
                //or not equal to land
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X, tile.Y - 1) || (secondaryDict != null &&
                    !GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X, tile.Y - 1)))
                    keyToCheck += 1;
            }

            if (tileManager.Y_IsValidIndex(tile.Y + 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X, tile.Y + 1)
                    || (secondaryDict != null &&
                    !GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X, tile.Y + 1)))
                    keyToCheck += 8;
            }



            //looking at rightmost tile
            if (tileManager.X_IsValidIndex(tile.X + 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X + 1, tile.Y)
                    || (secondaryDict != null &&
                    !GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X + 1, tile.Y)))
                    keyToCheck += 4;
            }

            if (tileManager.X_IsValidIndex(tile.X - 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X - 1, tile.Y)
                    || (secondaryDict != null &&
                    !GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X - 1, tile.Y)))
                    keyToCheck += 2;
            }


            // if (keyToCheck < 15 && keyToCheck > 0)
            // {
            return tDictionary[keyToCheck];
        }

        private int ForegroundTileWang(TileManager tileManager, TileData tile, Dictionary<int, int> tDictionary, ref int keyToCheck, Dictionary<int, int> secondaryDict)
        {
            if (tileManager.Y_IsValidIndex(tile.Y - 1))
            {
                //or not equal to land
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X, tile.Y - 1) || (secondaryDict != null &&
                    GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X, tile.Y - 1)))
                    keyToCheck += 1;
            }

            if (tileManager.Y_IsValidIndex(tile.Y + 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X, tile.Y + 1)
                    || (secondaryDict != null &&
                    GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X, tile.Y + 1)))
                    keyToCheck += 8;
            }



            //looking at rightmost tile
            if (tileManager.X_IsValidIndex(tile.X + 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X + 1, tile.Y)
                    || (secondaryDict != null &&
                    GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X + 1, tile.Y)))
                    keyToCheck += 4;
            }

            if (tileManager.X_IsValidIndex(tile.X - 1))
            {
                if (GidDictionaryMatch(tDictionary, tileManager, tile, tile.X - 1, tile.Y)
                    || (secondaryDict != null &&
                    GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X - 1, tile.Y)))
                    keyToCheck += 2;
            }


            // if (keyToCheck < 15 && keyToCheck > 0)
            // {
            return tDictionary[keyToCheck] ;
        }
    }
}
