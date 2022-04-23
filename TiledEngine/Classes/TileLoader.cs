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

        private static string MapPath;

        internal static TileSetPackage ExteriorTileSetPackage { get; private set; }

        internal static TileSetPackage InteriorTileSetPackage { get; private set; }

        private static Dictionary<int, TileLootData> s_foreGroundTileLootData;
        private static Dictionary<int, TileLootData> s_backGroundTileLootData;



        private static PortalLoader _portalLoader;



        private static bool s_hasDoneInitialLoad;

        public static bool HasEdge(string stageFromName, string stageToName) => _portalLoader.HasEdge(stageFromName, stageToName);
        public static string GetNextNodeStageName(string stageFromName, string stageToName) => _portalLoader.GetNextNodeStageName(stageFromName, stageToName);

        public static Rectangle GetNextNodePortalRectangle(string stageFromName, string stageToName) => _portalLoader.GetNextPortalRectangle(stageFromName, stageToName);

        public static ZoneManager ZoneManager;
        public static List<SpecialZone> GetZones(string stageName) => ZoneManager.GetZones(stageName);
        
        // <summary>
        /// This should only be called ONCE per save file.
        /// </summary>
        public static void LoadContent(ContentManager content)
        {
            MapPath = content.RootDirectory + "/Maps/";
            TmxMap worldMap = new TmxMap(MapPath + "LullabyTown.tmx");
            ExteriorTileSetPackage = new TileSetPackage(worldMap);
            ExteriorTileSetPackage.LoadContent(content, "maps/BackgroundMasterSpriteSheet_Spaced", "maps/ForegroundMasterSpriteSheet");


            TmxMap interiorMap = new TmxMap(MapPath + "Restaurant.tmx");
            InteriorTileSetPackage = new TileSetPackage(interiorMap);
            InteriorTileSetPackage.LoadContent(content, "maps/InteriorBackground_Spaced", "maps/InteriorForeground");

            _portalLoader = new PortalLoader();
            List<TileLootData> tileLootData = content.Load<List<TileLootData>>("Items/ForegroundTileLootData");
            s_foreGroundTileLootData = tileLootData.ToDictionary(x => x.TileId, x => x);

            tileLootData = content.Load<List<TileLootData>>("Items/BackgroundTileLootData");
            //Offset background GID here to make it easy to fetch correct loot for GID at runtime
            s_backGroundTileLootData = tileLootData.ToDictionary(x => ExteriorTileSetPackage.OffSetBackgroundGID(x.TileId), x => x);
            ZoneManager zoneManager = new ZoneManager();
        }
        internal static bool HasLootData(int tileId)
        {
            return s_foreGroundTileLootData.ContainsKey(tileId);
        }
        internal static TileLootData GetLootData(int tileId)
        {
            if (!HasLootData(tileId))
                throw new Exception($"No loot exists for tile with id {tileId}");
            return s_foreGroundTileLootData[tileId];
        }
        /// <summary>
        /// Call after all stages have been loaded in at least once so that portal data is complete.
        /// </summary>
        public static void FillFinalPortalGraph()
        {
            if (s_hasDoneInitialLoad)
                throw new Exception("May not load twice");
            _portalLoader.FillPortalGraph();
            s_hasDoneInitialLoad = true;
        }

        /// <summary>
        /// This should only be called ONCE per stage, per save file.
        /// </summary>
        /// <param name="stageData"></param>
        /// <param name="tileManager"></param>
        public static void LoadStagePortals(StageData stageData, TileManager tileManager)
        {
            TmxMap mapToLoad = new TmxMap(MapPath + stageData.Path);
            _portalLoader.AddPortals(tileManager.LoadPortals(mapToLoad));


        }


        /// <summary>
        /// This should only be called ONCE per stage, per save file.
        /// First load loads all the tiles in from the TMX maps, from then on
        /// all the tiles will be loaded with the binary reader
        /// </summary>
        /// <param name="stageData">Stage data is loaded in as a datamodel</param>
        /// <param name="tileManager"></param>
        public static void CreateNewSave(StageData stageData, TileManager tileManager, ContentManager content)
        {
            TmxMap mapToLoad = new TmxMap(MapPath + stageData.Path);
            tileManager.MapType = stageData.MapType;
            ZoneManager.Load(stageData.Name, mapToLoad, tileManager);

            tileManager.Load(ExtractTilesFromPreloadedMap(mapToLoad), mapToLoad.Width,
                GetPackageFromMapType(stageData.MapType));
        }

        public static void Save(BinaryWriter writer)
        {
            ZoneManager.Save(writer);
           
        }
        public static void LoadSave(BinaryReader reader)
        {
            ZoneManager.LoadSave(reader);
        }
        public static void Unload()
        {
            _portalLoader.Unload();
            ZoneManager.CleanUp();
            s_hasDoneInitialLoad = false;
        }
        internal static TileSetPackage GetPackageFromMapType(MapType mapType)
        {
            if (mapType == MapType.Exterior)
                return ExteriorTileSetPackage;


            if (mapType == MapType.Interior)
                return InteriorTileSetPackage;

            throw new Exception($"No sprite sheet associated with map type {mapType.ToString()}");
        }
        /// <summary>
        /// Create new tiles based on tiles found in TMX map file. This should
        /// only be done once per map per save.
        /// </summary>
        private static List<Tile[,]> ExtractTilesFromPreloadedMap(TmxMap map)
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
                {

                    tilesToReturn[i][layerNameTile.X, layerNameTile.Y] = new Tile(layerNameTile.Gid, (Layers)i, MapDepths[i], layerNameTile.X, layerNameTile.Y);


                }

            }
            return tilesToReturn;
        }

        
    }
}
