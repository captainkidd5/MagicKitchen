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
    internal class Inventory : InterfaceSection
    {
        private Sprite _selectorSprite;
        private int _rows;
        private int _columns;
        private StorageContainer StorageContainer { get; set; }

        private List<InventorySlot> InventorySlots { get; set; }

        internal InventorySlot SelectedSlot { get; set; }



        public int Capacity { get { return StorageContainer.Capacity; } set { StorageContainer.ChangeCapacity(value); } }

        public Inventory(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, StorageContainer? storageContainer) :
           base(interfaceSection, graphicsDevice, content, position)
        {
            StorageContainer = storageContainer ?? new StorageContainer(10);

            _rows = Capacity <= 10 ? 1 : 2;
            _columns = 10;
            GenerateUI();
            SelectedSlot = InventorySlots[0];
            _selectorSprite = SpriteFactory.CreateUISprite(SelectedSlot.Position, new Rectangle(272, 0, 64, 64),
                UserInterface.ButtonTexture, null, layer: Settings.Layers.foreground);
        }

        public override void Load()
        {
            base.Load();
        }

        /// <summary>
        /// Changes selected slot based on the controls scroll wheel behavior
        /// </summary>
        private void UpdateSelectorIndex()
        {
            if (Controls.ScrollWheelIncreased)
            {
                SelectedSlot = InventorySlots[ScrollHelper.GetIndexFromScroll(
                    Direction.Up, InventorySlots.IndexOf(SelectedSlot),
                    InventorySlots.Count)];
            }
            else if (Controls.ScrollWheelDecreased)
            {
                SelectedSlot = InventorySlots[ScrollHelper.GetIndexFromScroll(
                    Direction.Down, InventorySlots.IndexOf(SelectedSlot),
                    InventorySlots.Count)];
            }
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
            UpdateSelectorIndex();
            _selectorSprite.Update(gameTime, SelectedSlot.Position);


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (InventorySlot slot in InventorySlots)
            {
                slot.Draw(spriteBatch);

            }
            _selectorSprite.Draw(spriteBatch);
        }

        //internal protected int RemoveItem(Item item)
        //{
        //    return Inventory.remo
        //}

    

    }
}

