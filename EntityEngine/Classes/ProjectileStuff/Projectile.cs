using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace EntityEngine.Classes.ProjectileStuff
{
    internal abstract class Projectile : Collidable
    {
        public Projectile()
        {

        }
        public override void Move(Vector2 newPosition)
        {
            base.Move(newPosition);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.Projectile },
                new List<Category>() { (Category)PhysCat.Solid, (Category)PhysCat.Grass }, OnCollides,
                OnSeparates, ignoreGravity: true, blocksLight: true, userData: this);

            BigSensorCollidesWithCategories = new List<Category>() {  };
            BigSensor = PhysicsManager.CreateCircularHullBody(BodyType.Static, position, 16f, new List<Category>() { (Category)PhysCat.PlayerBigSensor }, BigSensorCollidesWithCategories,
               OnCollides, OnSeparates, sleepingAllowed: true, isSensor: true, userData: this);

        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
