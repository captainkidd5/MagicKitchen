using EntityEngine.ItemStuff.ItemStateStuff;
using Globals.Classes;
using Globals.Classes.Time;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using SpriteEngine.Classes;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace EntityEngine.ItemStuff
{

    public class WorldItem : Collidable, ISaveable
    {
        private readonly TileManager _tileManager;
        private readonly Vector2? _jettisonDirection;
        public static float TTL = 4000; //400 seconds
        public Item Item { get; private set; }
        internal WorldItemState WorldItemState { get; set; }
        public string Name => Item.Name;
        public ushort Id => Item.Id;
        public bool Stackable => Item.Stackable;
        public int MaxStackSize => Item.MaxStackSize;
        //How many items this item represents

        public ushort Count { get; private set; }
        public Sprite Sprite { get; set; }
        public Shadow Shadow { get; set; }
        private bool ImmuneToPickup => _immunityTimer != null;
        private SimpleTimer _immunityTimer;


        public bool FlaggedForRemoval { get; private set; }


        internal ItemBehaviour ItemBehaviour { get; set; }


        internal new void AddGadget(PhysicsGadget gadget) => Gadgets.Add(gadget);
        internal new void ClearGadgets() => Gadgets.Clear();

        internal void IgnoreGravity(bool val) => MainHullBody.Body.IgnoreGravity = val;

        public float TimeCreated { get; set; }
        public void Save(BinaryWriter writer)
        {

        }
        public bool InTileManagerUpdateRange()
        {
            return _tileManager.IsWithinUpdateRange(Position);
        }
        public void LoadSave(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
        public WorldItem(TileManager tileManager, Item item, int count, Vector2 position, WorldItemState worldItemState, Vector2? jettisonDirection)
        {
            Position = position;
            _tileManager = tileManager;
            Item = item;
            Count = (ushort)count;
            Sprite = SpriteFactory.CreateWorldSprite(position, Item.GetItemSourceRectangle(item.Id), ItemFactory.ItemSpriteSheet, scale: new Vector2(.75f, .75f));
            WorldItemState = worldItemState;
            _jettisonDirection = jettisonDirection;
            //CreateBody(position);
            //if(MainHullBody.Body.World == null)
            //    Console.WriteLine("test");
            //Move(position);
            XOffSet = 8;
            YOffSet = 8;
            _immunityTimer = new SimpleTimer(1f);
            TimeCreated = Clock.TotalTime;
            Sprite.CustomLayer = null;
      


        }

        private void GetBehaviour(Vector2? jettisonDirection)
        {
            switch (WorldItemState)
            {

                //want to set custom layer when bouncing because the sprite should be drawn at the
                //initial launch point's layerdepth to help with the illusion

                case WorldItemState.None:
                    ItemBehaviour = null;
                    Sprite.SwapSourceRectangle(new Rectangle(Sprite.SourceRectangle.X, Sprite.SourceRectangle.Y, 16, 16));
                    Shadow = new Shadow(ShadowType.Item, CenteredPosition, ShadowSize.Small, ItemFactory.ItemSpriteSheet);
                    Sprite.CustomLayer = null;
                    break;
                case WorldItemState.Bouncing:
                    ItemBehaviour = new BouncingItemBehaviour(this, jettisonDirection);
                    Sprite.CustomLayer = Sprite.GetYAxisLayerDepth();
                    Shadow = new Shadow(ShadowType.Item, CenteredPosition, ShadowSize.Small, ItemFactory.ItemSpriteSheet);


                    break;
                case WorldItemState.Floating:
                    //  Jettison(jettisonDirection.Value, null);
                    PlayPackage("Splash");
                    Shadow = null;
                    Sprite.CustomLayer = null;

                    ItemBehaviour = new FlotsamBehaviour(this);
                    //Drawing half the sprite creates the illusion that half of it is submerged
                    Sprite.SwapSourceRectangle(new Rectangle(Sprite.SourceRectangle.X, Sprite.SourceRectangle.Y, 16, 8));
                    break;
            }
        }

        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.Item },
               new List<Category>() { (Category)PhysCat.SolidLow, (Category)PhysCat.SolidHigh, (Category)PhysCat.Tool, (Category)PhysCat.ArtificialFloor },
               OnCollides, OnSeparates, blocksLight: true, userData: this);

            GetBehaviour(_jettisonDirection);

            if (_jettisonDirection != null)
            {
                // Jettison(jettisonDirection.Value, null);
                MainHullBody.Body.IgnoreGravity = true;
            }




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
            Count -= (ushort)count;

            if (Count == 0)
                FlaggedForRemoval = true;

            ClearGadgets();
            Shadow = null;

        }

        public void ChangeState(WorldItemState worldItemState)
        {
            WorldItemState = worldItemState;
            GetBehaviour(null);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MainHullBody == null)
                CreateBody(Position);
            if (ImmuneToPickup)
                TestIfImmunityDone(gameTime);
            Vector2 spriteBehaviourOffSet = Vector2.Zero;
            if (ItemBehaviour == null)
            {
                if (InWater())
                {
                    if(Gadgets.FirstOrDefault(x => x.GetType() == typeof(Magnetizer)) == null)
                    ChangeState(ItemEngine.Classes.WorldItemState.Floating);

                }
            }

            else
            {
                spriteBehaviourOffSet = ItemBehaviour.Update(gameTime);

            }

            Sprite.Update(gameTime, CenteredPosition + spriteBehaviourOffSet);
            if (Shadow != null)
                Shadow.Update(gameTime, new Vector2(CenteredPosition.X, CenteredPosition.Y - 1), false);
            if (TimeCreated + TTL < Clock.TotalTime)
                Remove(Count);

        }

        public void SetStandardCollides()
        {
            SetPrimaryCollidesWith(new List<Category>() { (Category)PhysCat.SolidLow,(Category)PhysCat.SolidHigh, (Category)PhysCat.Tool,(Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item,
                    (Category)PhysCat.Grass, (Category)PhysCat.ArtificialFloor });
            Gadgets.Clear();
            MainHullBody.Body.IgnoreGravity = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
            if (Shadow != null)
                Shadow.Draw(spriteBatch);
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (ItemBehaviour != null)
                ItemBehaviour.OnCollides(Gadgets, fixtureA, fixtureB, contact);
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

                if (InWater())
                {
                    PlayPackage("Splash");

                }
                //If magnetized, remove solids collisions
                SetCollidesWith(MainHullBody.Body, new List<Category>() {  (Category)PhysCat.Player,
                    (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.TransparencySensor, (Category)PhysCat.Item, (Category)PhysCat.Grass});
            }


            return base.OnCollides(fixtureA, fixtureB, contact);

        }
        internal bool InWater()
        {
            return _tileManager.IsTypeOfTile("water", Position) || _tileManager.IsTypeOfTile("deepWater", Position);
        }
        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            base.OnSeparates(fixtureA, fixtureB, contact);
        }


        public void SetToDefault()
        {
            //throw new NotImplementedException();
        }
    }
}
