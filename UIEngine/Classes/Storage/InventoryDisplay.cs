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

        protected WalletDisplay WalletDisplay {get;set;}
        protected StorageContainer StorageContainer { get; set; }

        protected List<InventorySlotDisplay> InventorySlots { get; set; }

        internal InventorySlotDisplay SelectedSlot { get; set; }

        internal new int Width { get { return DrawEndIndex * _buttonWidth; }}


        public int Capacity { get { return StorageContainer.Capacity; }  }
        protected int DrawEndIndex { get; set; }

        public InventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
           base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
           

        }

        public override void LoadContent()
        {
            
            base.LoadContent();

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


        public void SelectSlot(InventorySlotDisplay slotToSelect)
        {
            SelectedSlot = slotToSelect;
        }
        protected virtual void GenerateUI()
        {
            InventorySlots = new List<InventorySlotDisplay>();

            for (int i = 0; i < StorageContainer.Capacity; i++)
            {
                InventorySlots.Add(new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[i],
                    new Vector2(Position.X + i * _buttonWidth,
                    Position.Y + i % Rows * _buttonWidth), LayerDepth));
            }
            Vector2 walletDisplayPosition = new Vector2(0, 0);
            if(InventorySlots.Count > 0)
                walletDisplayPosition = InventorySlots[InventorySlots.Count - 1].Position;
            WalletDisplay = new WalletDisplay(this, graphics, content, walletDisplayPosition, GetLayeringDepth(UILayeringDepths.Low));
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);
        }
        public override void Update(GameTime gameTime)
        {
            Hovered = false;

            // base.Update(gameTime);
            for (int i = 0; i < DrawEndIndex; i++)
            {
                InventorySlotDisplay slot = InventorySlots[i];
                slot.Update(gameTime);
                if (slot.Hovered)
                    Hovered = true;
            }
            WalletDisplay.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for(int i =0; i < DrawEndIndex; i++)
            {
                InventorySlots[i].Draw(spriteBatch);
            }
            WalletDisplay.Draw(spriteBatch);
        }

    }
}

