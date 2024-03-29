﻿using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using TiledEngine.Classes.TileAnimationStuff;
using TiledEngine.Classes.TilePlacementStuff.TilingStuff;
using TiledSharp;
using static DataModels.Enums;
using static Globals.Classes.Settings;
using static TiledEngine.Classes.MapLoader;
using static TiledEngine.Classes.TileManager;

namespace TiledEngine.Classes
{
    internal static class TileUtility
    {
        /// <summary>
        /// Retrieve whether or not the tile contains the specified property.
        /// </summary>
        /// <param name="property">Property value will be stored in this string reference.</param>

        public static bool GetTileProperty(TileSetPackage tileSetPackage, TmxTilesetTile tile, ref string property)
        {
            return tile.Properties.TryGetValue(property, out property);
        }


        internal static Rectangle GetTileSourceRectangle(int gid, TileSetPackage tileSetPackage, int tileSetDimension)
        {
            if (!tileSetPackage.IsForeground(gid))
                return TileRectangleHelper.GetBackgroundSourceRectangle(gid, tileSetDimension);
            else
                return TileRectangleHelper.GetNormalSourceRectangle(tileSetPackage.OffSetForegroundGID(gid), tileSetDimension);

        }

        public static void Pass1PreliminaryData(TileSetPackage tileSetPackage, TmxTilesetTile tileSetTile, bool isForeground = false)
        {
         
           
            

                string propertyString = "tilingKey";
            if (tileSetTile.Properties.ContainsKey(propertyString))
            {
                
                tileSetPackage.WangManager.AddTilingKey(tileSetTile.Properties[propertyString],isForeground ? tileSetPackage.OffSetBackgroundGID(tileSetTile.Id) : tileSetTile.Id);

            }
     
            
        }
        /// <summary>
        /// Second pass goes through and checks for alternative tiles which can be treated as part of the normal tiling set
        /// for example, variations on dirt tiles
        /// </summary>

        public static void Pass2PreliminaryData(TileSetPackage tileSetPackage, TmxTilesetTile tileSetTile, bool isForeground = false)
        {
            //0: set name
            //1: Index of dictionary which contains tile (0 would be the primary tile)
            //2: Weight:20 gives a weight of 20



            string propertyString = "alternateKey";
            if (tileSetTile.Properties.ContainsKey(propertyString))
            {
                string val = tileSetTile.Properties[propertyString];
                string[] arr = val.Split(",");
                string setName = arr[0];
                int tilingValue = int.Parse(arr[1]);

                int weight = int.Parse(arr[2].Split(":")[1]);

                tileSetPackage.WangManager.WangSets[setName].Set[tilingValue].Add(new WangTile((byte)weight, tileSetTile.Id));


            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempTile">Setting this to true will prevent the grid status from updating, good for ghost tile</param>
        /// <param name="wang">Do not do this on loads/creates, only for individual tiles</param>
        public static void AssignProperties(TileManager tileManager, TileObject tileObject, TileData tileData, bool tempTile = false)
        {
            TileSetPackage tileSetPackage = tileManager.TileSetPackage;


            if (!string.IsNullOrEmpty(tileData.GetProperty(tileSetPackage,"tilingSet")))
            {
                List<int> ids = tileSetPackage.WangManager.WangTile(tileManager, tileData);
                if (!ids.Contains( tileData.GID))
                {
                    //if(tileManager.TileSetPackage.IsForeground(tileData.GID))
                    //    newGID = tileManager.TileSetPackage.OffSetBackgroundGID(newGID);
                    tileData.GID = (ushort)(ids[0] + 1);
                    tileObject.TileData.GID = (ushort)(tileData.GID + 1); ;
                }

            }
            tileSetPackage.WangManager.WangSorroundingTiles(tileManager, tileData);

            int tileSetDimension = tileSetPackage.GetDimension(tileData.GID);
            Texture2D texture = tileSetPackage.GetTexture(tileData.GID);


            tileObject.SourceRectangle = GetTileSourceRectangle(tileData.GID, tileSetPackage, tileSetDimension);
            tileObject.DestinationRectangle = TileRectangleHelper.GetDestinationRectangle(tileData);
            tileObject.Position = (Vector2Helper.GetVector2FromRectangle(tileObject.DestinationRectangle));

            //tile has some sort of property.
            if (tileSetPackage.ContainsKey(tileData.GID))
            {

                string propertyString;

                TmxTilesetTile tileSetTile = tileSetPackage.GetTmxTileSetTile(tileData.GID);

                propertyString = "newSource";

                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    Rectangle propertySourceRectangle = TileObjectHelper.GetSourceRectangleFromTileProperty(propertyString);

                    tileObject.SourceRectangle = TileRectangleHelper.AdjustSourceRectangle(tileObject.SourceRectangle, propertySourceRectangle);
                    tileObject.DestinationRectangle = TileRectangleHelper.AdjustDestinationRectangle(tileObject.DestinationRectangle, propertySourceRectangle);
                    tileObject.Position = Vector2Helper.GetVector2FromRectangle(tileObject.DestinationRectangle);


                }





                if (tileSetTile.ObjectGroups.Count > 0)
                {


                    TileObjectHelper.AddObjectsFromObjectGroups(tileObject, (Layers)tileData.Layer, tileSetTile, tempTile);


                }

                propertyString = "newHitBox";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    TileObjectHelper.AddObjectFromProperty(tileObject, (Layers)tileData.Layer, tileSetTile.Properties, tileManager, propertyString, tempTile);

                }

                propertyString = "lightSource";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    TileLightSourceHelper.AddJustLightSource(tileObject, tileManager, propertyString);
                }

                propertyString = "replace";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    tileObject.Addons.Add(new GrassTuft(tileObject, texture));

                }

                propertyString = "transparent";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    TestForTransparencyTile(tileObject, TileObjectHelper.GetSourceRectangleFromTileProperty(propertyString));
                }
  
                ////CREATE ANIMATION FRAMES
                TileAnimationHelper.CheckForAnimationFrames(tileObject, tileManager, tileSetPackage, propertyString);

            }

            tileObject.DrawLayer = AssignTileLayer(tileData, tileObject, (Layers)tileData.Layer,
                    tileManager.OffSetLayersDictionary);

            //Will be null if animation frames were not present
            if (tileObject.Sprite == null)
                tileObject.Sprite = SpriteFactory.CreateWorldSprite(tileObject.Position, tileObject.SourceRectangle, texture, customLayer: tileObject.DrawLayer, randomizeLayers: false);

            //this should come after new source rectangles are calculated because we need the height of those to calculate layer depth!

            if(!tempTile)
            tileObject.Load();

        }


        /// <summary>
        ///Height must also be greater than 32, otherwise kinda pointless as we can already mostly see the player!
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tempObj"></param>
        private static void TestForTransparencyTile(TileObject tile, Rectangle rectangle)
        {
            //if (tile.Layer >= .3f && tile.GID != -1 && tile.DestinationRectangle.Height > rectangle.Height && tile.DestinationRectangle.Height > 32)
            tile.Addons.Add(new TileTransparency(tile, tile.Position, new Rectangle((int)tile.Position.X + (int)rectangle.Width, (int)tile.Position.X + (int)rectangle.Height, rectangle.Width, rectangle.Height)));
        }
        /// <summary>
        /// If tile layer is in the forground we'll offset it according to its Y position. Else just give it the standard layerdepth.
        /// </summary>
        internal static float AssignTileLayer(TileData tileData, TileObject tile, Layers layer, Dictionary<int, float> tileLayerOffsetDictionary)
        {
            if (layer == Layers.foreground)
            {
                if (tileData.GID != 0)
                    return GetTileVariedLayerDepth(tile.Position, tile.SourceRectangle, tileLayerOffsetDictionary);
            }
            return MapLoader.MapDepths[(int)layer];

        }

        /// <summary>
        /// Returns a float value which is at least slightly larger than the given layerDepth, and at most .099 greater than the value.
        /// It will make sure that the value is not already contained within the dictionary.
        /// </summary>
        /// <param name="dictionary">Dictionary to search through.</param>
        private static float GetTileVariedLayerDepth(Vector2 position, Rectangle sourceRectangle, Dictionary<int, float> dictionary)
        {
            float randomOffset = Settings.Random.Next(1, 999) * SpriteUtility.LayerMultiplier * .001f;
            float yAxisLayerDepth = SpriteUtility.GetYAxisLayerDepth(position, sourceRectangle);
            float variedLayerDepth = yAxisLayerDepth + randomOffset;
            while (dictionary.ContainsValue(variedLayerDepth))
            {
                randomOffset = Settings.Random.Next(1, 999) * SpriteUtility.LayerMultiplier;
                variedLayerDepth = yAxisLayerDepth + randomOffset;
            }
            return variedLayerDepth;

        }


    }
}