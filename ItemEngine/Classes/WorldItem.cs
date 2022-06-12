using Globals.Classes;
using ItemEngine.Classes.ItemStateStuff;
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
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public enum WorldItemState
    {
        None = 0,
        Bouncing = 1,
        Floating = 2
    }
    public class WorldItem : Collidable, ISaveable
    {
        private static readonly int _width = 16;
        private static readonly float _timeUntilTouchable = 1f;
        public Item Item { get; private set; }
        internal WorldItemState WorldItemState { get; set; }
        public string Name => Item.Name;
        public int Id => Item.Id;
        public bool Stackable => Item.Stackable;
        public int MaxStackSize => Item.MaxStackSize;
        //How many items this item represents

        public int Count { get; private set; }
        public Sprite Sprite { get; set; }

        private bool ImmuneToPickup => _immunityTimer != null;
        private SimpleTimer _immunityTimer;


        public bool FlaggedForRemoval { get; private set; }


        private ItemBehaviour _itemBehaviour;


        internal new void AddGadget(PhysicsGadget gadget) => Gadgets.Add(gadget);
        internal new void ClearGadgets() => Gadgets.Clear();

        internal void IgnoreGravity(bool val) => MainHullBody.Body.IgnoreGravity = val;
        public void Save(BinaryWriter writer)
        {

        }

        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
        public WorldItem(Item item, int count, Vector2 position, WorldItemState worldItemState, Vector2? jettisonDirection)
        {
            Item = item;
            Count = count;
            Sprite = SpriteFactory.CreateWorldSprite(position, Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet, scale: new Vector2(.75f, .75f));
            WorldItemState = worldItemState;
            CreateBody(position);
            Move(position);
            XOffSet = 8;
            YOffSet = 8;
            _immunityTimer = new SimpleTimer(_timeUntilTouchable);


            switch (WorldItemState)
            {
                case WorldItemState.None:
                    break;
                case WorldItemState.Bouncing:
                    _itemBehaviour = new BouncingItemBehaviour(this);
                    break;
                case WorldItemState.Floating:
                    //  Jettison(jettisonDirection.Value, null);
                    _itemBehaviour = new FloatingItemBehaviour(this);
                    break;
            }

            if (jettisonDirection != null)
            {
                // Jettison(jettisonDirection.Value, null);
                MainHullBody.Body.IgnoreGravity = true;
            }

        }
        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.Item },
               new List<Category>() { (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh, (Category)PhysCat.Tool, (Category)PhysCat.ArtificialFloor }, OnCollides, OnSeparates, blocksLight: true, userData: this);
        }
        /// <summary>
        /// Waits <see cref="_timeUntilTouchable"/> amount until entities can interact with it
        /// </summary>
        private void TestIfImmunityDone(GameTime gameTime)
        {
            if (_immunityTimer != null && _immunityTimer.Run(gameTime))
            {

                SetPrimaryCollidesWith(new List<Category>() { (Category)PhysCat.SolidLow,(Category)PhysCat.SolidHigh, (Category)PhysCat.Tool,(Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item,
                    (Category)PhysCat.Grass, (Category)PhysCat.ArtificialFloor });
                _immunityTimer = null;



            }
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

        public void ChangeState(WorldItemState worldItemState)
        {
            WorldItemState = worldItemState;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ImmuneToPickup)
                TestIfImmunityDone(gameTime);
            Vector2 spriteBehaviourOffSet = Vector2.Zero;
            if (_itemBehaviour != null)
                spriteBehaviourOffSet = _itemBehaviour.Update(gameTime);
            Sprite.Update(gameTime, new Vector2(Position.X - XOffSet, Position.Y - YOffSet) + spriteBehaviourOffSet);


        }



        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (_itemBehaviour != null)
                _itemBehaviour.OnCollides(Gadgets, fixtureA, fixtureB, contact);
            if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.PlayerBigSensor))
            {
                //  if (Gadgets.FirstOrDefault(x => x.GetType() == typeof(Magnetizer)) == null)
                ClearGadgets();
                    AddGadget(new Magnetizer(this, (fixtureB.Body.Tag as Collidable)));
                //If magnetized, remove solids collisions
                SetCollidesWith(MainHullBody.Body, new List<Category>() {  (Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Grass});
                return true;
            }

                if (fixtureB.CollisionCategories.HasFlag((Category)PhysCat.Tool))
            {

                if (Gadgets.FirstOrDefault(x => x.GetType() == typeof(Magnetizer)) == null)
                    AddGadget(new Magnetizer(this, (fixtureB.Body.Tag as Collidable)));

                ChangeState(WorldItemState.None);

                //If magnetized, remove solids collisions
                SetCollidesWith(MainHullBody.Body, new List<Category>() {  (Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Grass});
            }


            return base.OnCollides(fixtureA, fixtureB, contact);

        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

    }
}
