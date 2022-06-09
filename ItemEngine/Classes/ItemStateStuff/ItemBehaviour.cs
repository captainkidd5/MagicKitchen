using Globals.Classes;
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
    internal abstract class ItemBehaviour
    {
        protected SimpleTimer SimpleTimer { get; set; }
        protected WorldItem WorldItem { get; set; }

        public ItemBehaviour(WorldItem worldItem)
        {
            WorldItem = worldItem;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual bool OnCollides(List<PhysicsGadget> gadgets, Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }
    }
}
