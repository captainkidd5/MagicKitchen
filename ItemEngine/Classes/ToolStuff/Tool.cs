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
        protected Rectangle SourceRectangle { get; set; }
        public static Tool GetTool(string typeName)
        {
            return (Tool)System.Reflection.Assembly.GetExecutingAssembly()
                .CreateInstance($"ItemEngine.Classes.ToolStuff.{typeName}", true, System.Reflection.BindingFlags.CreateInstance,
                null, new object[] { }, null, null);
        }
        protected AnimatedSprite Sprite { get; set; }
        protected Collidable Holder { get; set; }

        private List<Tool> _tools;

        public Tool()
        {


        }

        protected virtual AnimationFrame[] GetAnimationFrames()
        {
            throw new NotImplementedException();
        }
        public void Load(List<Tool> tools)
        {
            CreateBody(Position);
            _tools = tools;
            
            Sprite = SpriteFactory.CreateWorldAnimatedSprite(Position,SourceRectangle, 
                ItemFactory.ToolSheet, GetAnimationFrames());
            Sprite.Paused = true;
            XOffSet = 8;
            YOffSet = 8;
        }
        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f,
                new List<Category>() { (Category)PhysCat.Tool },
               new List<Category>() { (Category)PhysCat.Item }, OnCollides, OnSeparates,
               blocksLight: true, userData: this, mass: 1);


        }
        public virtual void ActivateTool(Vector2 directionVector, Collidable holder)
        {
            Holder = holder;
            MainHullBody.Body.ApplyLinearImpulse(directionVector * 100000f);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Sprite.Update(gameTime, new Vector2(MainHullBody.Position.X - XOffSet, MainHullBody.Position.Y - YOffSet));
            Sprite.Rotation = MainHullBody.Body.Rotation;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
       
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected void Unload()
        {
            ClearGadgets();
            MainHullBody.Destroy();
            _tools.Remove(this);

        }
    }
}
