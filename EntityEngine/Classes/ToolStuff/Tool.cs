using Globals.Classes.Helpers;
using ItemEngine.Classes;
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
using static DataModels.Enums;

namespace EntityEngine.Classes.ToolStuff
{
    public abstract class Tool : Collidable
    {
        public Item Item { get; set; }
        public bool Dirty { get; set; }
        protected Rectangle SourceRectangle { get; set; }
        public bool RequiresCharge { get; set; }
        public static Tool GetTool(Item item)
        {
            return (Tool)System.Reflection.Assembly.GetExecutingAssembly()
                .CreateInstance($"EntityEngine.Classes.ToolStuff.{item.ItemType.ToString()}", true, System.Reflection.BindingFlags.CreateInstance,
                null, new object[] { item }, null, null);
        }
        protected Sprite Sprite { get; set; }
        protected Collidable Holder { get; set; }

        public bool IsCharging { get; set; }
        public int ChargeAmt { get; set; }

        protected Point BaseOffSet = new Point(2, 7);

        protected Direction Direction{ get; set; }

        public Tool(Item item)
        {
            Item = item;

        }

        protected virtual AnimationFrame[] GetAnimationFrames()
        {
            throw new NotImplementedException();
        }
        protected virtual void LoadSprite()
        {
           Sprite = SpriteFactory.CreateWorldSprite(Position, SourceRectangle,
                   ItemFactory.ItemSpriteSheet);
            
        }
        public virtual void Load()
        {
            CreateBody(Position);
            LoadSprite();

         
            //XOffSet = BaseOffSet.X;
            //YOffSet = BaseOffSet.Y;
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



        public virtual void ReleaseTool(Direction direction, Vector2 directionVector, Collidable holder)
        {
            Holder = holder;
            Direction = direction;
            Load();
            IsCharging = false;



        }
        public override void Update(GameTime gameTime)
        {
          //  base.Update(gameTime);
            if (Sprite != null)
            {

                Sprite.Update(gameTime, new Vector2(Holder.CenteredPosition.X , Holder.CenteredPosition.Y ));
                AlterSpriteRotation(gameTime);
            }

        }
        protected virtual void AlterSpriteRotation(GameTime gameTime)
        {
            Sprite.Rotation = MainHullBody.Body.Rotation;

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite != null)
            {
                Sprite.Draw(spriteBatch);

            }
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        public virtual void Unload()
        {
            ClearGadgets();
            if (MainHullBody != null)
                MainHullBody.Destroy();
            Dirty = true;

        }
    }
}
