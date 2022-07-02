using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using static DataModels.Enums;

namespace EntityEngine.ItemStuff.ItemStateStuff
{
    internal class FlotsamBehaviour : ItemBehaviour
    {
        private static readonly float s_sinkTime = 2f;
        private static readonly float s_maxDistanceFromPlayer = 1400;

        private bool _isSinking = false;
        public bool AllowSinking { get; set; } = true;
        public FlotsamBehaviour(WorldItem worldItem) : base(worldItem)
        {
            SimpleTimer = new SimpleTimer(s_sinkTime);
            Vector2 velocity = GetJettisonDirection(worldItem.Position);

            WorldItem.MainHullBody.Body.LinearVelocity = velocity;

        }
        private Vector2 GetJettisonDirection(Vector2 spawnLocation)
        {
            Vector2 directionVector = Shared.PlayerPosition - spawnLocation;
            directionVector.Normalize();
            directionVector = directionVector * 6;
            directionVector = OffSetFlotsamTrajectory(directionVector);
            return directionVector;
        }

        private Vector2 OffSetFlotsamTrajectory(Vector2 originalTrajectory)
        {
            return new Vector2(originalTrajectory.X + Settings.Random.Next(-7, 7), originalTrajectory.Y + Settings.Random.Next(-7, 7));
        }
        public override bool OnCollides(List<PhysicsGadget> gadgets, Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.SolidLow) ||
               fixtureB.CollisionCategories.HasFlag((Category)PhysCat.SolidHigh))
            {
                //Floating object just hit the side of the island, probably
                if (AllowSinking)
                {

                    _isSinking = true;
                    WorldItem.Sprite.AddFaderEffect(0, null, true);
                }

            }
            return base.OnCollides(gadgets, fixtureA, fixtureB, contact);
        }

        public override Vector2 Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!WorldItem.InWater())
            {
                WorldItem.ChangeState(ItemEngine.Classes.WorldItemState.None);
                return Vector2.Zero;

            }
            //if (_isSinking)
            //{
            //    WorldItem.Sprite.SwapSourceRectangle(new Rectangle(WorldItem.Sprite.SourceRectangle.X, WorldItem.Sprite.SourceRectangle.Y,
            //        WorldItem.Sprite.SourceRectangle.Width, WorldItem.Sprite.SourceRectangle.Height - 1));
            //}
            if (_isSinking && SimpleTimer.Run(gameTime))
            {
                WorldItem.Remove(WorldItem.Count);
            }
            else if (IsTooFarFromPlayer())
            {
                WorldItem.Remove(WorldItem.Count);
            }
            return Vector2.Zero;

        }

        private bool IsTooFarFromPlayer()
        {
            float distance = Vector2.Distance(WorldItem.Position, Shared.PlayerPosition);
            return distance > s_maxDistanceFromPlayer;
        }
    }
}
