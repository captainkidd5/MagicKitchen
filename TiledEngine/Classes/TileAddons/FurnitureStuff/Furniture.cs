using DataModels.ItemStuff;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;
using UIEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class Furniture : LocateableTileAddon
    {
        private StorageContainer _storageContainer;

        public List<PlacedItem> PlacedItems { get; set; }

        protected int MaxPlacedItems { get; set; } = 1;

        public int ItemCount => PlacedItems?.Count ?? 0;

        public bool MayPlaceItem => ItemCount <= MaxPlacedItems;

        protected Vector2 TopOfFurniture => new Vector2(CenteredPosition.X, CenteredPosition.Y - IntermediateTmxShape.Radius);

        public Furniture(Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape, string actionType) :
            base(tile, tileManager, intermediateTmxShape, actionType)
        {
            Key = "furniture";
         
        }

        //private void ItemAdded(Item item)
        //{
        //    if (!PlacedItems.Any(x => x.ItemId == item.Id))
        //    {
        //        AddItem(item.Id);
        //    }
        //}

        //private void ItemRemoved(Item item)
        //{
        //    if (PlacedItems.Any(x => x.ItemId == item.Id))
        //    {
        //        PlacedItem placedItem = PlacedItems.FirstOrDefault(x => x.ItemId == item.Id);
        //        PlacedItems.Remove(placedItem);
        //        TileManager.PlacedItemManager.Remove(placedItem);

        //    }
        //}

        public void AddItem(int itemId)
        {
            PlacedItem placedItem = new PlacedItem(PlacedItems.Count - 1,itemId, Tile);
            placedItem.Load(TopOfFurniture, _storageContainer.Slots[placedItem.ListIndex]);
            PlacedItems.Add(placedItem);
            TileManager.PlacedItemManager.AddNewItem(placedItem);
        }
        public override void Load()
        {
            base.Load();
            _storageContainer = new StorageContainer(MaxPlacedItems);
            PlacedItems = TileManager.PlacedItemManager.GetPlacedItemsFromTile(Tile);
            PlacedItems.OrderBy(x => x.ListIndex);



            for(int i =0; i < PlacedItems.Count; i++)
            {
                PlacedItem placedItem = PlacedItems[i];
                ItemData itemData = ItemFactory.GetItemData(placedItem.ItemId);
                for (int j= 0; j < placedItem.ItemCount; j++)
                {
                    _storageContainer.Slots[i].Add(itemData.Name);

                }
                placedItem.Load(TopOfFurniture, _storageContainer.Slots[i]);

            }


          

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PlayerInClickRange && MouseHovering)
            {
                UI.Cursor.ChangeCursorIcon(CursorIconType.Rock);
                if (Controls.IsClickedWorld)
                {
                    UI.ActivateSecondaryInventoryDisplay(_storageContainer);
                    //if (StoreItem(UI.Cursor.HeldItem.Id))
                    //{
                    //    AddItem(UI.Cursor.HeldItem.Id);
                    //    UI.Cursor.RemoveSingleHeldItem();
                    //    Controls.ClickActionTriggeredThisFrame = true;
                    //}


                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (PlacedItem placedItem in PlacedItems)
                placedItem.Draw(spriteBatch);
        }
        public static Furniture GetFurnitureFromProperty(string value,
            Tile tile, TileManager tileManager, IntermediateTmxShape tmxShape)
        {
            switch (value)
            {
                case "diningTable":
                    return new DiningTable(tile, tileManager, tmxShape, "None");
                default:
                    throw new Exception($"Furniture type {value} does not exist");
            }
        }

        /// <summary>
        /// Returns true if was able to store a single item
        /// </summary>
        public bool StoreItem(int itemId)
        {
            if (MayPlaceItem)
            {
                AddItem(itemId);
                return true;
            }
            return false;
        }
        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
            if (fixtureB.CollisionCategories.HasFlag(VelcroPhysics.Collision.Filtering.Category.PlayerBigSensor))
            {
                Console.WriteLine("test");
                UI.DeactivateSecondaryInventoryDisplay();
            }
        }
    }
}
