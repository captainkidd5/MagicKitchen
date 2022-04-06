using DataModels;
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
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace ItemEngine.Classes
{
    public class Item
    {
        private readonly ItemData _itemData;

        private static readonly int SpriteSheetDimension = 100;

        public bool Stackable => MaxStackSize > 1;
        public int MaxStackSize => _itemData.MaxStackSize;
        public int Id => _itemData.Id;
        public string Name => _itemData.Name;
        public string Description => _itemData.Description;

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
