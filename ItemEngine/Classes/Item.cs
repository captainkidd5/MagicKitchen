using DataModels;
using DataModels.ItemStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Gadgets;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public class Item
    {
        private readonly ItemData _itemData;

        private static readonly int SpriteSheetDimension = 100;
        public ItemType ItemType => _itemData.ItemType;
        public ToolTier ToolTier => _itemData.ToolTier;
        public bool Stackable => MaxStackSize > 1;
        public int MaxStackSize => _itemData.MaxStackSize;
        public int Id => _itemData.Id;
        public string Name => _itemData.Name;
        public string Description => _itemData.Description;
        public RecipeInfo RecipeInfo => _itemData.RecipeInfo;

        public int FuelValue => _itemData.FuelValue;

        public int PlacedItemGID => _itemData.PlacedItemGID;
        public bool PlacedItemIsForeground => _itemData.PlacedItemIsForeground;
        public bool PlaceableItem => _itemData.PlaceableItem;
        internal Item(ItemData data)
        {
            _itemData = data;
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
    }
}
