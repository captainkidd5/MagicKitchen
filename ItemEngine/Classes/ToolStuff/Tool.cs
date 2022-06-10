using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
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
    public class Tool : Collidable
    {

        public static Tool GetTool(string typeName)
        {
            return (Tool)System.Reflection.Assembly.GetExecutingAssembly()
                .CreateInstance($"ItemEngine.Classes.ToolStuff.{typeName}", true, System.Reflection.BindingFlags.CreateInstance,
                null, new object[] { }, null, null);
        }
        private Sprite _sprite;
        protected Collidable Holder { get; set; }

        private List<Tool> _tools;

        public Tool()
        {


        }
        public void Load(List<Tool> tools)
        {
            CreateBody(Position);
            _tools = tools;
            _sprite = SpriteFactory.CreateWorldSprite(Position,
                Item.GetItemSourceRectangle(ItemFactory.GetItemData("Wooden_Hook").Id),
                ItemFactory.ItemSpriteSheet);
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
            _sprite.Update(gameTime, new Vector2(MainHullBody.Position.X - XOffSet, MainHullBody.Position.Y - YOffSet));
            _sprite.Rotation = MainHullBody.Body.Rotation;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
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
