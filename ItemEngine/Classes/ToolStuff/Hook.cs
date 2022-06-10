using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
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

            SourceRectangle = new Rectangle(16, 0, 16, 16);
        }


        protected override AnimationFrame[] GetAnimationFrames()
        {
            AnimationFrame[] frames = new AnimationFrame[2];
            frames[0] = new AnimationFrame(0, 0, 0, 1f);
            frames[1] = new AnimationFrame(1, 0, 0, 1f);
            return frames;
        }
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
                Sprite.SetTargetFrame(1, true);
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
