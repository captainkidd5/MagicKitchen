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
using TiledSharp;

namespace TiledEngine.Classes
{
    internal static class TileAnimationHelper
    {

        public static void SwitchGidToAnimationFrame(Tile tile)
        {
            Collection<TmxAnimationFrame> baseAnimationFrames = tile.TileSetPackage.GetTmxTileSetTile(tile.GID).AnimationFrames;

            if (baseAnimationFrames == null || baseAnimationFrames.Count < 1)
                throw new Exception($"Should not try to switch to a GID on a tile with no animation frames");

            int gidToSwapTo = -1;
            foreach (TmxAnimationFrame animationFrame in baseAnimationFrames)
            {
                //Sometimes animation frames contain original tile gid, ignore that case
                if (animationFrame.Id != tile.GID)
                {

                    int newGID = animationFrame.Id;
                    if (tile.TileSetPackage.IsForeground(tile.GID))
                        newGID = tile.TileSetPackage.OffSetBackgroundGID(newGID);

                    TmxTilesetTile tileSetTile = tile.TileSetPackage.GetTmxTileSetTile(newGID);

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
            TileUtility.SwitchGid(tile, tile.IndexLayer, gidToSwapTo + 1);
            return;

        }
        /// <summary>
        /// Checks for, and assigns an animated sprite in liu of the normal sprite. Will automatically adjust for new source rectangles 
        /// if they exist in the tilesheet. The only restriction is that the hitbox must stay constant throughout the animation frames
        /// as the frame only accounts for the image, not additional data
        /// </summary>
        public static void CheckForAnimationFrames(Tile tile, TileManager tileManager, TileSetPackage tileSetPackage, string propertyString)
        {
            TmxTilesetTile tmxTileSetTile = tileSetPackage.GetTmxTileSetTile(tile.GID);
            Collection<TmxAnimationFrame> animationFrames = tmxTileSetTile.AnimationFrames;

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
                        if (tileSetPackage.IsForeground(tile.GID))
                            frameRectangle = TileRectangleHelper.GetNormalSourceRectangle(animationFrames[i].Id, tileSetDimension);
                        else
                            frameRectangle = TileRectangleHelper.GetBackgroundSourceRectangle(animationFrames[i].Id, tileSetDimension);

                        //Animation frame ids are not global, need to offset them if we're using foreground sheet
                        int frameToCheckGID = animationFrames[i].Id;

                        if (tileSetPackage.IsForeground(tile.GID))
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
                if (tile.Layer > 1)
                    tile.Layer = tile.Layer * .1f;
                if(tile.GetCursorIconType() == InputEngine.Classes.CursorIconType.Break)
                {
                    tile.Sprite = SpriteFactory.CreateWorldAnimatedSprite(tile.Position, tile.SourceRectangle,
                  texture, frames, customLayer: tile.Layer, randomizeLayers: false);
                    return;
                }
                //if (tmxTileSetTile.ObjectGroups.Count > 0)
                //{
                //    for (int k = 0; k < tmxTileSetTile.ObjectGroups[0].Objects.Count; k++)
                //    {
                //        TmxObject tempObj = tmxTileSetTile.ObjectGroups[0].Objects[k];
                //        if (tempObj.Properties.ContainsKey("destructable"))
                //        {
                //            tile.Sprite = SpriteFactory.CreateWorldAnimatedSprite(tile.Position, tile.SourceRectangle,
                //   texture, frames, customLayer: tile.Layer, randomizeLayers: false);
                //            return;
                //        }



                //    }
                //}


                propertyString = "animate";
                if (TileUtility.GetTileProperty(tileSetPackage, tmxTileSetTile, ref propertyString))
                {

                    switch (propertyString)
                    {
                        case "pause":
                            tile.Sprite = SpriteFactory.CreateWorldAnimatedSprite(tile.Position, tile.SourceRectangle,
         texture, frames, customLayer: tile.Layer, randomizeLayers: false);
                            (tile.Sprite as AnimatedSprite).PingPong = true;


                            return;
                    }


                }



                tile.Sprite = SpriteFactory.CreateWorldIntervalAnimatedSprite(tile.Position, tile.SourceRectangle,
                texture, frames, customLayer: tile.Layer, randomizeLayers: false);


            }

        }
    }
}
