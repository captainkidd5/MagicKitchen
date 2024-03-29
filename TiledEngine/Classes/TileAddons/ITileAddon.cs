﻿using DataModels;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{
    public interface ITileAddon
    {

        protected TileObject Tile { get; }
        void Load();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);


        void SetToDefault();

        Action Interact(ref ActionType? actionType, bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing);
    }
}
