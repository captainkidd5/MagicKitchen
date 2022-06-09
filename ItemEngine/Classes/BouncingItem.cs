using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace ItemEngine.Classes
{
    internal class BouncingItem :WorldItem
    {

        private static readonly float _timeUntilResting = 3f;
        private SimpleTimer _bounceTimer;

        public BouncingItem(Item item, int count, Vector2 position, bool createFloor, Vector2? jettisonDirection) :
            base(item, count, position, jettisonDirection)
        {

            _bounceTimer = new SimpleTimer(_timeUntilResting);
            AddGadget(new ArtificialFloor(this));
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
            if (_bounceTimer != null && _bounceTimer.Run(gameTime))
            {

                SetCollidesWith(MainHullBody.Body, new List<Category>() { (Category)PhysCat.Solid, (Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Grass });
                MainHullBody.Body.IgnoreGravity = true;
                _bounceTimer = null;
                ClearGadgets();
            }
        }
    }
}
