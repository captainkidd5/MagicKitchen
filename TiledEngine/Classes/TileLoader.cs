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
        public static Dictionary<int, TmxTilesetTile> MasterTileSetDictionary { get; private set; }
        public static Dictionary<int, TmxTilesetTile> InteriorTileSetDictionary { get; private set; }

        public static Texture2D MasterSpriteSheet { get; private set; }
        public static Texture2D InteriorSpriteSheet { get; private set; }

        private static PortalLoader _portalLoader;




        private static bool s_hasDoneInitialLoad;

        public static bool HasEdge(string stageFromName, string stageToName) => _portalLoader.HasEdge(stageFromName, stageToName);
        public static string GetNextNodeStageName(string stageFromName, string stageToName) => _portalLoader.GetNextNodeStageName(stageFromName, stageToName);

        public static Rectangle GetNextNodePortalRectangle(string stageFromName, string stageToName) => _portalLoader.GetNextPortalRectangle(stageFromName, stageToName);


        // <summary>
        /// This should only be called ONCE per save file.
        /// </summary>
        public static void LoadContent(ContentManager content)
        {
            MapPath = content.RootDirectory + "/Maps/";
            TmxMap worldMap = new TmxMap(MapPath + "LullabyTown.tmx");
            TmxMap interiorMap = new TmxMap(MapPath + "Restaurant.tmx");
            MasterTileSetDictionary = worldMap.Tilesets[0].Tiles;
            InteriorTileSetDictionary = interiorMap.Tilesets[0].Tiles;

            MasterSpriteSheet = content.Load<Texture2D>("maps/MasterSpriteSheet");
            InteriorSpriteSheet = content.Load<Texture2D>("maps/InteriorSpriteSheet1");
            _portalLoader = new PortalLoader();
        }

        /// <summary>
        /// Call after all stages have been loaded in at least once so that portal data is complete.
        /// </summary>
        public static void LoadFinished()
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
            tileManager.Load(ExtractTilesFromPreloadedMap(mapToLoad), mapToLoad.Width, GetTextureFromMapType(stageData.MapType));
        }

        public static Texture2D GetTextureFromMapType(MapType mapType)
        {
            if (mapType == MapType.Exterior)
                return MasterSpriteSheet;


            if (mapType == MapType.Interior)
                return InteriorSpriteSheet;

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
                    tilesToReturn[i][layerNameTile.X, layerNameTile.Y] = new Tile(layerNameTile.Gid, MapDepths[i], layerNameTile.X, layerNameTile.Y);

            }
            return tilesToReturn;
        }
    }
}
