using DataModels.MapStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes.Helpers;

namespace TiledEngine.Classes.TileAddons.FurnitureStuff
{
    internal class Grill : CraftingFurniture
    {
        public Grill(FurnitureData furnitureData, TileObject tile, IntermediateTmxShape intermediateTmxShape, string actionType)
            : base(furnitureData, tile, intermediateTmxShape, actionType)
        {
            TotalStorageCapacity = 3;
            CraftAction = DataModels.ItemStuff.CraftAction.Grill;

        }
        protected override void AddPlacedItems(FurnitureData furnitureData, TileObject tile)
        {
 
           
                PlacedItems.Add(new PlacedOnItem(0, tile.TileData.GetKey()));
            
        }
        public override void Load()
        {
            base.Load();
        }
    }
}
