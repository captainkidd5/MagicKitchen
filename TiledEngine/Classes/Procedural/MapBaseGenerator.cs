using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;
using TiledEngine.Classes.TileAddons;
using TiledSharp;
using Microsoft.Xna.Framework;

namespace TiledEngine.Classes.Procedural
{
    internal class MapBaseGenerator
    {
        private List<TileData[,]> GenerateEmptyMapArray(TileManager tileManager, TmxMap map, int totalMapBounds)
        {

            List<TileData[,]> tilesToReturn = new List<TileData[,]>();

            for (int z = 0; z < MapLoader.MapDepths.Length; z++)
            {
                tilesToReturn.Add(new TileData[totalMapBounds, totalMapBounds]);

                for (int x = 0; x < totalMapBounds; x++)
                {
                    for (int y = 0; y < totalMapBounds; y++)
                    {
                        tilesToReturn[z][x, y] = new TileData(0, (ushort)x, (ushort)y, (Layers)z);

                    }
                }
            }
            return tilesToReturn;
        }

        private static TileData[,] GenerateAutomataLayer(int mapWidth)
        {
            TileData[,] tileDataToReturn = new TileData[mapWidth, mapWidth];
            bool[,] automata = CellularAutomataGenerator.GenerateMap(mapWidth);
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    ushort gid = 0;
                    if (automata[i, j])
                        gid = 1121;
                    else
                        gid = 723;

                    tileDataToReturn[i, j] = new TileData(gid, (ushort)i, (ushort)j, 0);


                }
            }

            return tileDataToReturn;
        }

        private static void InsertCustomMapAt(List<TileData[,]> fullMapData, Point insertionPoint, TmxMap map)
        {
            List<TmxLayer> allLayers = new List<TmxLayer>()
            {
                map.TileLayers["background"],
            map.TileLayers["midground"],
           map.TileLayers["buildings"],
           map.TileLayers["foreground"],
           map.TileLayers["front"]
        };

            for (int i = 0; i < MapLoader.MapDepths.Length; i++)
            {
                foreach (TmxLayerTile layerNameTile in allLayers[i].Tiles)
                {

                    fullMapData[i][insertionPoint.X + layerNameTile.X, insertionPoint.Y + layerNameTile.Y] =
                                          new TileData((ushort)layerNameTile.Gid, (ushort)(insertionPoint.X + layerNameTile.X),
                                          (ushort)(insertionPoint.Y + layerNameTile.Y), (Layers)i);


                }

            }
        }
    }
}
