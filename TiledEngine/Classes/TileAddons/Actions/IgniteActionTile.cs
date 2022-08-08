using DataModels;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons.Actions
{
    internal class IgniteActionTile : ActionTile
    {
        public IgniteActionTile(TileObject tile, IntermediateTmxShape intermediateTmxShape, string actionType) : base(tile, intermediateTmxShape, actionType)
        {
            
     
        }

        public override void Load()
        {
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            IntervalAnimatedSprite spr = Tile.Sprite as IntervalAnimatedSprite;

    
            if (PlayerInClickRange && MouseHovering && Controls.IsClickedWorld)
            {
                if (!IsPlayingASound)
                {
                    PlayPackage(Tile.GetCursorIconType().ToString());


                }
                (spr).Paused = false;
               
            }
            if ((spr).HasLoopedAtLeastOnce)
            {
                TileAnimationHelper.SwitchGidToAnimationFrame(Tile.TileManager, Tile);

            }
        }

        public override Action Interact(ref ActionType? actionType, bool isPlayer, Item heldItem, Vector2 entityPosition, Direction directionEntityFacing)

        {
            return base.Interact(ref actionType, isPlayer, heldItem, entityPosition, directionEntityFacing);
        }

    }
}
