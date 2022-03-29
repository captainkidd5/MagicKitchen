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
        public IgniteActionTile(Tile tile, TileManager tileManager, TileSetPackage tileSetPackage, IntermediateTmxShape intermediateTmxShape, string actionType) : base(tile, tileManager, tileSetPackage, intermediateTmxShape, actionType)
        {
            
            if (tile.Sprite.GetType() == typeof(AnimatedSprite))
            {
                (tile.Sprite as AnimatedSprite).Paused = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PlayerInClickRange && MouseHovering && Controls.IsClicked)
            {
                AnimatedSprite spr = Tile.Sprite as AnimatedSprite;
                (spr).Paused = false;
                if ((spr).HasLoopedAtLeastOnce)
                {
                    Console.WriteLine("test");

                    for(int i =0; i < spr.AnimationFrames.Length - 1; i++)
                    {

                    }
                    //TileUtility.SwitchGid(Tile, TileManager, _layer);
                    //TileManager.UpdateGrid(Tile.X, Tile.Y, GridStatus.Clear);


                }
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
