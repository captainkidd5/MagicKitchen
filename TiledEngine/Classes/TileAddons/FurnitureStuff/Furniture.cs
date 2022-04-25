using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class Furniture : LocateableTileAddon
    {
        public Furniture(TileLocator tileLocator,Tile tile) : base(tileLocator, tile)
        {
            Key = "furniture";
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
