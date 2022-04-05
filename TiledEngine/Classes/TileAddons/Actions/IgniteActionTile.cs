using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            IntervalAnimatedSprite spr = Tile.Sprite as IntervalAnimatedSprite;

            if (PlayerInClickRange && MouseHovering && Controls.IsClicked)
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

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
