using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons;
using TiledSharp;

namespace TiledEngine.Classes
{
    internal static class TileAnimationHelper
    {

        public static void SwitchGidToAnimationFrame(TileManager tileManager, TileObject tile)
        {
            Collection<TmxAnimationFrame> baseAnimationFrames = tileManager.TileSetPackage.GetTmxTileSetTile(tile.TileData.GID).AnimationFrames;

            if (baseAnimationFrames == null || baseAnimationFrames.Count < 1)
                throw new Exception($"Should not try to switch to a GID on a tile with no animation frames");

            int gidToSwapTo = -1;
            foreach (TmxAnimationFrame animationFrame in baseAnimationFrames)
            {
                //Sometimes animation frames contain original tile gid, ignore that case
                if (animationFrame.Id != tile.TileData.GID)
                {

                    int newGID = animationFrame.Id;
                    if (tileManager.TileSetPackage.IsForeground(tile.TileData.GID))
                        newGID = tileManager.TileSetPackage.OffSetBackgroundGID(newGID);

                    TmxTilesetTile tileSetTile = tileManager.TileSetPackage.GetTmxTileSetTile(newGID);

                    if (tileSetTile.AnimationFrames != null && tileSetTile.AnimationFrames.Count > 0)
                    {
                        gidToSwapTo = newGID;
                        break;
                        //Found animated gid within animation frames, swap to it and end

                    }
                }

            }
            if (gidToSwapTo < 0)
                return;
            tileManager.SwitchGID((ushort)gidToSwapTo, tile.TileData);
            return;

        }
        /// <summary>
        /// Checks for, and assigns an animated sprite in liu of the normal sprite. Will automatically adjust for new source rectangles 
        /// if they exist in the tilesheet. The only restriction is that the hitbox must stay constant throughout the animation frames
        /// as the frame only accounts for the image, not additional data
        /// </summary>
        public static void CheckForAnimationFrames(TileObject tileObject, TileManager tileManager, TileSetPackage tileSetPackage, string propertyString)
        {
            TmxTilesetTile tmxTileSetTile = tileSetPackage.GetTmxTileSetTile(tileObject.TileData.GID);
            Collection<TmxAnimationFrame> animationFrames = tmxTileSetTile.AnimationFrames;

            int tileSetDimension = tileSetPackage.GetDimension(tileObject.TileData.GID);
            Texture2D texture = tileSetPackage.GetTexture(tileObject.TileData.GID);
            if (animationFrames.Count > 0)
            {
                AnimationFrame[] frames = new AnimationFrame[animationFrames.Count];

                for (int i = 0; i < animationFrames.Count; i++)
                {
                    Rectangle frameRectangle = tileObject.SourceRectangle;
                    //First animation frame will already have expanded source rectangle
                    if (i > 0)
                    {

                        propertyString = "newSource";
                        if (tileSetPackage.IsForeground(tileObject.TileData.GID))
                            frameRectangle = TileRectangleHelper.GetNormalSourceRectangle(animationFrames[i].Id, tileSetDimension);
                        else
                            frameRectangle = TileRectangleHelper.GetBackgroundSourceRectangle(animationFrames[i].Id, tileSetDimension);

                        //Animation frame ids are not global, need to offset them if we're using foreground sheet
                        int frameToCheckGID = animationFrames[i].Id;

                        if (tileSetPackage.IsForeground(tileObject.TileData.GID))
                            frameToCheckGID = tileSetPackage.OffSetBackgroundGID(frameToCheckGID);

                        TmxTilesetTile tileSetTile = tileSetPackage.GetTmxTileSetTile(frameToCheckGID);
                        if (tileSetTile != null)
                        {
                            if (TileUtility.GetTileProperty(tileSetPackage, tileSetTile, ref propertyString))
                            {
                                frameRectangle = TileRectangleHelper.AdjustSourceRectangle(frameRectangle, TileObjectHelper.GetSourceRectangleFromTileProperty(propertyString));
                            }

                        }
                    }

                    frames[i] = new AnimationFrame(i, frameRectangle,
                    animationFrames[i].Duration * .001f);
                }
                if (tileObject.TileData.Layer> 1)
                    tileObject.DrawLayer = tileObject.TileData.Layer * .1f;

        
                if(tileObject.Addons.Any(x => x.GetType() == typeof(DestructableTile)))
                {
                    tileObject.Sprite = SpriteFactory.CreateWorldAnimatedSprite(tileObject.Position, tileObject.SourceRectangle,
                  texture, frames, customLayer: tileObject.TileData.Layer, randomizeLayers: false);
                    (tileObject.Sprite as AnimatedSprite).Paused = true;
                    return;
                }

                propertyString = "animate";
                if (TileUtility.GetTileProperty(tileSetPackage, tmxTileSetTile, ref propertyString))
                {

                    switch (propertyString)
                    {
                        case "pause":
                            tileObject.Sprite = SpriteFactory.CreateWorldAnimatedSprite(tileObject.Position, tileObject.SourceRectangle,
         texture, frames, customLayer: tileObject.TileData.Layer, randomizeLayers: false);
                            (tileObject.Sprite as AnimatedSprite).PingPong = true;


                            return;
                    }


                }



                tileObject.Sprite = SpriteFactory.CreateWorldIntervalAnimatedSprite(tileObject.Position, tileObject.SourceRectangle,
                texture, frames, customLayer: tileObject.TileData.Layer, randomizeLayers: false);


            }

        }
    }
}
