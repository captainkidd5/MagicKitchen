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

namespace UIEngine.Classes.Storage
{
    /// <summary>
    /// Graphical and interactable wrapper of storage container
    /// </summary>
    internal class Inventory : InterfaceSection
    {
        internal Sprite SelectorSprite { get; set; }
        private StorageContainer StorageContainer { get; set; }

        private List<InventorySlot> InventorySlots { get; set; }

        internal InventorySlot SelectedSlot { get; set; }

        private int Rows { get; set; }
        private int Columns { get; set; }

        public int Capacity { get { return StorageContainer.Capacity; } set { StorageContainer.ChangeCapacity(value); } }

        public Inventory(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content,  Vector2? position, StorageContainer? storageContainer) :
           base(interfaceSection, graphicsDevice, content, position)
        {
            StorageContainer = storageContainer ?? new StorageContainer(10);

            Rows = Capacity <= 10 ? 1 : 2;
            Columns = 10;
            GenerateUI();
            SelectedSlot = InventorySlots[0];
            SelectorSprite = SpriteFactory.CreateUISprite(SelectedSlot.Position, new Rectangle(272, 0, 64, 64),
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
            if(Controls.ScrollWheelIncreased)
            {
                SelectedSlot = InventorySlots[ScrollHelper.GetIndexFromScroll(
                    Settings.Direction.Up, InventorySlots.IndexOf(SelectedSlot),
                    InventorySlots.Count)];
            }
            else if (Controls.ScrollWheelDecreased)
            {
                SelectedSlot = InventorySlots[ScrollHelper.GetIndexFromScroll(
                    Settings.Direction.Down, InventorySlots.IndexOf(SelectedSlot),
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
                    Position.Y + i % Rows * 64)));
            }
            ChildSections.AddRange(InventorySlots);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateSelectorIndex();
            SelectorSprite.Update(gameTime, SelectedSlot.Position);
           

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (InventorySlot slot in InventorySlots)
            {
                slot.Draw(spriteBatch);

            }
            SelectorSprite.Draw(spriteBatch);
        }

        //internal protected int RemoveItem(Item item)
        //{
        //    return Inventory.remo
        //}

        internal class InventorySlot : InterfaceSection
        {
            private readonly StorageSlot storageSlot;
            private Button Button { get; set; }

            private Text Text { get; set; }

            public new bool Clicked { get => Button.Clicked; }
            internal protected new bool Hovered { get => Button.Hovered; }

            public InventorySlot(InterfaceSection interfaceSection,GraphicsDevice graphicsDevice, ContentManager content
                 , StorageSlot storageSlot, Vector2 position)
                : base(interfaceSection, graphicsDevice, content,position)
            {
                this.storageSlot = storageSlot;
                storageSlot.ItemChanged += ItemChanged;
                Button = new Button(interfaceSection, graphicsDevice, content, position, null, null, hoverTransparency: true);
                Text = TextFactory.CreateUIText("0");
            }


            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);
                Button.Update(gameTime);

                if (Clicked)
                {
                    (parentSection as Inventory).SelectSlot(this);
                    
                    storageSlot.ClickInteraction(Controls.HeldItem, Controls.PickUp, Controls.DropToSlot);

                }

            }

            public Item RemoveOne()
            {
                return storageSlot.RemoveSingle();
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                base.Draw(spriteBatch);

                if (storageSlot.StoredCount > 0)
                    Text.Draw(spriteBatch, Position, true);
           
                Button.Draw(spriteBatch);
            }

            private void ItemChanged(int id)
            {
                if(id == (int)ItemType.None)
                {
                    Button.SwapForeGroundSprite(null);
                    Text.SetFullString(string.Empty);
                }
                else
                {
                    Button.SwapForeGroundSprite(SpriteFactory.CreateUISprite(Position, 
                    Item.GetItemSourceRectangle(id), ItemFactory.ItemSpriteSheet, Color.White, Vector2.Zero, 4f));
                    Text.SetFullString(storageSlot.StoredCount.ToString());

                }
            }
        }

    }
}

