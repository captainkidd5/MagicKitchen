using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ItemStuff
{

    public enum ItemType
    {
        None = 0,
        Axe = 21,
        Hammer = 22,
        Shovel = 23,
        Sword = 25,
        Bow = 26,
        Ammunition = 27,
        Tree = 40

    }

    public enum ToolTier
    {
        None = 0,
        Poor = 1,
        Good = 2,
        Excellent = 3
    }

    public enum AllowedPlacementTileType
    {
        land = 0,
        water = 1,
        all = 2,

    }
    public class ItemData
    {
        public int Id { get; set; }

        public string Name { get; set; }
         
        public string Description { get; set; }

        public int FuelValue { get; set; }

        public int MaxStackSize { get; set; }

        public int Price { get; set; }


        public RecipeInfo RecipeInfo { get; set; }

        public ToolTier ToolTier { get; set; }

        public ItemType ItemType { get; set; }

        public int PlacedItemGID { get; set; } = -1;
        public bool PlacedItemIsForeground { get; set; }
        public List<AllowedPlacementTileType> AllowedPlacementTileTypes { get; set; }
        public bool PlaceableItem => PlacedItemGID > -1;
        public void Load()
        {
            if(RecipeInfo != null)
            RecipeInfo.Name = Name;

            //If no value is supplied, will default to allowing land placement
            if (AllowedPlacementTileTypes == null && PlaceableItem)
                AllowedPlacementTileTypes = new List<AllowedPlacementTileType>() { AllowedPlacementTileType.land };
        }


    }
}
