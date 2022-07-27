using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using SoundEngine.Classes;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public class Item : ISaveable
    {
        private  ItemData _itemData;

        private static readonly int SpriteSheetDimension = 100;
        public ItemType ItemType => _itemData.ItemType;
        public ToolTier ToolTier => _itemData.ToolTier;
        public bool Stackable => MaxStackSize > 1;
        public ushort MaxStackSize => _itemData.MaxStackSize;
        public ushort Id => _itemData.Id;
        public string Name => _itemData.Name;
        public string Description => _itemData.Description;
        public RecipeInfo RecipeInfo => _itemData.RecipeInfo;

        public int FuelValue => _itemData.FuelValue;
        public byte FoodValue => _itemData.FoodValue;


        public int PlacedItemGID => _itemData.PlacedItemGID;
        public Layers? LayerToPlace => _itemData.LayerToPlace;
        public bool PlaceableItem => _itemData.PlaceableItem;
        public string PlacementSound => _itemData.PlacementSound;
        public byte MaxDurability => _itemData.MaxDurability;
        public byte ArmorValue => _itemData.ArmorValue;
        public byte DamageValue => _itemData.DamageValue;



        public EquipmentType EquipmentSlot => _itemData.EquipmentSlot;
        public byte EquipmentYIndex => _itemData.EquipmentYIndex;
        public ushort CurrentDurability { get;  private set; }
        public List<AllowedPlacementTileType> AllowedPlacementTileTypes => _itemData.AllowedPlacementTileTypes;
        internal Item(ItemData data)
        {
            _itemData = data;
            CurrentDurability = MaxDurability;
        }
        public Item()
        {
        }
        /// <summary>
        /// Returns true if item just broke
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public bool RemoveDurability(int amt = 1)
        {
            if (MaxDurability <= 0)
                throw new Exception($"Should not try to remove durability from item with no max durability");
            CurrentDurability -= (ushort)amt;
            if (CurrentDurability <= 0)
            {
                SoundFactory.PlayEffectPackage("ToolBreak");
                return true;

            }
            return false;

        }
        public static Rectangle GetItemSourceRectangle(int itemId)
        {
            int Row = itemId % SpriteSheetDimension;
            int Column = (int)Math.Floor((double)itemId / (double)SpriteSheetDimension);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }



        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(CurrentDurability);
        }

        public void LoadSave(BinaryReader reader)
        {
            _itemData = ItemFactory.GetItemData(reader.ReadUInt16());
            CurrentDurability = reader.ReadUInt16();

        }

        public void SetToDefault( )
        {
            CurrentDurability = _itemData.MaxDurability;
        }
    }
}
