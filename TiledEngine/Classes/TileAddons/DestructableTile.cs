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

    public class DestructableTile : ActionTile
    {
        protected bool RequireLoopBeforeDestruction { get; set; }
        public bool FlaggedForDestruction { get; set; }
        public DestructableTile(Tile tile, IntermediateTmxShape intermediateTmxShape, string action) : base(tile,intermediateTmxShape, action)
        {
     
            if (IsAnimatedSprite())
            {
                (tile.Sprite as AnimatedSprite).Paused = true;
            }
            RequireLoopBeforeDestruction = true;
        }

        private bool IsAnimatedSprite()
        {
            return Tile.Sprite != null && Tile.Sprite.GetType() == typeof(AnimatedSprite);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PlayerInClickRange || PlayerInControllerActionRange)
                Tile.WithinRangeOfPlayer = true;
            if (IsAnimatedSprite() && (Tile.Sprite as AnimatedSprite).HasLoopedAtLeastOnce)
            {
                FlaggedForDestruction = true;

            }

            if(FlaggedForDestruction)
                DestroyTileAndGetLoot();

        }
        protected override void AlterCursorAndAlertTile()
        {

            //UI.Cursor.ChangeCursorIcon(Tile.GetCursorIconType());
            Tile.AlertTileManagerCursorIconChanged();

            if (MeetsItemRequirements(UI.PlayerCurrentSelectedItem))
            {
                UI.Cursor.ChangeCursorIcon(CursorIconType.Break);

            }
            //else
            //{
            //    UI.Cursor.ChangeCursorIcon(CursorIconType.Selectable);

            //}
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

            if (!IsPlayingASound)
                PlaySound(GetDestructionSoundName());

            if (RequireLoopBeforeDestruction)
            {
                if (IsAnimatedSprite())
                    (Tile.Sprite as AnimatedSprite).Paused = false;
            }

            else
            {
                FlaggedForDestruction = true;


            }


        }
    }
}
