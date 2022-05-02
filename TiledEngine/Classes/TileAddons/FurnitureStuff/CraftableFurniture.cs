using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class CraftableFurniture : Furniture
    {
        public CraftableFurniture(Tile tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape, string actionType) :
            base(tile, tileManager, intermediateTmxShape, actionType)
        {
        }
    }
}
