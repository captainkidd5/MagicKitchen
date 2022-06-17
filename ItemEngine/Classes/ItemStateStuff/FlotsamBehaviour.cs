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

namespace ItemEngine.Classes.ItemStateStuff
{
    internal class FlotsamBehaviour : ItemBehaviour
    {
        private static readonly float s_sinkTime = 2f;
        private static readonly float s_maxDistanceFromPlayer = 1400;

        private bool _isSinking = false;
        public FlotsamBehaviour(WorldItem worldItem) : base(worldItem)
        {
            SimpleTimer = new SimpleTimer(s_sinkTime);
            Vector2 velocity = GetJettisonDirection(worldItem.Position);

            WorldItem.MainHullBody.Body.LinearVelocity = velocity;

        }
        private Vector2 GetJettisonDirection(Vector2 spawnLocation)
        {
            Vector2 directionVector =Shared.PlayerPosition - spawnLocation;
            directionVector = directionVector / 80;
           // directionVector.Normalize();
            return directionVector;
        }
        public override bool OnCollides(List<PhysicsGadget> gadgets, Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.SolidLow) ||
               fixtureB.CollisionCategories.HasFlag((Category)PhysCat.SolidHigh))
            {
                //Floating object just hit the side of the island, probably
                _isSinking = true;
                WorldItem.Sprite.AddFaderEffect(0, null, true);
            }
                return base.OnCollides(gadgets,fixtureA, fixtureB, contact);
        }

        public override Vector2 Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(_isSinking && SimpleTimer.Run(gameTime))
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
