using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace ItemEngine.Classes.ToolStuff
{
    public class Hook : Tool
    {


        private float _maximumDistanceFromEntity = 140f;
        public Hook()
        {


        }


        //public void FireHook(Vector2 directionVector, Collidable bodyFiring)
        //{
        //    _bodyFiring = bodyFiring;
        //    MainHullBody.Body.ApplyLinearImpulse(directionVector * 1000f);
        //}
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Vector2.Distance(MainHullBody.Position, Holder.Position) > _maximumDistanceFromEntity)
            {
                Return();
            }
        }


        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.Item))
            {
                Return();

            }
            else if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.Player))
            {
                Unload();
            }
            return base.OnCollides(fixtureA, fixtureB, contact);
        }
        private void Return()
        {
            if (Gadgets.FirstOrDefault(x => x.GetType() == typeof(Magnetizer)) == null)
            {
                AddGadget(new Magnetizer(this, Holder));
                SetCollidesWith(MainHullBody.Body,
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.Player });
            }
        }
    }
}
