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

namespace ItemEngine.Classes.ItemStateStuff
{
    internal class BouncingItemBehaviour : ItemBehaviour
    {
        public BouncingItemBehaviour(WorldItem worldItem) : base(worldItem)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        /// <summary>
        /// Waits <see cref="_timeUntilTouchable"/> amount until entities can interact with it
        /// </summary>
        private void TestIfImmunityDone(GameTime gameTime)
        {
            if (SimpleTimer != null && SimpleTimer.Run(gameTime))
            {

                WorldItem.SetCollidesWith(MainHullBody.Body, new List<Category>() { (Category)PhysCat.Solid, (Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Grass, (Category)PhysCat.ArtificialFloor });
                SimpleTimer = null;



            }
        }
       

        public override bool OnCollides(List<PhysicsGadget> gadgets, Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayerBigSensor))
            {
                ArtificialFloor floor = gadgets.FirstOrDefault(x => x.GetType() == typeof(ArtificialFloor)) as ArtificialFloor;
                if (floor != null)
                {
                    floor.Destroy();
                    gadgets.Remove(floor);
                }

                WorldItem.ChangeState(WorldItemState.None);

            }
            return base.OnCollides(gadgets, fixtureA, fixtureB, contact);
        }
    }
}
