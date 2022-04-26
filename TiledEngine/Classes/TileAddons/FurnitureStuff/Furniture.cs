using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class Furniture : LocateableTileAddon
    {
        private readonly PlacedItemManager _placedItemManager;

        public List<PlacedItem> PlacedItems { get; set; }
        public Furniture(TileLocator tileLocator,Tile tile, PlacedItemManager placedItemManager) : base(tileLocator, tile)
        {
            Key = "furniture";
            _placedItemManager = placedItemManager;
        }

        public override void Load()
        {
            base.Load();
            PlacedItems = _placedItemManager.GetPlacedItemsFromTile(Tile);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach(PlacedItem placedItem in PlacedItems)
                placedItem.
        }
        public static Furniture GetFurnitureFromProperty(string value, TileLocator tileLocator, Tile tile)
        {
            switch (value)
            {
                case "diningTable":
                    return new DiningTable(tileLocator,tile);
                default:
                    throw new Exception($"Furniture type {value} does not exist");
            }
        }

    }
}
