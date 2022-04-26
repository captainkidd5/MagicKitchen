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
    

        public List<PlacedItem> PlacedItems { get; set; }
        public Furniture(Tile tile,TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape,string actionType) :
            base( tile, tileManager, intermediateTmxShape, actionType)
        {
            Key = "furniture";
        }


        public void AddItem(int itemId)
        {
            PlacedItem placedItem = new PlacedItem(itemId, Tile);
            placedItem.Load(new Vector2(Position.X, Position.Y -12));
            PlacedItems.Add(placedItem);
            TileManager.PlacedItemManager.AddNewItem(placedItem);
        }
        public override void Load()
        {
            base.Load();
            PlacedItems = TileManager.PlacedItemManager.GetPlacedItemsFromTile(Tile);
                foreach(PlacedItem placedItem in PlacedItems)
                placedItem.Load(Tile.CentralPosition);

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
                    return new DiningTable(tile, tileManager, tmxShape, "Ignite");
                default:
                    throw new Exception($"Furniture type {value} does not exist");
            }
        }

    }
}
