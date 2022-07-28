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
        internal Dictionary<string, WangSet> WangSets { get; private set; }

        internal Dictionary<int, string> TilingPairs { get; private set; }

        public WangManager()
        {
            WangSets = new Dictionary<string, WangSet>();
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
            WangSets.Add(name, new WangSet());

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
                        List<int> ids = WangTile(tileManager, neighborTile);
                      
                        if (!ids.Contains(neighborTile.GID))
                            tileManager.SwitchGID((ushort)ids[0], neighborTile);


                    }
                }
            }
        }
        private bool GidDictionaryMatch(WangSet wangSet, TileManager tileManager, TileData tile, int alteredX, int alteredY)
        {
            int gidToCheck = tileManager.GetTileDataFromPoint(
                new Point(alteredX, alteredY), (Layers)tile.Layer).Value.GID;

            if (wangSet.ContainsValue(gidToCheck))
                return true;

            return false;
        }
        public bool IsPartOfSet(TileManager tileManager, string setName, int gid)
        {
            if (WangSets[setName].ContainsValue(tileManager.TileSetPackage.GetAdjustedGID(gid)))
                return true;
            return false;
        }
        //so, water should only wang if touching land tiles

        public List<int> WangTile(TileManager tileManager, TileData tile)
        {
            List<int> returnedIds = new List<int>() { tile.GID };

            //if (tile.Layer == Layers.foreground && tile.GID < 10000)
            //    tile.GID = (ushort)tileManager.TileSetPackage.OffSetBackgroundGID(tile.GID);
            if (string.IsNullOrEmpty(tile.GetProperty(tileManager.TileSetPackage, "tilingSet")))
                return returnedIds;
            string tilingSetValue = tile.GetProperty(tileManager.TileSetPackage, "tilingSet");

            if (tilingSetValue == "land")
                return returnedIds;
            if (!WangSets.ContainsKey(tilingSetValue))
                return returnedIds;
            WangSet wangSet = WangSets[tilingSetValue];

            if (!GidDictionaryMatch(wangSet, tileManager, tile, tile.X, tile.Y))
                return returnedIds;

            int keyToCheck = 0;
            WangSet secondaryDict = null;
            foreach (WangTile wangTile in wangSet.Set[15])
            {
                if (TilingPairs.ContainsKey(wangTile.GID))
                {
                    secondaryDict = WangSets[TilingPairs[wangTile.GID]];
                    break;
                }
            }
            returnedIds.Clear();
     
            if (tile.Layer == (byte)Layers.background)
            {
               var list= BackgroundTileWang(tileManager, tile, wangSet, ref keyToCheck, secondaryDict);
                foreach (var item in list)
                    returnedIds.Add(item.GID);

            }
            else
            {
                var list = ForegroundTileWang(tileManager, tile, wangSet, ref keyToCheck, secondaryDict);
                foreach (var item in list)
                    returnedIds.Add(item.GID);
            }

            return returnedIds;
            // }
            // return tile.GID;
        }

        private List<WangTile> BackgroundTileWang(TileManager tileManager, TileData tile, WangSet wangSet, ref int keyToCheck, WangSet secondaryDict)
        {
            if (tileManager.Y_IsValidIndex(tile.Y - 1))
            {
                //or not equal to land
                if (GidDictionaryMatch(wangSet, tileManager, tile, tile.X, tile.Y - 1) || (secondaryDict != null &&
                    !GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X, tile.Y - 1)))
                    keyToCheck += 1;
            }

            if (tileManager.Y_IsValidIndex(tile.Y + 1))
            {
                if (GidDictionaryMatch(wangSet, tileManager, tile, tile.X, tile.Y + 1)
                    || (secondaryDict != null &&
                    !GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X, tile.Y + 1)))
                    keyToCheck += 8;
            }



            //looking at rightmost tile
            if (tileManager.X_IsValidIndex(tile.X + 1))
            {
                if (GidDictionaryMatch(wangSet, tileManager, tile, tile.X + 1, tile.Y)
                    || (secondaryDict != null &&
                    !GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X + 1, tile.Y)))
                    keyToCheck += 4;
            }

            if (tileManager.X_IsValidIndex(tile.X - 1))
            {
                if (GidDictionaryMatch(wangSet, tileManager, tile, tile.X - 1, tile.Y)
                    || (secondaryDict != null &&
                    !GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X - 1, tile.Y)))
                    keyToCheck += 2;
            }


            // if (keyToCheck < 15 && keyToCheck > 0)
            // {
            return wangSet.Set[keyToCheck];
        }

        private List<WangTile> ForegroundTileWang(TileManager tileManager, TileData tile, WangSet wangSet, ref int keyToCheck, WangSet secondaryDict)
        {
            if (tileManager.Y_IsValidIndex(tile.Y - 1))
            {
                //or not equal to land
                if (GidDictionaryMatch(wangSet, tileManager, tile, tile.X, tile.Y - 1) || (secondaryDict != null &&
                    GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X, tile.Y - 1)))
                    keyToCheck += 1;
            }

            if (tileManager.Y_IsValidIndex(tile.Y + 1))
            {
                if (GidDictionaryMatch(wangSet, tileManager, tile, tile.X, tile.Y + 1)
                    || (secondaryDict != null &&
                    GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X, tile.Y + 1)))
                    keyToCheck += 8;
            }



            //looking at rightmost tile
            if (tileManager.X_IsValidIndex(tile.X + 1))
            {
                if (GidDictionaryMatch(wangSet, tileManager, tile, tile.X + 1, tile.Y)
                    || (secondaryDict != null &&
                    GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X + 1, tile.Y)))
                    keyToCheck += 4;
            }

            if (tileManager.X_IsValidIndex(tile.X - 1))
            {
                if (GidDictionaryMatch(wangSet, tileManager, tile, tile.X - 1, tile.Y)
                    || (secondaryDict != null &&
                    GidDictionaryMatch(secondaryDict, tileManager, tile, tile.X - 1, tile.Y)))
                    keyToCheck += 2;
            }


            // if (keyToCheck < 15 && keyToCheck > 0)
            // {
            return wangSet.Set[keyToCheck];


        }
    }
}
