using Globals.Classes;
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
using static Globals.Classes.Settings;

namespace TiledEngine.Classes
{

    public class Tile
    {

        internal static bool TileIndexDebug = false;

        private int gid;

        //The icon the mouse should change to when hovered over this tile
        internal CursorIconType GetCursorIconType()
        {
            string property = GetProperty("IconType");
            if (property == null)
                return CursorIconType.None;
            //now need to check that no peripherary cursor icon types exist (ex. destructable)
            return Cursor.GetCursorIconTypeFromString(property);
        }
        internal TmxTilesetTile TmxTileSetTile => TileSetPackage.GetTmxTileSetTile(GID);

        internal readonly TileManager TileManager;
        internal TileSetPackage TileSetPackage => TileManager.TileSetPackage;

        public int GID { get { return gid - 1; } internal set { gid = value; } }

        //public TileType TileType { get; set; }
        internal float Layer { get; set; }

        internal Layers IndexLayer { get; set; }
        internal int X { get; set; }
        internal int Y { get; set; }
        internal Rectangle SourceRectangle { get; set; }
        internal Rectangle DestinationRectangle { get; set; }

        public Sprite Sprite { get; set; }


        internal List<ITileAddon> Addons { get; set; }
        public Vector2 Position { get; set; }

        public Vector2 CentralPosition => new Vector2(Position.X + DestinationRectangle.Width / 2,
            Position.Y + DestinationRectangle.Height / 2);

        public bool WithinRangeOfPlayer { get; internal set; }

        internal Tile(TileManager tileManager, int gid, Layers indexLayer, float layer, int x, int y)
        {

            GID = gid;
            Layer = layer;
            IndexLayer = indexLayer;
            X = x;
            Y = y;
            Addons = new List<ITileAddon>();
            TileManager = tileManager;

        }

        internal void AlertTileManagerCursorIconChanged()
        {
            TileManager.TileToInteractWith = this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="useObjectSearch">If true, will search tile objects properties</param>
        /// <returns></returns>
        internal string GetProperty(string key, bool useObjectSearch = false)
        {
            if (TmxTileSetTile == null)
                return null;
            if (TmxTileSetTile.Properties.ContainsKey(key))
                return TmxTileSetTile.Properties[key];

            if (useObjectSearch)
            {
                var objects = TmxTileSetTile.ObjectGroups[0].Objects;
                for (int k = 0; k < objects.Count; k++)
                {
                    if (objects[k].Properties.ContainsKey(key))
                        return objects[k].Properties[key];
                }
            }
            return null;

        }

        internal bool HasAnimationFrames => TmxTileSetTile != null && TmxTileSetTile.AnimationFrames.Count > 0;

        public void Update(GameTime gameTime, PathGrid pathGrid)
        {
            WithinRangeOfPlayer = false;
            Sprite.Update(gameTime, Position);

            for (int i = 0; i < Addons.Count; i++)
            {
                Addons[i].Update(gameTime);
            }

#if DEBUG
            if (Flags.DebugGrid)
            {
                if (pathGrid.Weight[X, Y] == (byte)GridStatus.Obstructed)
                {
                    Sprite.UpdateColor(Color.Red);
                }
            }
#endif

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
        /// returns unique tile key thru bitwise shifting.
        /// </summary>
        /// <returns>Tile Key</returns>
        internal int GetKey()
        {
            return ((X << 18) | (Y << 4) | ((int)IndexLayer << 0)); //14 bits for x and y, 4 bits for layer.
        }

        internal string GetTileKeyString(int layer, int x, int y)
        {
            return "" + X + "," + Y + "," + layer;
        }

        internal Rectangle GetTotalHitBoxRectangle()
        {
            Rectangle totalRectangle = Rectangle.Empty;
            foreach (ITileAddon addon in Addons)
            {
                if(addon.GetType() == typeof(TileBody))
                {
                    TileBody body = (TileBody)addon;
                    Rectangle rect = body.IntermediateTmxShape.GetBoundingRectangle();
                    return new Rectangle((int)Position.X, (int)Position.Y, rect.Width, rect.Height);
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
        public void Interact(bool isPlayer, Item heldItem)
        {
            foreach (ITileAddon addon in Addons)
                addon.Interact(isPlayer, heldItem);
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
