using DataModels.ItemStuff;
using DataModels.MapStuff;
using InputEngine.Classes;
using ItemEngine.Classes;
using ItemEngine.Classes.StorageStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes.Helpers;
using UIEngine.Classes;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class StorableFurniture : Furniture
    {
        protected StorageContainer StorageContainer;

        public List<PlacedItem> PlacedItems { get; set; }

        protected int MaxPlacedItems { get; set; } = 4;

        public int ItemCount => PlacedItems?.Count ?? 0;

        public bool MayPlaceItem => ItemCount <= MaxPlacedItems;

        protected int TotalStorageCapacity { get; set; }
        protected Vector2 TopOfFurniture => new Vector2(CenteredPosition.X, CenteredPosition.Y - IntermediateTmxShape.Radius);
        public StorableFurniture(FurnitureData furnitureData, Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape, string actionType) :
            base(furnitureData, tile, tileManager, intermediateTmxShape, actionType)
        {
            PlacedItems = new List<PlacedItem>();

            AddPlacedItems(furnitureData, tile);

           
        }
        /// <summary>
        /// Will set the storage slot at index to dissalow player from placing items into this slow (may still retrieve)
        /// </summary>
        /// <param name="index"></param>
        protected void SetPlaceLock(int index)
        {
            StorageContainer.Slots[index].SetPlaceLock();
        }

        protected void UnlockPlaceLock(int index)
        {
            StorageContainer.Slots[index].RemovePlaceLock();

        }

        protected virtual void AddPlacedItems(FurnitureData furnitureData, Tile tile)
        {
            TotalStorageCapacity = furnitureData.StorageRows * furnitureData.StorageColumns;
            for (int i = 0; i < TotalStorageCapacity; i++)
            {
                PlacedItems.Add(new PlacedItem(i, tile));
            }
        }
        protected virtual void CreateStorageContainer()
        {
            StorageContainer = new StorageContainer(TotalStorageCapacity, FurnitureData);

        }
        public override void Load()
        {
            base.Load();
            if (Tile.Sprite.GetType() == typeof(AnimatedSprite))
            {
                (Tile.Sprite as AnimatedSprite).Paused = true;

            }
            CreateStorageContainer();
            List<PlacedItem> loadedPlacedItems = TileManager.PlacedItemManager.GetPlacedItemsFromTile(Tile);
            bool loadedItemsWereSavedAtLeastOnce = loadedPlacedItems.Count > 0;
            //Means there were some saved items here previously. Load those in instead of default load
            if (loadedPlacedItems.Count > 0)
                PlacedItems = loadedPlacedItems;

            PlacedItems.OrderBy(x => x.ListIndex);



            for (int i = 0; i < PlacedItems.Count; i++)
            {
                PlacedItem placedItem = PlacedItems[i];
                if(FurnitureData.VisibleStorageIndicies != null)
                {
                    //Assign visible storage slots based on passed in indicies from json
                    if (FurnitureData.VisibleStorageIndicies.Any(x => x.Index == i))
                        StorageContainer.Slots[i].HoldsVisibleFurnitureItem = true;
                }
               

                if (placedItem.ItemId > 0)
                {

                    ItemData itemData = ItemFactory.GetItemData(placedItem.ItemId);
                    for (int j = 0; j < placedItem.ItemCount; j++)
                    {
                        StorageContainer.Slots[i].Add(itemData.Name);

                    }
                }

                placedItem.Load(TopOfFurniture + GetVisibleStorageIndexPositionOffSet(i), StorageContainer.Slots[i]);
                if (!loadedItemsWereSavedAtLeastOnce)
                    TileManager.PlacedItemManager.AddNewItem(placedItem);

            }
        }

        /// <summary>
        /// Returns position offset given by json data at specified index
        /// </summary>
        private Vector2 GetVisibleStorageIndexPositionOffSet(int index)
        {
            if (FurnitureData.VisibleStorageIndicies == null)
                return Vector2.Zero;
            VisibleStorageIndex v = FurnitureData.VisibleStorageIndicies.FirstOrDefault(x => x.Index == index);
            if(v == null)
                return Vector2.Zero;
            return new Vector2(v.XOffSet, v.YOffSet);
        }
        //public void AddItem(int index, int itemId)
        //{
        //    PlacedItems[index] = new PlacedItem(index, Tile);
        //    PlacedItems[index].Load(TopOfFurniture, _storageContainer.Slots[PlacedItems[index].ListIndex]);
        //    TileManager.PlacedItemManager.AddNewItem(PlacedItems[index]);
        //}
        public void OnUIClosed()
        {
            UI.StorageDisplayHandler.SecondaryStorageClosed -= OnUIClosed;
            (Tile.Sprite as AnimatedSprite).Paused = false;

            (Tile.Sprite as AnimatedSprite).SetTargetFrame(0);

        }
        public void RemoveItemAtIndex(int slotIndex, int count)
        {
            StorageContainer.Slots[slotIndex].Remove(count);
        }

        public void AddItemAtIndex(int slotIndex, string itemName, int count)
        {
            while(count > 0 && StorageContainer.Slots[slotIndex].Add(itemName))
            {
                count--;
            } 

        }
        public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (IsHovered(Controls.ControllerConnected))
        {
            UI.Cursor.ChangeCursorIcon(CursorIconType.Selectable);
            if (Controls.IsClickedWorld || Controls.WasGamePadButtonTapped(GamePadActionType.Select))
            {
                UI.ActivateSecondaryInventoryDisplay(FurnitureData.FurnitureType, StorageContainer);
                    //Subscribe to ui 
                    UI.StorageDisplayHandler.SecondaryStorageClosed += OnUIClosed;
                    (Tile.Sprite as AnimatedSprite).Paused = false;
                    (Tile.Sprite as AnimatedSprite).SetTargetFrame((Tile.Sprite as AnimatedSprite).AnimationFrames.Length - 1);


                }
            }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        for (int i = 0; i < StorageContainer.Slots.Count; i++)
        {
            if (StorageContainer.Slots[i].HoldsVisibleFurnitureItem)
                PlacedItems[i].Draw(spriteBatch);

        }

    }
    protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        return base.OnCollides(fixtureA, fixtureB, contact);
    }

    protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        base.OnSeparates(fixtureA, fixtureB, contact);
        if (fixtureB.CollisionCategories.HasFlag(
           (Category)PhysCat.FrontalSensor))
        {
            UI.DeactivateSecondaryInventoryDisplay();
        }
    }
    }

}
