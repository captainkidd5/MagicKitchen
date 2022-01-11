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
using EntityEngine.Classes;

namespace UIEngine.Classes.Storage
{
    /// <summary>
    /// Graphical and interactable wrapper of storage container
    /// </summary>
    internal class InventoryDisplay : InterfaceSection
    {
        private int _rows;
        private int _columns;
        protected StorageContainer StorageContainer { get; set; }

        protected List<InventorySlot> InventorySlots { get; set; }

        internal InventorySlot SelectedSlot { get; set; }



        public int Capacity { get { return StorageContainer.Capacity; } set { StorageContainer.ChangeCapacity(value); } }

        public InventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position) :
           base(interfaceSection, graphicsDevice, content, position)
        {
           

        }

        public override void Load()
        {
            base.Load();

        }

        public void LoadNewEntityInventory(Entity? entity)
        {
            StorageContainer = entity.StorageContainer;
            _rows = Capacity <= 10 ? 1 : 2;
            _columns = 10;
            GenerateUI();
            SelectedSlot = InventorySlots[0];
        }


        public void SelectSlot(InventorySlot slotToSelect)
        {
            SelectedSlot = slotToSelect;
        }
        private void GenerateUI()
        {
            InventorySlots = new List<InventorySlot>();

            for (int i = 0; i < StorageContainer.Slots.Count; i++)
            {
                InventorySlots.Add(new InventorySlot(this, graphics, content, StorageContainer.Slots[i],
                    new Vector2(Position.X + i * 64,
                    Position.Y + i % _rows * 64)));
            }
            ChildSections.AddRange(InventorySlots);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (InventorySlot slot in InventorySlots)
            {
                slot.Draw(spriteBatch);

            }
        }

        //internal protected int RemoveItem(Item item)
        //{
        //    return Inventory.remo
        //}

    

    }
}

