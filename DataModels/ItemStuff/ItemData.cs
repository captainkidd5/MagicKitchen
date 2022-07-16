using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;

namespace DataModels.ItemStuff
{
    
    public enum ItemType
    {
        None = 0,
        Hook = 20,
        Axe = 21,
        Hammer = 22,
        Shovel = 23,
        Sword = 25,
        Bow = 26,
        Ammunition = 27,
        Food = 28,
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
        deepWater = 2,
        woodWall = 3,
        all = 4,

    }
    public class ItemData
    {
        public ushort Id { get; set; }

        public string Name { get; set; }
        public string ProperName => Name.Replace("_", " ");

        public string Description { get; set; }

        public byte FuelValue { get; set; }
        public byte FoodValue { get; set; }

        public ushort MaxStackSize { get; set; }

        public ushort Price { get; set; }


        public RecipeInfo RecipeInfo { get; set; }

        public ToolTier ToolTier { get; set; }

        public ItemType ItemType { get; set; }

        public int PlacedItemGID { get; set; } = -1;


        public Layers? LayerToPlace { get; set; }
        public List<AllowedPlacementTileType> AllowedPlacementTileTypes { get; set; }
        public bool PlaceableItem => PlacedItemGID > -1;

       
        public EquipmentType EquipmentSlot { get; set; }
        public byte EquipmentYIndex { get; set; }
        public byte MaxDurability { get; set; }
        public byte ArmorValue { get; set; }
        public byte DamageValue { get; set; }
        public void Load()
        {
            if(RecipeInfo != null)
            RecipeInfo.Name = Name;

            if (PlaceableItem && LayerToPlace == null)
                LayerToPlace = Layers.foreground;
            //If no value is supplied, will default to allowing land placement
            if (AllowedPlacementTileTypes == null && PlaceableItem)
                AllowedPlacementTileTypes = new List<AllowedPlacementTileType>() { AllowedPlacementTileType.land };
        }


    }
}
