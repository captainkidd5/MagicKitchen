using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public class WorldItem : Collidable, ISaveable
    {
        private static readonly int _width = 16;
        private static readonly float _timeUntilTouchable = 1f;
        private static readonly float _timeUntilResting = 3f;
        public Item Item { get; private set; }

        public string Name => Item.Name;
        public int Id => Item.Id;
        public bool Stackable => Item.Stackable;   
        public int MaxStackSize => Item.MaxStackSize;
        //How many items this item represents

        public int Count { get; private set; }
        public Sprite Sprite { get; set; }

        private SimpleTimer _immunityTimer;
        private SimpleTimer _bounceTimer;


        public bool FlaggedForRemoval { get; private set; }



        public void Save(BinaryWriter writer)
        {
           
        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
        public WorldItem(Item item, int count, Vector2 position, Vector2? jettisonDirection)
        {
            Item = item;
            Count = count;
            Sprite = SpriteFactory.CreateWorldSprite(position, Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet, scale:new Vector2(.75f,.75f));
            CreateBody(position);
            Move(position);
            XOffSet = 8;
            YOffSet = 8;
            _immunityTimer = new SimpleTimer(_timeUntilTouchable);
            _bounceTimer = new SimpleTimer(_timeUntilResting);
            if(jettisonDirection != null)
            {
                Jettison(jettisonDirection.Value, null);
                AddGadget(new ArtificialFloor(this));
            }

        }
        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { Category.Item },
               new List<Category>() { Category.Solid, Category.ArtificialFloor}, OnCollides, OnSeparates,blocksLight: true, userData: this);
        }

        public void Remove(int count)
        {
            if ((Count - count) > 1)
                throw new Exception($"Unable to remove more than contained");
            Count -= count;

            if (Count == 0)
                FlaggedForRemoval = true;

            ClearGadgets();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            TestIfImmunityDone(gameTime);
            TestIfShouldRest(gameTime);
            //  Jettison(new Vector2(10,10));

            Sprite.Update(gameTime, new Vector2(Position.X - XOffSet, Position.Y - YOffSet));
        }

        /// <summary>
        /// Waits <see cref="_timeUntilTouchable"/> amount until entities can interact with it
        /// </summary>
        private void TestIfImmunityDone(GameTime gameTime)
        {
            if (_immunityTimer != null && _immunityTimer.Run(gameTime))
            {

                SetCollidesWith(MainHullBody.Body, new List<Category>() { Category.Solid, Category.Player, Category.PlayerBigSensor, Category.TransparencySensor, Category.Item, Category.Grass, Category.ArtificialFloor });
                _immunityTimer = null;
            }
        }

        /// <summary>
        /// Waits <see cref="_timeUntilResting"/> amount until artificial floor is removed and comes to a rest
        /// </summary>
        private void TestIfShouldRest(GameTime gameTime)
        {
            if (_bounceTimer != null && _bounceTimer.Run(gameTime))
            {

                SetCollidesWith(MainHullBody.Body, new List<Category>() { Category.Solid, Category.Player, Category.PlayerBigSensor, Category.TransparencySensor, Category.Item, Category.Grass });
                MainHullBody.Body.IgnoreGravity = true;
                _bounceTimer = null;
                ClearGadgets();
            }
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
                    AddGadget(new Magnetizer(this, (fixtureB.Body.UserData as Collidable)));
                    ArtificialFloor floor = Gadgets.FirstOrDefault(x => x.GetType() == typeof(ArtificialFloor)) as ArtificialFloor;
                    if (floor != null)
                    {
                        floor.Destroy();
                        Gadgets.Remove(floor);
                    }
                }
            }
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

    }
}
