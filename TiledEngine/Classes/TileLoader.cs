using DataModels;
using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TiledSharp;
using Penumbra;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.IO;
using static Globals.Classes.Settings;
using System.Linq;
using TiledEngine.Classes.ZoneStuff;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using static DataModels.Enums;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.Procedural;

namespace TiledEngine.Classes
{
    public static class TileLoader
    {
        public static float[] MapDepths = new float[5]
            {
                .01f,
                .1f,
                .2f,
                .3f,
                .5f,
            };

        private static string s_mapPath;


        internal static TileSetPackage TileSetPackage;
        internal static TileLootManager TileLootManager;



        private static ProceduralPlacer s_proceduralPlacer;

        public static FurnitureLoader FurnitureLoader;

        // <summary>
        /// This should only be called ONCE per save file.
        /// </summary>
        public static void LoadContent(ContentManager content)
        {
            s_proceduralPlacer = new ProceduralPlacer();
            s_proceduralPlacer.Load(content);

            s_mapPath = content.RootDirectory + "/Maps/";
            TmxMap worldMap = new TmxMap(s_mapPath + "LullabyTown.tmx");
            TileSetPackage = new TileSetPackage(worldMap);

            TileSetPackage.LoadContent(content, "maps/BackgroundMasterSpriteSheet_Spaced", "maps/ForegroundMasterSpriteSheet");




            TileLootManager = new TileLootManager(TileSetPackage);
            TileLootManager.LoadContent(content, TileSetPackage);

            FurnitureLoader = new FurnitureLoader();
            FurnitureLoader.LoadContent(content);
        }



        /// <summary>
        /// This should only be called ONCE per stage, per save file.
        /// First load loads all the tiles in from the TMX maps, from then on
        /// all the tiles will be loaded with the binary reader
        /// </summary>
        public static void CreateNewSave(Dictionary<string, StageData> stageData, TileManager tileManager, ContentManager content)
        {
            TmxMap worldMap = new TmxMap(s_mapPath + stageData["LullabyTown"].Path);
            TmxMap town = new TmxMap(s_mapPath + stageData["HomeIsland"].Path);

            List<TileData[,]> mapData;// = ExtractTilesFromPreloadedMap(tileManager, mapToLoad);
            mapData = GenerateEmptyMapArray(tileManager, worldMap, 512);
            mapData[0] = GenerateAutomataLayer(512);
            InsertCustomMapAt(mapData, new Point(112, 112), worldMap);
            InsertCustomMapAt(mapData, new Point(24, 24), town);

            tileManager.LoadMap(worldMap, mapData, 512, TileSetPackage);

            s_proceduralPlacer.AddClusterTiles(tileManager);
            //s_proceduralPlacer.AddPoissonTiles(tileManager);

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

            for (int i = 0; i < MapDepths.Length; i++)
            {
                foreach (TmxLayerTile layerNameTile in allLayers[i].Tiles)
                {
             
                    fullMapData[i][insertionPoint.X + layerNameTile.X, insertionPoint.Y + layerNameTile.Y] =
                                          new TileData((ushort)layerNameTile.Gid, (ushort)(insertionPoint.X + layerNameTile.X),
                                          (ushort)(insertionPoint.Y + layerNameTile.Y), (Layers)i);


                }

            }
        }
        /// <summary>
        /// Generates the first iteration of the background layer of the entire map
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Create new tiles based on tiles found in TMX map file. This should
        /// only be done once per map per save.
        /// </summary>
        private static List<TileData[,]> ExtractTilesFromPreloadedMap(TileManager tileManager, TmxMap map)
        {
            List<TmxLayer> allLayers = new List<TmxLayer>()
            {
                map.TileLayers["background"],
            map.TileLayers["midground"],
           map.TileLayers["buildings"],
           map.TileLayers["foreground"],
           map.TileLayers["front"]
        };
            List<TileData[,]> tilesToReturn = new List<TileData[,]>();

            for (int i = 0; i < MapDepths.Length; i++)
            {
                tilesToReturn.Add(new TileData[map.Width, map.Width]);
                foreach (TmxLayerTile layerNameTile in allLayers[i].Tiles)
                    tilesToReturn[i][layerNameTile.X, layerNameTile.Y] = new TileData((ushort)layerNameTile.Gid, (ushort)layerNameTile.X, (ushort)layerNameTile.Y, (Layers)i);
            }
            return tilesToReturn;
        }

        private static List<TileData[,]> GenerateEmptyMapArray(TileManager tileManager, TmxMap map, int totalMapBounds)
        {

            List<TileData[,]> tilesToReturn = new List<TileData[,]>();

            for (int z = 0; z < MapDepths.Length; z++)
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

    }
}
