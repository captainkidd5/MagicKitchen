using DataModels.ItemStuff;
using InputEngine.Classes;
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
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class Furniture : LocateableTileAddon
    {
        private StorageContainer _storageContainer;

        public List<PlacedItem> PlacedItems { get; set; }

        protected int MaxPlacedItems { get; set; } = 4;

        public int ItemCount => PlacedItems?.Count ?? 0;

        public bool MayPlaceItem => ItemCount <= MaxPlacedItems;

        protected Vector2 TopOfFurniture => new Vector2(CenteredPosition.X, CenteredPosition.Y - IntermediateTmxShape.Radius);

        public Furniture(Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape, string actionType) :
            base(tile, tileManager, intermediateTmxShape, actionType)
        {
            Key = "furniture";
            PlacedItems = new List<PlacedItem>();
            for (int i = 0; i < MaxPlacedItems; i++)
            {
                PlacedItems.Add(new PlacedItem(i, tile));
            }

        }

        public void AddItem(int index, int itemId)
        {
            PlacedItems[index] = new PlacedItem(itemId, Tile);
            PlacedItems[index].Load(TopOfFurniture, _storageContainer.Slots[PlacedItems[index].ListIndex]);
            TileManager.PlacedItemManager.AddNewItem(PlacedItems[index]);
        }
        public override void Load()
        {
            base.Load();
            _storageContainer = new StorageContainer(MaxPlacedItems);
            List<PlacedItem> loadedPlacedItems = TileManager.PlacedItemManager.GetPlacedItemsFromTile(Tile);
            bool loadedItemsWereSavedAtLeastOnce = loadedPlacedItems.Count > 0;
            //Means there were some saved items here previously. Load those in instead of default load
            if (loadedPlacedItems.Count > 0)
                PlacedItems = loadedPlacedItems;

            PlacedItems.OrderBy(x => x.ListIndex);



            for (int i = 0; i < PlacedItems.Count; i++)
            {
                PlacedItem placedItem = PlacedItems[i];
              // if (i == 0)
                    _storageContainer.Slots[i].HoldsVisibleFurnitureItem = true;
                if (placedItem.ItemId > 0)
                {

                    ItemData itemData = ItemFactory.GetItemData(placedItem.ItemId);
                    for (int j = 0; j < placedItem.ItemCount; j++)
                    {
                        _storageContainer.Slots[i].Add(itemData.Name);

                    }
                }

                placedItem.Load(TopOfFurniture, _storageContainer.Slots[i]);
                if(!loadedItemsWereSavedAtLeastOnce)
                    TileManager.PlacedItemManager.AddNewItem(placedItem);

            }




        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PlayerInControllerActionRange || MouseHovering)
            {
                UI.Cursor.ChangeCursorIcon(CursorIconType.Selectable);
                if (Controls.IsClickedWorld || Controls.GamePadButtonTapped(GamePadActionType.Select))
                {
                    UI.ActivateSecondaryInventoryDisplay(StorageType.Craftable, _storageContainer);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            for(int i =0; i < _storageContainer.Slots.Count; i++)
            {
                if(_storageContainer.Slots[i].HoldsVisibleFurnitureItem)
                    PlacedItems[i].Draw(spriteBatch);

            }
           
        }
        public static Furniture GetFurnitureFromProperty(string value,
            Tile tile, TileManager tileManager, IntermediateTmxShape tmxShape)
        {
            Furniture furniture = (Furniture)System.Reflection.Assembly.GetExecutingAssembly()
                           .CreateInstance($"TiledEngine.Classes.TileAddons.FurnitureStuff.{value}", true, System.Reflection.BindingFlags.CreateInstance,
                           null, new object[] { tile, tileManager, tmxShape, "None" }, null, null);

            return furniture;

        }


        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
            if (fixtureB.CollisionCategories.HasFlag(
                VelcroPhysics.Collision.Filtering.Category.FrontalSensor))
            {
                UI.DeactivateSecondaryInventoryDisplay();
            }
        }
    }
}
