using Globals.Classes;
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
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public class WorldItem : Collidable
    {
        private static readonly int _width = 16;
        private static readonly float _timeUntilTouchable = .4f;
        public Item Item { get; private set; }

        public string Name => Item.Name;
        public int Id => Item.Id;
        public bool Stackable => Item.Stackable;   
        public int MaxStackSize => Item.MaxStackSize;
        //How many items this item represents

        public int Count { get; private set; }
        public Sprite Sprite { get; set; }

        private SimpleTimer _simpleTimer;

        public bool FlaggedForRemoval { get; private set; }
        public WorldItem(Item item, int count, Vector2 position, Vector2? jettisonDirection)
        {
            Item = item;
            Count = count;
            Sprite = SpriteFactory.CreateWorldSprite(new Rectangle((int)position.X, (int)position.Y, _width, _width), Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet);
            CreateBody(position);
            Move(position);
            XOffSet = 8;
            YOffSet = 8;
            _simpleTimer = new SimpleTimer(_timeUntilTouchable);
            if(jettisonDirection != null)
                Jettison(jettisonDirection.Value, null);

        }
        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.Item },
               new List<Category>() { Category.Solid}, OnCollides, OnSeparates, blocksLight: true, userData: this);
        }

        public void Remove(int count)
        {
            if ((Count - count) > 1)
                throw new Exception($"Unable to remove more than contained");
            Count -= count;

            if (Count == 0)
                FlaggedForRemoval = true;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_simpleTimer != null && _simpleTimer.Run(gameTime))
            {

                SetCollidesWith(MainHullBody.Body, new List<Category>() { Category.Solid, Category.Player, Category.PlayerBigSensor, Category.TransparencySensor, Category.Item, Category.Grass });
                MainHullBody.Body.IgnoreGravity = true;
                _simpleTimer = null;
            }
          //  Jettison(new Vector2(10,10));

            Sprite.Update(gameTime, new Vector2(Position.X - XOffSet, Position.Y - YOffSet));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
            if (fixtureB.CollisionCategories.HasFlag(Category.PlayerBigSensor))
            {
                if(Gadgets.FirstOrDefault(x => x.GetType() == typeof(Magnetizer)) == null){
                    Gadgets.Add(new Magnetizer(this, (fixtureB.Body.UserData as Collidable)));
                }
            }
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

       
    }
}
