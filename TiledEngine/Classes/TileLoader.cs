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
using TiledEngine.Classes.Misc;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.IO;
using static Globals.Classes.Settings;
using System.Linq;
using TiledEngine.Classes.ZoneStuff;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using static DataModels.Enums;

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

        private static PortalLoader _portalLoader;

        internal static TilesetPackageManager TilesetPackageManager;
        internal static TileLootManager TileLootManager;



        public static bool HasEdge(string stageFromName, string stageToName) => _portalLoader.HasEdge(stageFromName, stageToName);
        public static string GetNextNodeStageName(string stageFromName, string stageToName) => _portalLoader.GetNextNodeStageName(stageFromName, stageToName);

        public static Rectangle GetNextNodePortalRectangle(string stageFromName, string stageToName) => _portalLoader.GetNextPortalRectangle(stageFromName, stageToName);
        internal static TileSetPackage GetPackageFromMapType(MapType mapType) => TilesetPackageManager.GetPackageFromMapType(mapType);

        public static ZoneManager ZoneManager;
        public static List<SpecialZone> GetZones(string stageName) => ZoneManager.GetZones(stageName);

        public static FurnitureLoader FurnitureLoader;
        
        // <summary>
        /// This should only be called ONCE per save file.
        /// </summary>
        public static void LoadContent(ContentManager content)
        {
            s_mapPath = content.RootDirectory + "/Maps/";
            TmxMap worldMap = new TmxMap(s_mapPath + "LullabyTown.tmx");
            TilesetPackageManager = new TilesetPackageManager();


            TmxMap interiorMap = new TmxMap(s_mapPath + "Restaurant.tmx");

            TilesetPackageManager.LoadContent(content, worldMap, interiorMap);

            _portalLoader = new PortalLoader();

            TileLootManager = new TileLootManager();
            TileLootManager.LoadContent(content, TilesetPackageManager.ExteriorTileSetPackage);
            ZoneManager = new ZoneManager();

            FurnitureLoader = new FurnitureLoader();
            FurnitureLoader.LoadContent(content);
        }
       
        /// <summary>
        /// Call after all stages have been loaded in at least once so that portal data is complete.
        /// </summary>
        public static void FillFinalPortalGraph() => _portalLoader.FillPortalGraph();


        /// <summary>
        /// This should only be called ONCE per stage, per save file.
        /// First load loads all the tiles in from the TMX maps, from then on
        /// all the tiles will be loaded with the binary reader
        /// </summary>
        public static void CreateNewSave(StageData stageData, TileManager tileManager, ContentManager content)
        {
            TmxMap mapToLoad = new TmxMap(s_mapPath + stageData.Path);
            tileManager.MapType = stageData.MapType;
            ZoneManager.CreateNewSave(stageData.Name, mapToLoad, tileManager);

            tileManager.CreateNewSave(tileManager,ExtractTilesFromPreloadedMap(tileManager,mapToLoad), mapToLoad.Width,
                GetPackageFromMapType(stageData.MapType));

            _portalLoader.AddPortals(tileManager.LoadPortals(mapToLoad));

        }

        public static void Save(BinaryWriter writer)
        {
            ZoneManager.Save(writer);
            _portalLoader.Save(writer);
           
        }
        public static void LoadSave(BinaryReader reader)
        {
            ZoneManager.LoadSave(reader);
            _portalLoader.LoadSave(reader); 
        }
        public static void Unload()
        {
            ZoneManager.CleanUp();
            _portalLoader.CleanUp();

        }

        /// <summary>
        /// Create new tiles based on tiles found in TMX map file. This should
        /// only be done once per map per save.
        /// </summary>
        private static List<Tile[,]> ExtractTilesFromPreloadedMap(TileManager tileManager, TmxMap map)
        {
            List<TmxLayer> allLayers = new List<TmxLayer>()
            {
                map.TileLayers["background"],
            map.TileLayers["midground"],
           map.TileLayers["buildings"],
           map.TileLayers["foreground"],
           map.TileLayers["front"]
        };
            List<Tile[,]> tilesToReturn = new List<Tile[,]>();

            for (int i = 0; i < MapDepths.Length; i++)
            {
                tilesToReturn.Add(new Tile[map.Width, map.Width]);
                foreach (TmxLayerTile layerNameTile in allLayers[i].Tiles)
                    tilesToReturn[i][layerNameTile.X, layerNameTile.Y] = new Tile(tileManager,layerNameTile.Gid, (Layers)i, MapDepths[i], layerNameTile.X, layerNameTile.Y);
            }
            return tilesToReturn;
        }
        
    }
}
