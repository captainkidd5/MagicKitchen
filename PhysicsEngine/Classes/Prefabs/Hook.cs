using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Joints;

namespace PhysicsEngine.Classes.Prefabs
{
    public class Hook : Collidable
    {
        private Sprite _hookSprite;
        public Hook()
        {
            
        }
        public void Load()
        {
            _hookSprite = SpriteFactory.CreateWorldSprite()
        }
        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.Hook },
               new List<Category>() { (Category)PhysCat.Item }, OnCollides, OnSeparates, blocksLight: true, userData: this);
        }
        public void FireHook(Vector2 directionVector)
        {
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 100f);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           
        }

        public void Draw()
        {
           
        }
    }
}

