using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
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
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace TiledEngine.Classes.TileAddons
{

    public class DestructableTile : ActionTile
    {
        protected bool RequireLoopBeforeDestruction { get; set; }
        public bool FlaggedForDestruction { get; set; }
        public DestructableTile(TileObject tile, IntermediateTmxShape intermediateTmxShape, string action) : base(tile,intermediateTmxShape, action)
        {
    
            RequireLoopBeforeDestruction = false;

        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandleDestruction();

        }

        private void HandleDestruction()
        {
            if (Tile.TileData.HasAnimationFrames(Tile.TileManager.TileSetPackage) && (Tile.Sprite as AnimatedSprite).HasLoopedAtLeastOnce)
            {
                FlaggedForDestruction = true;

            }

            if (FlaggedForDestruction)
                DestroyTileAndGetLoot();
        }

        protected override void AlterCursorAndAlertTile()
        {

            Tile.AlertTileManagerCursorIconChanged();

            if (MeetsItemRequirements(UI.PlayerCurrentSelectedItem))
            {
                CursorIconType iconType = CursorIconType.Break; 
                string actionProperty = Tile.TileData.GetProperty(Tile.TileManager.TileSetPackage, "action", true);
                if (!string.IsNullOrEmpty(actionProperty))
                {
                    string str = actionProperty.Split(',')[0];
                     iconType = (CursorIconType)(Enum.Parse(typeof(CursorIconType), str));

                }

                if(Tile.IsHighestOccupiedTile())
                UI.Cursor.ChangeCursorIcon(iconType);
            }

            

        }



        public override Action Interact(ref ActionType? actionType, bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)

        {
            actionType = ActionType.Interact;

            if (isPlayer)
            {
                if (!PlayerInClickRange && !PlayerInControllerActionRange)
                    return null;
                if (!MeetsItemRequirements(heldItem))
                    return null;
                if (Tile.TileManager.TileLocationHelper.IsOnTopOf(Tile.TileData, Shared.PlayerPosition))
                    return null;
            }
            if (Tile.Sprite.GetType() == typeof(AnimatedSprite) && (Tile.Sprite as AnimatedSprite).Paused)
                actionType = ActionType.Smash;

            return new Action(() => {
                if (RequireLoopBeforeDestruction)
                {
                  

                    (Tile.Sprite as AnimatedSprite).Paused = false;

                }

                else
                {
                    FlaggedForDestruction = true;


                }
                if (!IsPlayingASound())
                    PlayPackage(GetTileLootSound());
            }
            );
            

   

        }
    }
}
