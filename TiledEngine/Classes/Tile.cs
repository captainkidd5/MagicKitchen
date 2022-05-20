using Globals.Classes;
using InputEngine.Classes;
using InputEngine.Classes.Input;
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
using UIEngine.Classes;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes
{

    public enum TileType
    {
        None = 0,
        Basic = 1

    }
    public class Tile
    {

        internal static bool TileIndexDebug = false;

        private int gid;

        //The icon the mouse should change to when hovered over this tile
        internal CursorIconType CursorIconType;
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

        internal Tile(int gid, Layers indexLayer, float layer, int x, int y)
        {
            
            GID = gid;
            Layer = layer;
            IndexLayer = indexLayer;
            X = x;
            Y = y;
            Addons = new List<ITileAddon>();
            CursorIconType = CursorIconType.None;

        }

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


        /// <summary>
        /// Call at the end of assign properties.
        /// </summary>
        internal void Load()
        {
            foreach (ITileAddon addon in Addons)  
                addon.Load();               
        }
        public void Interact(bool isPlayer)
        {
            foreach (ITileAddon addon in Addons)
                addon.Interact(isPlayer);
        }

        public void Unload()
        {
            foreach(ITileAddon addon in Addons)
            {
                addon.CleanUp();
            }
            Addons.Clear();
            Sprite = null;
            CursorIconType = CursorIconType.None;
            
        }

        public List<ITileAddon> GetAddonsByType(Type t)
        {
            return Addons.Where(x => x.GetType() == t).ToList();
        }

        


    }
}
