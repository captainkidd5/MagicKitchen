using DataModels.MapStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class CraftingTable : CraftingFurniture
    {
        public CraftingTable(FurnitureData furnitureData, TileObject tile, IntermediateTmxShape intermediateTmxShape, string actionType) : base(furnitureData, tile, intermediateTmxShape, actionType)
        {
            CraftAction = DataModels.ItemStuff.CraftAction.Smelt;

        }

        public override void Load()
        {
            base.Load();
        }
    }
}
