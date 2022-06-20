﻿using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.Misc;
using TiledEngine.Classes.TileAddons;
using TiledEngine.Classes.TileAddons.FurnitureStuff;
using TiledSharp;
using static DataModels.Enums;
using static Globals.Classes.Settings;
using static TiledEngine.Classes.TileLoader;
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

        /// <summary>
        /// Default gid is blank tile
        /// </summary>
        public static void SwitchGid(Tile tile,Layers layer, int newGid = -1, bool wang = false)
        {
            tile.Unload();
            tile.Sprite = null;
            // tile = new Tile(newGid, MapDepths[(int)layer], tile.X, tile.Y);
            tile.GID = newGid + 1;
            AssignProperties(tile, layer, wang: wang);
        }
        internal static Rectangle GetTileSourceRectangle(int gid, TileSetPackage tileSetPackage, int tileSetDimension)
        {
            if (!tileSetPackage.IsForeground(gid))
                return TileRectangleHelper.GetBackgroundSourceRectangle(gid, tileSetDimension);
            else
                return TileRectangleHelper.GetNormalSourceRectangle(tileSetPackage.OffSetForegroundGID(gid), tileSetDimension);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempTile">Setting this to true will prevent the grid status from updating, good for ghost tile</param>
        /// <param name="wang">Do not do this on loads/creates, only for individual tiles</param>
        public static void AssignProperties(Tile tile, Layers layer, bool tempTile = false, bool wang = true)
        {
            
            TileSetPackage tileSetPackage = tile.TileSetPackage;

            if (tileSetPackage.OffSetBackgroundGID(tile.GID) == 3370)
                Console.WriteLine("test");
            if (wang)
            {
                int newGID = tileSetPackage.TilingSetManager.WangTile(tile);
                if(tile.GID != newGID)
                {
                    tile.GID = newGID + 1;

                }
                tileSetPackage.TilingSetManager.WangSorroundingTiles(tile);

            }

            TileManager tileManager = tile.TileManager;
            int tileSetDimension = tileSetPackage.GetDimension(tile.GID);
            Texture2D texture = tileSetPackage.GetTexture(tile.GID);
            tile.SourceRectangle = GetTileSourceRectangle(tile.GID, tileSetPackage, tileSetDimension);
            tile.DestinationRectangle = TileRectangleHelper.GetDestinationRectangle(tile);
            tile.Position = (Vector2Helper.GetVector2FromRectangle(tile.DestinationRectangle));

            //tile has some sort of property.
            if (tileSetPackage.ContainsKey(tile.GID))
            {

                string propertyString;

                TmxTilesetTile tileSetTile = tileSetPackage.GetTmxTileSetTile(tile.GID);
                propertyString = "portal";

                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {

                    PortalData portaldata = PortalData.PortalFromPropertyString(propertyString, tile.Position);
                    tileManager.Portals.Add(portaldata);
                }
                propertyString = "newSource";

                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    Rectangle propertySourceRectangle = TileObjectHelper.GetSourceRectangleFromTileProperty(propertyString);

                    tile.SourceRectangle = TileRectangleHelper.AdjustSourceRectangle(tile.SourceRectangle, propertySourceRectangle);
                    tile.DestinationRectangle = TileRectangleHelper.AdjustDestinationRectangle(tile.DestinationRectangle, propertySourceRectangle);
                    tile.Position = Vector2Helper.GetVector2FromRectangle(tile.DestinationRectangle);


                }



              

                if (tileSetTile.ObjectGroups.Count > 0)
                {


                    TileObjectHelper.AddObjectsFromObjectGroups(tile, layer, tileManager, tileSetTile, tempTile);


                }

                propertyString = "newHitBox";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    TileObjectHelper.AddObjectFromProperty(tile, layer, tileSetTile.Properties, tileManager, propertyString, tempTile);

                }

                propertyString = "lightSource";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    TileLightSourceHelper.AddJustLightSource(tile, tileManager, propertyString);
                }

                propertyString = "replace";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    tile.Addons.Add(new GrassTuft(tile, texture));

                }

                propertyString = "transparent";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    TestForTransparencyTile(tile, TileObjectHelper.GetSourceRectangleFromTileProperty(propertyString));
                }
                propertyString = "tilingKey";
                if (GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                {
                    tileSetPackage.TilingSetManager.AddNewSet(propertyString, tile.GID);
                }
                ////CREATE ANIMATION FRAMES
                TileAnimationHelper.CheckForAnimationFrames(tile, tileManager, tileSetPackage, propertyString);

            }


            //this should come after new source rectangles are calculated because we need the height of those to calculate layer depth!
            AssignTileLayer(tile, layer, tileManager.OffSetLayersDictionary);
            //Will be null if animation frames were not present
            if (tile.Sprite == null)
                tile.Sprite = SpriteFactory.CreateWorldSprite(tile.Position, tile.SourceRectangle, texture, customLayer: tile.Layer, randomizeLayers: false);
            else
            {
                tile.Sprite.CustomLayer = tile.Layer;
            }


            tile.Load();

        }


        /// <summary>
        ///Height must also be greater than 32, otherwise kinda pointless as we can already mostly see the player!
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tempObj"></param>
        private static void TestForTransparencyTile(Tile tile, Rectangle rectangle)
        {
            //if (tile.Layer >= .3f && tile.GID != -1 && tile.DestinationRectangle.Height > rectangle.Height && tile.DestinationRectangle.Height > 32)
            tile.Addons.Add(new TileTransparency(tile, tile.Position, new Rectangle((int)tile.Position.X + (int)rectangle.Width, (int)tile.Position.X + (int)rectangle.Height, rectangle.Width, rectangle.Height)));
        }
        /// <summary>
        /// If tile layer is in the forground we'll offset it according to its Y position. Else just give it the standard layerdepth.
        /// </summary>
        private static void AssignTileLayer(Tile tile, Layers layer, Dictionary<int, float> tileLayerOffsetDictionary)
        {
            if (layer == Layers.foreground)
            {
                if (tile.GID != -1)
                    tile.Layer = GetTileVariedLayerDepth(tile.Position, tile.SourceRectangle, tileLayerOffsetDictionary);
            }

            else
                tile.Layer = TileLoader.MapDepths[(int)layer];

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
