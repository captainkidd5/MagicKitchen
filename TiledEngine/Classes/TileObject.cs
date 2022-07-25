using DataModels;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledEngine.Classes.TileAddons;
using TiledSharp;
using UIEngine.Classes;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes
{

    public class TileObject
    {

        internal static bool TileIndexDebug = false;

        internal bool HasAnimationFrames => TileManager.TileSetPackage.GetTmxTileSetTile(TileData.GID).AnimationFrames.Count > 0;

        internal readonly TileManager TileManager;


        internal bool IsHighestClearTile()
        {
            for(int i = TileManager.TileData.Count - 1; i >= (byte)TileData.Layer; i--)
            {
                if (!TileManager.TileData[i][TileData.X, TileData.Y].Empty)
                    return false;
            }
            return true;
        }
        public bool FlaggedForRemoval { get; set; }


        public TileData TileData;

        internal Rectangle SourceRectangle { get; set; }
        internal Rectangle DestinationRectangle { get; set; }

        public Sprite Sprite { get; set; }

        public bool IsLoaded { get; set; }
        internal List<ITileAddon> Addons { get; set; }
        public Vector2 Position { get; set; }

        public float DrawLayer { get; set; }
        public Vector2 CentralPosition => new Vector2(Position.X + DestinationRectangle.Width / 2,
            Position.Y + DestinationRectangle.Height / 2);
        public bool WithinRangeOfPlayer { get; internal set; }
        internal string GetProperty(string key, bool useObjectSearch = false)
        {
            return TileData.GetProperty(TileManager.TileSetPackage, key, useObjectSearch);
        }
        //The icon the mouse should change to when hovered over this tile
        internal CursorIconType GetCursorIconType()
        {
            return TileData.GetCursorIconType(TileManager.TileSetPackage);
        }
        internal TileObject(TileManager tileManager, TileData tileData, bool wang = false, bool tempTile = false)
        {

            TileData = tileData;


            Addons = new List<ITileAddon>();
            TileManager = tileManager;
            TileUtility.AssignProperties(tileManager, this, tileData);

        }

        internal void AlertTileManagerCursorIconChanged()
        {
            
            TileManager.TileToInteractWith = this;
        }



        public void Update(GameTime gameTime, PathGrid pathGrid)
        {
            if (!TileManager.IsWithinUpdateRange(TileData))
                FlaggedForRemoval = true;
            WithinRangeOfPlayer = false;
            Sprite.Update(gameTime, Position);

            for (int i = 0; i < Addons.Count; i++)
            {
                Addons[i].Update(gameTime);
            }

            

        }

        internal void DrawLights(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Addons.Count; i++)
            {
                ITileAddon addon = Addons[i];
                if (addon.GetType() == typeof(LightBody))
                    (addon as Collidable).DrawLights(spriteBatch);
            }
        }
        internal void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            Sprite.Draw(spriteBatch);
            for (int i = 0; i < Addons.Count; i++)
            {
                Addons[i].Draw(spriteBatch);
            }



            //if (TileIndexDebug)
            //spriteBatch.DrawString(Game1.MainFont, X + "," + Y, Position, Color.Orange, 0f, Vector2.Zero, .35f, SpriteEffects.None, .99f);
        }



        /// <summary>
        /// TODO: Combine rectangles into larger rectangle if multiple bodies do not overlap
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal Rectangle GetTotalHitBoxRectangle(Vector2? position = null)
        {
            Rectangle totalRectangle = Rectangle.Empty;
            foreach (ITileAddon addon in Addons)
            {
                if (addon.GetType() == typeof(TileBody))
                {
                    TileBody body = (TileBody)addon;
                    Rectangle rect = body.IntermediateTmxShape.GetBoundingRectangle();
                    //if(rect.Width <= Settings.TileSize)
                    //    return RectangleHelper.RectFromPosition(position.Value, rect.Width, rect.Height);

                    return RectangleHelper.RectFromPosition(position ?? Position, rect.Width, rect.Height);
                }
            }
            return Rectangle.Empty;
        }
        /// <summary>
        /// Call at the end of assign properties.
        /// </summary>
        internal void Load()
        {
            foreach (ITileAddon addon in Addons)
                addon.Load();
        }
        public ActionType? Interact(bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)
        {
           ActionType? actionType = null;
            foreach (ITileAddon addon in Addons)
            {
                ActionType? aType = addon.Interact(isPlayer, heldItem, entityPosition, directionEntityFacing);
                    if(aType != null)
                    actionType = aType;

            }
            return actionType;
        }

        public void Unload()
        {
            foreach (ITileAddon addon in Addons)
            {
                addon.CleanUp();
            }
            Addons.Clear();
            Sprite = null;

        }

        public List<ITileAddon> GetAddonsByType(Type t)
        {
            return Addons.Where(x => x.GetType() == t).ToList();
        }




    }
}