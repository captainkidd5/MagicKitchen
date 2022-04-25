using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class DiningTable : Furniture
    {

        public int TotalSeatingCapacity { get; private set; } = 4;
        public int OccupiedSeatCount { get; internal set; }

        public bool SeatingAvailable => OccupiedSeatCount < TotalSeatingCapacity;
        public DiningTable(TileLocator tileLocator, Tile tile) : base(tileLocator, tile)
        {
            SubKey = "diningTable";
        }

    }
}
