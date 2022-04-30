using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

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

        public Furniture(Tile tile,TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape,string actionType) :
            base( tile, tileManager, intermediateTmxShape, actionType)
        {
            Key = "furniture";
        }


        public void AddItem(int itemId)
        {
            PlacedItem placedItem = new PlacedItem(itemId, Tile);
            placedItem.Load(TopOfFurniture);
            PlacedItems.Add(placedItem);
            TileManager.PlacedItemManager.AddNewItem(placedItem);
        }
        public override void Load()
        {
            base.Load();
            _storageContainer = new StorageContainer(MaxPlacedItems);
            PlacedItems = TileManager.PlacedItemManager.GetPlacedItemsFromTile(Tile);
                foreach(PlacedItem placedItem in PlacedItems)
            {
                _storageContainer.AddItem(ItemFactory.GetItem(placedItem.ItemId), ref placedItem.ItemCount);
                placedItem.Load(TopOfFurniture);

            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach(PlacedItem placedItem in PlacedItems)
                placedItem.Draw(spriteBatch);
        }
        public static Furniture GetFurnitureFromProperty(string value,
            Tile tile,TileManager tileManager, IntermediateTmxShape tmxShape)
        {
            switch (value)
            {
                case "diningTable":
                    return new DiningTable(tile, tileManager, tmxShape, "None");
                default:
                    throw new Exception($"Furniture type {value} does not exist");
            }
        }

   

    }
}
