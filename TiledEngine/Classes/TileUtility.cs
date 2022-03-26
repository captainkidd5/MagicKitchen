using Globals.Classes;
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
using TiledSharp;
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

        private static bool GetProperty(TileSetPackage tileSetPackage, int tileGID, ref string property)
        {
            return tileSetPackage.GetProperty(tileGID).Properties.TryGetValue(property, out property);
        }

        /// <summary>
        /// Default gid is blank tile
        /// </summary>
        public static void SwitchGid(Tile tile, TileManager tileManager, Layers layer,int newGid = 0)
        {
            tile.Unload();
            tile.Sprite = null;
           // tile = new Tile(newGid, MapDepths[(int)layer], tile.X, tile.Y);
            tile.GID = newGid;
            AssignProperties(tile, layer, tileManager);
        }

        public static void AssignProperties(Tile tile, Layers layer,  TileManager tileManager)
        {

            tile.TileType = TileType.Basic;
            TileSetPackage tileSetPackage = tileManager.TileSetPackage;
            int tileSetDimension = tileSetPackage.GetDimension(tile.GID);
            Texture2D texture = tileSetPackage.GetTexture(tile.GID);
            tile.SourceRectangle = TileRectangleHelper.GetTileSourceRectangle(tile.GID, tileSetDimension);
            tile.DestinationRectangle = TileRectangleHelper.GetDestinationRectangle(tile);
            tile.Position = (Vector2Helper.GetVector2FromRectangle(tile.DestinationRectangle));

            //tile has some sort of property.
            if (tileSetPackage.ContainsKey(tile.GID))
            {

                string propertyString;

                TmxTilesetTile tileSetTile = tileSetPackage.GetProperty(tile.GID);
                propertyString = "portal";

                if (GetProperty(tileSetPackage, tile.GID, ref propertyString))
                {

                    PortalData portaldata = PortalData.PortalFromPropertyString(propertyString, tile.Position);
                    tileManager.Portals.Add(portaldata);
                }
                propertyString = "newSource";

                if (GetProperty(tileSetPackage, tile.GID, ref propertyString))
                {
                    Rectangle propertySourceRectangle = TileRectangleHelper.GetSourceRectangleFromTileProperty(propertyString);

                    tile.SourceRectangle = TileRectangleHelper.AdjustSourceRectangle(TileRectangleHelper.GetLargeSourceRectangle(tileSetPackage.OffSetForegroundGID(tile.GID), tileSetDimension), propertySourceRectangle);
                    tile.DestinationRectangle = TileRectangleHelper.AdjustDestinationRectangle(tile, propertySourceRectangle);
                    tile.Position = (Vector2Helper.GetVector2FromRectangle(tile.DestinationRectangle));
                }

               

                ////CREATE ANIMATION FRAMES
                CheckForAnimationFrames(tile, tileManager, tileSetPackage, propertyString);

                if (tileSetTile.ObjectGroups.Count > 0)
                {
                   

                        TileObjectHelper.AddObjectsFromObjectGroups(tile, layer, tileManager, tileSetTile);

                    
                }

                propertyString = "newHitBox";
                if (GetProperty(tileSetPackage, tile.GID, ref propertyString))
                {
                    TileObjectHelper.AddObjectFromProperty(tile, tileManager, TileRectangleHelper.GetSourceRectangleFromTileProperty(propertyString));
                }

                propertyString = "lightSource";
                if (GetProperty(tileSetPackage, tile.GID, ref propertyString))
                {
                    TileLightSourceHelper.AddJustLightSource(tile,tileManager, propertyString, 3f);
                }

                propertyString = "replace";
                if (GetProperty(tileSetPackage, tile.GID, ref propertyString))
                {
                    tile.Addons.Add(new GrassTuft(tile, texture));

                }
                

            }

           
            //this should come after new source rectangles are calculated because we need the height of those to calculate layer depth!
            AssignTileLayer(tile, layer, tileManager.OffSetLayersDictionary);
            //Will be null if animation frames were not present
            if (tile.Sprite == null)
                tile.Sprite = SpriteFactory.CreateWorldSprite(tile.Position, tile.SourceRectangle, texture, customLayer: tile.Layer, randomizeLayers: false);


            tile.Load();

        }

        /// <summary>
        /// Checks for, and assigns an animated sprite in liu of the normal sprite. Will automatically adjust for new source rectangles 
        /// if they exist in the tilesheet. The only restriction is that the hitbox must stay constant throughout the animation frames
        /// as the frame only accounts for the image, not additional data
        /// </summary>
        private static void CheckForAnimationFrames(Tile tile, TileManager tileManager, TileSetPackage tileSetPackage, string propertyString)
        {

            Collection<TmxAnimationFrame> animationFrames = tileSetPackage.GetProperty(tile.GID).AnimationFrames;

            int tileSetDimension = tileSetPackage.GetDimension(tile.GID);
            Texture2D texture = tileSetPackage.GetTexture(tile.GID);
            if (animationFrames.Count > 0)
            {
                AnimationFrame[] frames = new AnimationFrame[animationFrames.Count];

                for (int i = 0; i < animationFrames.Count; i++)
                {
                    Rectangle frameRectangle = tile.SourceRectangle;
                    //First animation frame will already have expanded source rectangle
                    if (i > 0)
                    {
                        propertyString = "newSource";
                        frameRectangle = TileRectangleHelper.GetTileSourceRectangle(animationFrames[i].Id, tileSetDimension);
                        if (tileSetPackage.ContainsKey(animationFrames[i].Id))
                        {
                            if (GetProperty(tileSetPackage, animationFrames[i].Id, ref propertyString))
                            {
                                frameRectangle = TileRectangleHelper.AdjustSourceRectangle(frameRectangle, TileRectangleHelper.GetSourceRectangleFromTileProperty(propertyString));
                            }

                        }
                    }

                    frames[i] = new AnimationFrame(i, frameRectangle,
                    animationFrames[i].Duration * .001f);
                }
                if (tile.Layer > 1)
                    tile.Layer = tile.Layer * .1f;
                tile.Sprite = SpriteFactory.CreateWorldAnimatedSprite(tile.Position, tile.SourceRectangle,
                    texture, frames, customLayer: tile.Layer, randomizeLayers: false);
            }

        }

        /// <summary>
        /// If tile layer is in the forground we'll offset it according to its Y position. Else just give it the standard layerdepth.
        /// </summary>
        private static void AssignTileLayer(Tile tile, Layers layer, Dictionary<int, float> tileLayerOffsetDictionary)
        {
            if (layer == Layers.foreground || layer == Layers.front)
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
