using UIEngine.Classes.ButtonStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine.Classes;
using TextEngine;
using static Globals.Classes.Settings;

namespace UIEngine.Classes.Storage
{
    /// <summary>
    /// Graphical and interactable wrapper of storage container
    /// </summary>
    internal class InventoryDisplay : InterfaceSection
    {
        protected static readonly int _buttonWidth = 64;
        protected int Rows;
        protected int Columns;
        protected StorageContainer StorageContainer { get; set; }

        protected List<InventorySlot> InventorySlots { get; set; }

        internal InventorySlot SelectedSlot { get; set; }

        internal int Width { get { return DrawEndIndex * _buttonWidth; }}


        public int Capacity { get { return StorageContainer.Capacity; }  }
        protected int DrawEndIndex { get; set; }

        public InventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position) :
           base(interfaceSection, graphicsDevice, content, position)
        {
           

        }

        public override void Load()
        {
            base.Load();

        }

        public virtual void LoadNewEntityInventory(StorageContainer storageContainer)
        {
            StorageContainer = storageContainer;
            DrawEndIndex = StorageContainer.Capacity;
            Rows = (int)Math.Floor((float)Capacity / (float)DrawEndIndex);
            Columns = DrawEndIndex;
            GenerateUI();
            SelectedSlot = InventorySlots[0];
        }


        public void SelectSlot(InventorySlot slotToSelect)
        {
            SelectedSlot = slotToSelect;
        }
        protected virtual void GenerateUI()
        {
            InventorySlots = new List<InventorySlot>();

            for (int i = 0; i < StorageContainer.Capacity; i++)
            {
                InventorySlots.Add(new InventorySlot(this, graphics, content, StorageContainer.Slots[i],
                    new Vector2(Position.X + i * _buttonWidth,
                    Position.Y + i % Rows * _buttonWidth)));
            }
            ChildSections.AddRange(InventorySlots);
        }
        public override void Update(GameTime gameTime)
        {
           // base.Update(gameTime);
            for (int i = 0; i < DrawEndIndex; i++)
            {
                InventorySlots[i].Update(gameTime);
            }


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for(int i =0; i < DrawEndIndex; i++)
            {
                InventorySlots[i].Draw(spriteBatch);
            }
        }

        //internal protected int RemoveItem(Item item)
        //{
        //    return Inventory.remo
        //}

    

    }
}

