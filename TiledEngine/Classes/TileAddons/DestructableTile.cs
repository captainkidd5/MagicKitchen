using DataModels.ItemStuff;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes.Helpers;
using TiledEngine.Classes.TileAddons.Actions;
using UIEngine.Classes;

using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{

    internal class DestructableTile : ActionTile
    {

        public DestructableTile(Tile tile, IntermediateTmxShape intermediateTmxShape, string action) : base(tile,intermediateTmxShape, action)
        {
     
            if (tile.Sprite.GetType() == typeof(AnimatedSprite))
            {
                (tile.Sprite as AnimatedSprite).Paused = true;
            }
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PlayerInClickRange || PlayerInControllerActionRange)
                Tile.WithinRangeOfPlayer = true;
            if ((Tile.Sprite as AnimatedSprite).HasLoopedAtLeastOnce)
            {
                DestroyTileAndGetLoot();

            }

        }



        public override void Interact(bool isPlayer, Item heldItem)
        {
            if (isPlayer)
            {
                if (!PlayerInClickRange && !PlayerInControllerActionRange)
                    return;
                if (!MeetsItemRequirements(heldItem))
                    return;
            }

                (Tile.Sprite as AnimatedSprite).Paused = false;
            if (!IsPlayingASound)
            {
                PlaySound(Tile.GetCursorIconType().ToString());


            }


        }
    }
}
