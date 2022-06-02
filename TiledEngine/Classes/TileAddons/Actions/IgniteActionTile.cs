using InputEngine.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;


namespace TiledEngine.Classes.TileAddons.Actions
{
    internal class IgniteActionTile : ActionTile
    {
        public IgniteActionTile(Tile tile, TileManager tileManager , IntermediateTmxShape intermediateTmxShape, string actionType) : base(tile, tileManager, intermediateTmxShape, actionType)
        {
            
            if (tile.Sprite.GetType() == typeof(IntervalAnimatedSprite))
            {
                (tile.Sprite as IntervalAnimatedSprite).Paused = true;
            }
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
                    PlaySound(CursorIconType.ToString());


                }
                (spr).Paused = false;
               
            }
            if ((spr).HasLoopedAtLeastOnce)
            {
                TileAnimationHelper.SwitchGidToAnimationFrame(Tile, TileManager, TileSetPackage);

            }
        }

    }
}
