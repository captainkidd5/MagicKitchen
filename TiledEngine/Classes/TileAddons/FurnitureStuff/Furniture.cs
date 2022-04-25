using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    public class Furniture : LocateableTileAddon
    {
        public Furniture(TileLocator tileLocator) : base(tileLocator)
        {
            Key = "furniture";
        }

    }
}
