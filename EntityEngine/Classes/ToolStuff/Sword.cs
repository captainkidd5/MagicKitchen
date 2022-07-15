using Globals.Classes.Helpers;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace EntityEngine.Classes.ToolStuff
{
    internal class Sword : Tool
    {
        private float _swingSpeed = 10f;
        public Sword(Item item) : base(item)
        {
            RequiresCharge = false;
            SourceRectangle = Item.GetItemSourceRectangle(item.Id);
        }
        protected override void LoadSprite()
        {
            base.LoadSprite();
        }

        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, 2f,16f,
                new List<Category>() { (Category)PhysCat.Tool },
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.SolidHigh }, OnCollides, OnSeparates,
               blocksLight: true, userData: this, mass: 1, isSensor: true);
            var joint = PhysicsManager.Weld(Holder.MainHullBody.Body, MainHullBody.Body, Vector2.Zero, Vector2.Zero, null, null);
        }

        private void Swing()
        {
        }
        protected override void AlterSpriteRotation(GameTime gameTime)
        {
            Sprite.Rotation += MainHullBody.Body.Rotation;

        }
        public override void ReleaseTool(Vector2 directionVector, Collidable holder)
        {
            base.ReleaseTool(directionVector, holder);
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 1000000f);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(Holder.CenteredPosition);
        }
    }
}
