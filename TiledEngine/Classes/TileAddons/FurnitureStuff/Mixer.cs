using DataModels.MapStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class Mixer : CraftingFurniture
    {
        public Mixer(FurnitureData furnitureData, TileObject tile, TileManager tileManager,
            IntermediateTmxShape intermediateTmxShape, string actionType) :
            base(furnitureData, tile, tileManager, intermediateTmxShape, actionType)
        {
            TotalStorageCapacity = 6;
        }
    }
}
