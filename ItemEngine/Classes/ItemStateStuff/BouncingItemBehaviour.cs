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
        private static readonly float _timeUntilResting = 3f;

        public BouncingItemBehaviour(WorldItem worldItem) : base(worldItem)
        {
            SimpleTimer = new Globals.Classes.SimpleTimer(_timeUntilResting);
            worldItem.AddGadget(new ArtificialFloor(worldItem));

            WorldItem.SetPrimaryCollidesWith(new List<Category>() { (Category)PhysCat.SolidHigh, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Grass });

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TestIfShouldRest(gameTime);

        }
       

        /// <summary>
        /// Waits <see cref="_timeUntilResting"/> amount until artificial floor is removed and comes to a rest
        /// </summary>
        private void TestIfShouldRest(GameTime gameTime)
        {
            if (SimpleTimer != null && SimpleTimer.Run(gameTime))
            {

                WorldItem.SetPrimaryCollidesWith(new List<Category>() { (Category)PhysCat.SolidHigh, (Category)PhysCat.SolidLow,  (Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Grass,(Category)PhysCat.Tool });
                WorldItem.IgnoreGravity(true);
                SimpleTimer = null;
                WorldItem.ClearGadgets();
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
