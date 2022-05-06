using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff.SeatingFurniture
{
    public class DiningTableSeat
    {
        public Direction DirectionAtTable { get; set; }
        public bool Occupied { get; set; }

        public DiningTableSeat(Direction directionAtTable)
        {
            DirectionAtTable = directionAtTable;
            Occupied = false;
        }
    }
}
