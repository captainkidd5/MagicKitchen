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
    internal class FloatingItemBehaviour : ItemBehaviour
    {
        private static readonly float s_sinkTime = 2f;

        private bool _isSinking = false;
        private Direction _floatDirection;
        public FloatingItemBehaviour(WorldItem worldItem) : base(worldItem)
        {
            SimpleTimer = new SimpleTimer(s_sinkTime);
            _floatDirection = Direction.Down;
            Vector2 velocity = Vector2Helper.GetTossDirectionFromDirectionFacing(_floatDirection);

            WorldItem.MainHullBody.Body.LinearVelocity = velocity;

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

                //Remove
                WorldItem.Remove(WorldItem.Count);
            }
            return Vector2.Zero;
         
        }
    }
}
