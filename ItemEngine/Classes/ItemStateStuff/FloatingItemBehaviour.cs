using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Gadgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace ItemEngine.Classes.ItemStateStuff
{
    internal class FloatingItemBehaviour : ItemBehaviour
    {
        public FloatingItemBehaviour(WorldItem worldItem) : base(worldItem)
        {
        }

        public override bool OnCollides(List<PhysicsGadget> gadgets, Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(gadgets,fixtureA, fixtureB, contact);
        }

        public override Vector2 Update(GameTime gameTime)
        {
            base.Update(gameTime);

            return Vector2.Zero;
         
        }
    }
}
