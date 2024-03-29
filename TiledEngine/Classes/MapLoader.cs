﻿using DataModels;
using Globals.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TiledSharp;

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
using PhysicsEngine.Classes;
using TiledEngine.Classes.PortalStuff;
using MonoGame.Extended.Timers;

namespace TiledEngine.Classes
{
    public enum WorldSize
    {
        None = 0,
        Small = 512,
        Medium = 1024,
        Large = 2048,
        Huge = 4096

    }
    public static class MapLoader
    {
        public static float[] MapDepths = new float[5]
            {
                .01f,
                .1f,
                .2f,
                .3f,
                .5f,
            };

        private static WorldSize s_currentWorldSize;
        private static string s_mapPath;


        internal static TileSetPackage TileSetPackage;
        internal static TileLootManager TileLootManager;



        private static ProceduralPlacer s_proceduralPlacer;

        public static FurnitureLoader FurnitureLoader;
        public static PortalManager Portalmanager { get; set; }

        static public ZoneManager ZoneManager;
        public static bool HasEdge(string stageFromName, string stageToName) => Portalmanager.HasEdge(stageFromName, stageToName);
        public static string GetNextNodeStageName(string stageFromName, string stageToName) => Portalmanager.GetNextNodeStageName(stageFromName, stageToName);
        public static Rectangle GetNextPortalRectangle(string stageFrom, string stageTo) =>
            Portalmanager.GetNextPortalRectangle(stageFrom, stageTo);


    
        public static void Initialize(ContentManager content)
        {
            s_currentWorldSize = WorldSize.Small;
            s_proceduralPlacer = new ProceduralPlacer();
            s_proceduralPlacer.Load(content);

            s_mapPath = content.RootDirectory + "/Maps/";
            TmxMap worldMap = new TmxMap(s_mapPath + "LullabyTown.tmx");
            TileSetPackage = new TileSetPackage(worldMap);

            TileSetPackage.LoadContent(content, "Maps/BackgroundMasterSpriteSheet_Spaced", "Maps/Foreground2");

            TileLootManager = new TileLootManager(TileSetPackage);
            TileLootManager.LoadContent(content, TileSetPackage);

            FurnitureLoader = new FurnitureLoader();
            FurnitureLoader.LoadContent(content);
            if(Portalmanager != null)
            {
                Portalmanager.SetToDefault();
            }
            Portalmanager = new PortalManager();
            ZoneManager = new ZoneManager();

  

        }


     
        public static TileManager CreateTileManagerFromTmxMap(GraphicsDevice graphics, StageData stageData, ContentManager content)
        {
            TmxMap map = new TmxMap(s_mapPath + stageData.Path);

            TileManager tileManager = new TileManager(graphics, content);
            tileManager.Initialize(Camera);
            List<TileData[,]> mapData = ExtractTilesFromPreloadedMap(tileManager, map);


            stageData.Load(map.Width);
            ZoneManager.LoadZones(map, stageData.Name);
            Portalmanager.LoadPortalZones(map, stageData.InsertionX, stageData.InsertionY);
            //_portalLoader.AssimilatePortalObjectLayer(map);




            tileManager.LoadMap(map, mapData, map.Width, TileSetPackage);


            Portalmanager.CreateNewSave(tileManager.TileData, tileManager.TileSetPackage);


            s_proceduralPlacer.AddClusterTiles(tileManager);

            return tileManager;
        }

        public static void FillPortalGraph()
        {
            Portalmanager.FillPortalGraph();

        }

        public static void CreatePortalObjects() => Portalmanager.CreatePortalObjects();

        /// <summary>
        /// Generates the first iteration of the background layer of the entire map
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <returns></returns>

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



    }
}
