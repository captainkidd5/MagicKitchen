using Globals.Classes;
using Globals.Classes.Helpers;
using ItemEngine.Classes;
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
    internal class BouncingItemBehaviour : ItemBehaviour
    {
        private static readonly int s_minBounceVelocity = 50;
        private static readonly int s_maxBounceVelocity = 80;
        private static readonly float _timeUntilResting = 3f;
        float gravity = -500f;

        private bool sleep = false;
     
        // The velocity we're moving at. Only for the y-axis.
        float velocity;
        private byte _maxBounces = 3;
        private byte _currentBounces = 0;

        // A place for us to store the angularVelocity for the rigidbody2D. We don't want
        // to rotate the entire rigidbody because that would offset the shadow sprite which
        // follows the ground plane so we rotate the sprite by this velocity instead.
        float angularVelocity;

        // The amount we place the sprite above the game object to simulate it flying over
        // the ground. The shadow follows the gameobject in a straight line so when we launch
        // the sprite in an arc it looks like it's flying even though everything is still
        // entirely flat.
        float height;


        private Vector2? _tossedDirection;
        public BouncingItemBehaviour( WorldItem worldItem,Vector2? tossDirection = null) : base(worldItem)
        {
            SimpleTimer = new Globals.Classes.SimpleTimer(_timeUntilResting);


            WorldItem.SetPrimaryCollidesWith(new List<Category>() { (Category)PhysCat.SolidHigh, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Grass });
            // Set the initial velocity.
            velocity = Settings.Random.Next(s_minBounceVelocity, s_maxBounceVelocity);
            angularVelocity = WorldItem.MainHullBody.Body.AngularVelocity;
            WorldItem.MainHullBody.Body.AngularVelocity = 0f;
            _tossedDirection = tossDirection;
            if (tossDirection != null)
            WorldItem.MainHullBody.Body.ApplyLinearImpulse(tossDirection.Value * 1000);
        }

        public override Vector2 Update(GameTime gameTime)
        {

            base.Update(gameTime);
            velocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            height -= velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If the height is 0 we've landed and we stop moving.
            // The rigidbody2D's velocity is what moves us in a straight line across the ground,
            // we're only faking the vertical part.
            if (height >= 0 || TestIfShouldRest(gameTime) )
            {
                if(_currentBounces < _maxBounces && !WorldItem.InWater())
                {
                    _currentBounces++;
                    velocity = s_minBounceVelocity;
                    WorldItem.MainHullBody.Body.ApplyLinearImpulse(_tossedDirection.Value * -1 * 5);

                }
                else
                {
                    WorldItem.MainHullBody.Body.LinearVelocity = Vector2.Zero;
                    ReturnToNormal();
                }
                

            }
            return new Vector2(0, height);

        }
        /// <summary>
        /// Waits <see cref="_timeUntilResting"/> amount until artificial floor is removed and comes to a rest
        /// </summary>
        private bool TestIfShouldRest(GameTime gameTime)
        {
            if (SimpleTimer != null && SimpleTimer.Run(gameTime))
            {
                return true;
            }
            return false;
        }

        private void ReturnToNormal()
        {
            WorldItem.SetPrimaryCollidesWith(new List<Category>() { (Category)PhysCat.SolidHigh, (Category)PhysCat.SolidLow,  (Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Grass,(Category)PhysCat.Tool });
            WorldItem.IgnoreGravity(true);
            SimpleTimer = null;
            WorldItem.ClearGadgets();
            if (WorldItem.InWater())
            {
                WorldItem.ChangeState(WorldItemState.Floating);
                (WorldItem.ItemBehaviour as FlotsamBehaviour).AllowSinking = false;
            }
            else
            {
                WorldItem.ChangeState(WorldItemState.None);

            }

        }

        public override bool OnCollides(List<PhysicsGadget> gadgets, Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayerBigSensor))
            {


                WorldItem.ChangeState(WorldItemState.None);

            }
            return base.OnCollides(gadgets, fixtureA, fixtureB, contact);
        }
    }
}
