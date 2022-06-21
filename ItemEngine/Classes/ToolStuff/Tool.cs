using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
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
    public class Tool : Collidable
    {
        public bool Dirty { get; set; }
        protected Rectangle SourceRectangle { get; set; }
        public bool RequiresCharge { get; set; }
        public static Tool GetTool(string typeName)
        {
            return (Tool)System.Reflection.Assembly.GetExecutingAssembly()
                .CreateInstance($"ItemEngine.Classes.ToolStuff.{typeName}", true, System.Reflection.BindingFlags.CreateInstance,
                null, new object[] { }, null, null);
        }
        protected AnimatedSprite Sprite { get; set; }
        protected Collidable Holder { get; set; }

        public bool IsCharging { get; set; }
        public int ChargeAmt { get; set; }

        protected Point BaseOffSet = new Point(2, 7);

        public Tool()
        {


        }

        protected virtual AnimationFrame[] GetAnimationFrames()
        {
            throw new NotImplementedException();
        }
        public virtual void Load()
        {
            CreateBody(Position);

            
            Sprite = SpriteFactory.CreateWorldAnimatedSprite(Position,SourceRectangle, 
                ItemFactory.ToolSheet, GetAnimationFrames());
            Sprite.Paused = true;
            XOffSet = BaseOffSet.X;
            YOffSet = BaseOffSet.Y;
        }
        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 8f,
                new List<Category>() { (Category)PhysCat.Tool },
               new List<Category>() { (Category)PhysCat.Item, (Category)PhysCat.SolidHigh }, OnCollides, OnSeparates,
               blocksLight: true, userData: this, mass: 1);


        }
        public virtual void BeginCharge(Collidable holder)
        {
            IsCharging = true;
            Holder = holder;
           // Load();
        }
        public virtual void ChargeUpTool(GameTime gameTime, Vector2 aimPosition)
        {

        }
        

        
        public virtual void ReleaseTool(Vector2 directionVector, Collidable holder)
        {
            Load();
            IsCharging = false;
            Holder = holder;
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 1000000f);
            Sprite.Rotation = Vector2Helper.VectorToDegrees(directionVector);
            int anchorX = XOffSet;
            XOffSet =(int)(Math.Ceiling((float)XOffSet * directionVector.X)) ;
            XOffSet = XOffSet - (int)((float)anchorX * directionVector.Y);

            YOffSet = YOffSet + (int)(Math.Ceiling((float)YOffSet * directionVector.Y)) ;
            YOffSet = (int)((float)YOffSet * directionVector.X);


        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(Sprite != null)
            {

            Sprite.Update(gameTime, new Vector2(MainHullBody.Position.X - XOffSet, MainHullBody.Position.Y - YOffSet));
            AlterSpriteRotation();
            }

        }
        protected virtual void AlterSpriteRotation()
        {
            Sprite.Rotation = MainHullBody.Body.Rotation;

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(Sprite != null)
            {
                Sprite.Draw(spriteBatch);

            }
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
       
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected void Unload()
        {
            ClearGadgets();
            MainHullBody.Destroy();
            Dirty = true;

        }
    }
}
