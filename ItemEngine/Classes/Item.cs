using DataModels;
using IOEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public class Item : Collidable, ISaveable
    {
        private readonly ItemData data;

        private static readonly int SpriteSheetDimension = 100;
        public ElementType ElementType { get; private set; }

        //How many items this item represents
        public int StackSize { get; set; }
        public int MaxStackSize => data.MaxStackSize;
        public int Id => data.Id;
        public string Name => data.Name;
        public string Description => data.Description;
        public Sprite Sprite { get; set; }

        //For example, items with durability should be stored as separate items even though
        //they might share the same id. Items are automatically set as unique if their max stack size is 1.
        public bool Unique { get; private set; }

        private List<Item> Items { get; set; }

        //GADGETS

        private Magnetizer Magnetizer { get; set; }
        private Ejector Ejector { get; set; }

        private ArtificialFloor ArtificialFloor { get; set; }
        internal Item(ItemData data, int count, Sprite sprite)
        {
            this.data = data;
            StackSize = count;
            Sprite = sprite;
            ElementType = ElementType.UI;
            if (data.MaxStackSize == 1)
                Unique = true;

            XOffSet = 0;
            YOffSet = 0;
        }


        public void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.PlayerBigSensor)
            {
                if (Magnetizer == null)
                {
                    // Magnetizer = new Magnetizer(this, fixtureB.Body.UserData as Collidable);
                    // Gadgets.Add(Magnetizer);

                }
            }
            if (fixtureB.CollisionCategories == Category.ArtificialFloor)
            {
                //make sure it's this item's specific floor body
                if (fixtureB.Body == ArtificialFloor.FloorBody)
                {
                    if (IsStopped())
                    {
                        AllowForPickUp();

                    }
  
                }

            }

        }

        private void AllowForPickUp()
        {
            Gadgets.Remove(ArtificialFloor);
            ArtificialFloor.Remove();
            ArtificialFloor = null;
            MainHullBody.Body.LinearVelocity = Vector2.Zero;
           //MainHullBody.Body.SetCollidesWith = Category.EntityBigSensor | Category.Player;
            MainHullBody.Body.IgnoreGravity = true;
        }
        public void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.PlayerBigSensor)
            {
                if (Magnetizer != null)
                {
                    Gadgets.Remove(Magnetizer);
                    Magnetizer = null;

                }
            }
        }
        /// <summary>
        /// Removes item from ground and adds it to inventory of grabber
        /// </summary>
        public void PickUp()
        {
            if (ElementType == ElementType.World)
            {
                ElementType = ElementType.UI;
                Items.Remove(this);
                Gadgets.Clear();
                Unload();

            }
            else
                throw new Exception("Cannot pick up an item which is already picked up");
        }

        /// <summary>
        /// Removes item from inventory and places it at the feet of the entity which dropped it
        /// </summary>
        /// <param name="entityPosition"></param>
        public void Drop(Vector2 entityPosition, Direction entityDirectionFacing, List<Item> items, int? countToDrop = null)
        {
            //Load ui item into world
            if (ElementType == ElementType.UI)
            {
                Vector2 directionToToss = GetTossDirectionFromDirectionFacing(entityDirectionFacing);
                Move(entityPosition + directionToToss);

                Item itemToAddToWorld;
                //Want to drop a specific amount, not all. So we generate a new item instead of using this one because it needs its own sprite.
                if (countToDrop != null && countToDrop < StackSize)
                {
                    Item newItem = ItemFactory.GenerateItem(Id, (int)countToDrop, Position);
                    itemToAddToWorld = newItem;
                }
                else
                {
                    itemToAddToWorld = this;
                }
                itemToAddToWorld.ElementType = ElementType.World;

                itemToAddToWorld.CreateBody(itemToAddToWorld.Position);

                itemToAddToWorld.Items = items;
                itemToAddToWorld.Items.Add(this);
                itemToAddToWorld.Ejector = new Ejector(this, directionToToss);
                itemToAddToWorld.ArtificialFloor = new ArtificialFloor(this, Category.Item);
                itemToAddToWorld.ArtificialFloor.FloorBody.SetCollisionCategory(Category.ArtificialFloor);

                itemToAddToWorld.Gadgets.Add(ArtificialFloor);
                itemToAddToWorld.Ejector.Trigger();
                itemToAddToWorld.Gadgets.Add(Ejector);


            }
            else
                throw new Exception("Cannot drop an item already on the ground");
        }

        private static readonly float directionMagnitude = 10;
        private Vector2 GetTossDirectionFromDirectionFacing(Direction directionFacing)
        {
            switch (directionFacing)
            {
                case Direction.Down:
                    return new Vector2(0, directionMagnitude);
                case Direction.Up:
                    return new Vector2(0, -directionMagnitude);
                case Direction.Left:
                    return new Vector2(-directionMagnitude, 0);
                case Direction.Right:
                    return new Vector2(directionMagnitude, 0);

                default:
                    throw new Exception(directionFacing.ToString() + " is invalid");
            }
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);

            HullBody itemBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, position,8f,
                new List<Category>() { Category.Item },
                new List<Category>() { Category.PlayerBigSensor, Category.ArtificialFloor },null,null);

            AddPrimaryBody(itemBody);

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Sprite.Update(gameTime, Position);
            if (ElementType == ElementType.World)
            {
                //if (ArtificialFloor != null && IsStopped())
                //{
                //    AllowForPickUp();
                //}
                Move(MainHullBody.Position);

            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public static Rectangle GetItemSourceRectangle(int itemId)
        {
            int Row = itemId % SpriteSheetDimension;
            int Column = (int)Math.Floor((double)itemId / (double)SpriteSheetDimension);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(StackSize);
            writer.Write((int)ElementType);
            writer.Write(Position.X);
            writer.Write(Position.Y);
        }

        public void LoadSave(BinaryReader reader)
        {

            throw new NotImplementedException();
        }
    }
}
