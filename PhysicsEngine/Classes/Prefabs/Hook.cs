using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Gadgets;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using tainicom.Aether.Physics2D.Dynamics.Joints;

namespace PhysicsEngine.Classes.Prefabs
{
    public class Hook : Collidable
    {
        private Sprite _hookSprite;
        private Collidable _bodyFiring;

        private List<Hook> _hooks;

        private float _maximumDistanceFromEntity = 140f;
        public Hook()
        {


        }
        public void Load(List<Hook> hooks)
        {
            //_hookSprite = SpriteFactory.CreateWorldSprite()
            CreateBody(Position);
            _hooks = hooks;
        }
        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f,
                new List<Category>() { (Category)PhysCat.Hook },
               new List<Category>() { (Category)PhysCat.Item }, OnCollides, OnSeparates,
               blocksLight: true, userData: this, mass: 1);


        }
        public void FireHook(Vector2 directionVector, Collidable bodyFiring)
        {
            _bodyFiring = bodyFiring;
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 1000f);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(Vector2.Distance(MainHullBody.Position, _bodyFiring.Position) > _maximumDistanceFromEntity)
            {
                Return();
            }
        }

        public void Draw()
        {

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
                AddGadget(new Magnetizer(this, _bodyFiring));
                SetCollidesWith(MainHullBody.Body,
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.Player });
            }
        }
        private void Unload()
        {
            ClearGadgets();
            MainHullBody.Destroy();
            _hooks.Remove(this);

        }
    }
}

