using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using PhysicsEngine.Classes.Shapes;
using SoundEngine.Classes;
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
        private bool _isReturning;

        private float _maximumDistanceFromEntity = 140f;
        private static readonly Vector2 s_anchorOffSet = new Vector2(4, 11);
        public Hook()
        {

            SourceRectangle = new Rectangle(16, 0, 16, 16);
        }

        public override void Load(List<Tool> tools)
        {
            base.Load(tools);
            Sprite.Origin = new Vector2(XOffSet, YOffSet);
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
            if (Vector2.Distance(MainHullBody.Position, Holder.CenteredPosition) > _maximumDistanceFromEntity)
            {
                Return();
            }
        }
        public override void ActivateTool(Vector2 directionVector, Collidable holder)
        {
            base.ActivateTool(directionVector, holder);
            SoundFactory.PlaySoundEffect("HookFire");

            Vector2 newOffSet = new Vector2(BaseOffSet.X, BaseOffSet.Y);
            //if (directionVector.X < 0)
            //    newOffSet.X = BaseOffSet.X * -1;
            //if (directionVector.Y < 0)
            //    newOffSet.Y = BaseOffSet.Y * -1;

            Sprite.Origin = newOffSet;

        }
        protected override void AlterSpriteRotation()
        {
            //base.AlterSpriteRotation();
            if (_isReturning)
            {

                Vector2 directionVector = MainHullBody.Position - Holder.CenteredPosition;
                directionVector.Normalize();
                Sprite.Rotation = Vector2Helper.VectorToDegrees(directionVector);
                //int anchorX = XOffSet;
                //XOffSet = (int)(Math.Ceiling((float)s_anchorOffSet.X * directionVector.X));
                //XOffSet = XOffSet - (int)((float)anchorX * directionVector.Y);

                //YOffSet = (int)s_anchorOffSet.Y + (int)(Math.Ceiling((float)s_anchorOffSet.Y * directionVector.Y));
                //YOffSet = (int)((float)YOffSet * directionVector.X);

            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            LineUtility.DrawLine(null, spriteBatch, Position, Holder.CenteredPosition, Color.White, Sprite.LayerDepth);
        }
        private void Return()
        {
            Sprite.SetTargetFrame(1, true);

            if (Gadgets.FirstOrDefault(x => x.GetType() == typeof(Magnetizer)) == null)
            {
                AddGadget(new Magnetizer(this, Holder, 2));
                SetCollidesWith(MainHullBody.Body,
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.PlayerBigSensor });
                _isReturning = true;

            }
        }
        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.Item))
            {
                Return();
                SoundFactory.PlaySoundEffect("HookGrab");


            }
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.SolidHigh))
            {
                SoundFactory.PlaySoundEffect("HookMiss");
                Return();

            }
            else if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayerBigSensor))
            {
                Unload();
            }
            return base.OnCollides(fixtureA, fixtureB, contact);
        }
       
    }
}
