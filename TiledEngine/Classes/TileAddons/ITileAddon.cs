using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{
    public interface ITileAddon
    {

        protected Tile Tile { get; }
        void Load();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);


        void CleanUp();

        void Interact(bool isPlayer, Item heldItem);
    }
}
