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
using UIEngine.Classes.Components;
using InputEngine.Classes;
using static DataModels.Enums;

namespace UIEngine.Classes.Storage
{
    /// <summary>
    /// Graphical and interactable wrapper of storage container
    /// </summary>
    internal class InventoryDisplay : MenuSection
    {
        protected static readonly int _buttonWidth = 64;
        protected int Rows;
        protected int Columns;

        protected WalletDisplay WalletDisplay {get;set;}
        protected StorageContainer StorageContainer { get; set; }

        protected List<InventorySlotDisplay> InventorySlots { get; set; }

        protected int CurrentSelectedIndex { get; set; }
        internal InventorySlotDisplay SelectedSlot { get; set; }

        internal new int Width { get { return DrawEndIndex * _buttonWidth; }}


        public int Capacity { get { return StorageContainer.Capacity; }  }
        protected int DrawEndIndex { get; set; }

        private StackPanel _stackPanel;

        public InventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
           base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

            NormallyActivated = false;
        }

        public override void LoadContent()
        {
            
            //base.LoadContent();

        }

        public virtual void LoadNewEntityInventory(StorageContainer storageContainer, bool displayWallet)
        {

            StorageContainer = storageContainer;
            DrawEndIndex = StorageContainer.Capacity;
            Rows = (int)Math.Floor((float)Capacity / (float)DrawEndIndex);
            Columns = DrawEndIndex;
            GenerateUI(displayWallet);
            SelectedSlot = InventorySlots[0];
        }


        public void SelectSlot(InventorySlotDisplay slotToSelect)
        {
            SelectedSlot = slotToSelect;
        }
        protected virtual void GenerateUI(bool displayWallet)
        {
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            Selectables.Clear();
            InventorySlots = new List<InventorySlotDisplay>();
            int slotIndex = 0;
            for(int i = 0; i < Rows; i++)
            {
                StackRow stackRow = new StackRow(Columns * _buttonWidth);
                for(int j =0; j < Columns; j++)
                {
                    InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[slotIndex],
                    Position, LayerDepth);
                    InventorySlots.Add(display);
                    Selectables.Add(display);
                    display.LoadContent();

                    stackRow.AddItem(display, StackOrientation.Left);
                    slotIndex++;
                }
                _stackPanel.Add(stackRow);
            }

            if (displayWallet)
            {
                Vector2 walletDisplayPosition = new Vector2(0, 0);
                if (InventorySlots.Count > 0)
                    walletDisplayPosition = InventorySlots[InventorySlots.Count - 1].Position;
                WalletDisplay = new WalletDisplay(this, graphics, content, walletDisplayPosition, GetLayeringDepth(UILayeringDepths.Low));
            }
           
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);
        }

      
        public override void Update(GameTime gameTime)
        {
            Hovered = false;
            // base.Update(gameTime);
            if (IsActive)
            {
                CheckLogic(gameTime);

                SelectedSlot.IsSelected = true;                                            

                if (Controls.WasGamePadButtonTapped(GamePadActionType.BigBumperLeft))
                {
                    CurrentSelectedIndex = ScrollHelper.GetIndexFromScroll(Direction.Up, CurrentSelectedIndex, DrawEndIndex);
                    SelectSlot(InventorySlots[CurrentSelectedIndex]);
                }
                else if(Controls.WasGamePadButtonTapped(GamePadActionType.BigBumperRight))
                {
                    CurrentSelectedIndex = ScrollHelper.GetIndexFromScroll(Direction.Down, CurrentSelectedIndex, DrawEndIndex);
                    SelectSlot(InventorySlots[CurrentSelectedIndex]);

                }
                for (int i = 0; i < DrawEndIndex; i++)
                {
                    InventorySlotDisplay slot = InventorySlots[i];
                    slot.Update(gameTime);
                    if (slot.Hovered)
                        Hovered = true;
                }
                WalletDisplay?.Update(gameTime);
            }
                CheckFramesActive();

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                for (int i = 0; i < DrawEndIndex; i++)
                {
                    InventorySlots[i].Draw(spriteBatch);
                }
                WalletDisplay?.Draw(spriteBatch);
            }
          
        }

    }
}

