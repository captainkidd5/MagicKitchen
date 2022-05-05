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

        protected WalletDisplay WalletDisplay { get; set; }
        protected StorageContainer StorageContainer { get; set; }

        protected List<InventorySlotDisplay> InventorySlots { get; set; }

        protected int CurrentSelectedIndex { get; set; }
        internal InventorySlotDisplay SelectedSlot { get; set; }

        internal new int Width { get { return DrawEndIndex * _buttonWidth; } }


        public int Capacity { get { return StorageContainer.Capacity; } }
        protected int DrawEndIndex { get; set; }

        private StackPanel _stackPanel;

        public bool HasControl { get; protected set; }
        public bool ExtendedInventoryOpen { get; 
            protected set; }
        protected bool WasExtendedOpenLastFrame { get; set; }

        public bool WasExtendedJustOpened => ExtendedInventoryOpen && !WasExtendedOpenLastFrame;

        protected int ExtendedInventoryCutOff { get; set; }

        private Sprite _selectorSprite;

        public InventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
           base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

            NormallyActivated = false;
        }

        public override void LoadContent()
        {

            //base.LoadContent();



        }
        public void UpdateSelectorSprite(GameTime gameTime)
        {
            _selectorSprite.Update(gameTime, SelectedSlot.Position);

        }

        public void DrawSelectorSprite(SpriteBatch spriteBatch)
        {
            _selectorSprite.Draw(spriteBatch);

        }
        public override void Activate()
        {
            base.Activate();
        }
        public override void Deactivate()
        {
            base.Deactivate();
            ExtendedInventoryOpen = false;
            if (Controls.ControllerConnected && UI.Cursor.IsHoldingItem)
            {
                //Should drop the item if item is grabbed and player closes the inventory

                UI.Cursor.OnItemDropped();

            }
        }
        public virtual void LoadNewEntityInventory(StorageContainer storageContainer, bool displayWallet)
        {

            StorageContainer = storageContainer;
            DrawEndIndex = StorageContainer.Capacity;
            Rows = (int)Math.Floor((float)Capacity / (float)DrawEndIndex);
            Columns = DrawEndIndex;
            GenerateUI(displayWallet);
            SelectedSlot = InventorySlots[0];
            ExtendedInventoryCutOff = InventorySlots.Count;
            LoadSelectorSprite();
        }

        protected void LoadSelectorSprite()
        {
            _selectorSprite = SpriteFactory.CreateUISprite(SelectedSlot.Position, new Rectangle(272, 0, 64, 64),
               UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.High), null, randomizeLayers: false);
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
            for (int i = 0; i < Rows; i++)
            {
                StackRow stackRow = new StackRow(Columns * _buttonWidth);
                for (int j = 0; j < Columns; j++)
                {
                    InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[slotIndex],
                    Position, GetLayeringDepth(UILayeringDepths.Low));
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

        public virtual void GiveControl()
        {
            HasControl = true;
           // CurrentSelectedIndex = 0;
            //SelectSlot(InventorySlots[CurrentSelectedIndex]);
            if(Controls.ControllerConnected)
            Controls.ControllerSetUIMousePosition(InventorySlots[CurrentSelectedIndex].Position);
        }
        public virtual void RemoveControl()
        {
            HasControl = false;
        }

        public virtual void CloseExtendedInventory()
        {
            ExtendedInventoryOpen = false;

        }
        public virtual void OpenExtendedInventory()
        {
            ExtendedInventoryOpen = true;
        }
        public override void Update(GameTime gameTime)
        {
            Hovered = false;
            // base.Update(gameTime);
            if (IsActive)
            {
                CheckLogic(gameTime);

                if (ExtendedInventoryOpen && HasControl)
                    SelectedSlot.IsSelected = true;
                if (HasControl)

                {


                    if (Controls.WasGamePadButtonTapped(GamePadActionType.BumperLeft) ||
                        Controls.WasGamePadButtonTapped(GamePadActionType.DPadLeft))
                    {
                        int newIndex = CurrentSelectedIndex;

                        newIndex = ScrollHelper.GetIndexFromScroll(Direction.Up, CurrentSelectedIndex, DrawEndIndex);
                        if ((newIndex +1) % (Columns ) != 0 && newIndex + 1 < InventorySlots.Count )
                            CurrentSelectedIndex = newIndex;
                        SelectSlotAndMoveCursorIcon();
                    }
                    else if (Controls.WasGamePadButtonTapped(GamePadActionType.BumperRight) ||
                        Controls.WasGamePadButtonTapped(GamePadActionType.DPadRight))
                    {
                        int newIndex = CurrentSelectedIndex;
                        newIndex = ScrollHelper.GetIndexFromScroll(Direction.Down, CurrentSelectedIndex, DrawEndIndex);
                        //prevent wrapping
                        if(newIndex % Columns != 0 )
                            CurrentSelectedIndex = newIndex;

                        SelectSlotAndMoveCursorIcon();

                    }
                    else if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadUp))
                    {
                        CurrentSelectedIndex +=  Columns;
                        if(CurrentSelectedIndex >= InventorySlots.Count )
                            CurrentSelectedIndex = CurrentSelectedIndex - InventorySlots.Count;
                        SelectSlotAndMoveCursorIcon();

                    }
                    else if (Controls.WasGamePadButtonTapped(GamePadActionType.DPadDown))
                    {
                        CurrentSelectedIndex -= Columns;
                        if (CurrentSelectedIndex < 0)
                            CurrentSelectedIndex = InventorySlots.Count + CurrentSelectedIndex;

                        SelectSlotAndMoveCursorIcon();

                    }
                }
                for (int i = 0; i < DrawEndIndex; i++)
                {
                    InventorySlotDisplay slot = InventorySlots[i];
                    slot.Update(gameTime);
                    if (slot.Hovered)
                        Hovered = true;
                }
                WalletDisplay?.Update(gameTime);
                WasExtendedOpenLastFrame = ExtendedInventoryOpen;

            }
            CheckFramesActive();
            UpdateSelectorIndex();

        }

        private void SelectSlotAndMoveCursorIcon()
        {
            SelectSlot(InventorySlots[CurrentSelectedIndex]);
            Controls.ControllerSetUIMousePosition(InventorySlots[CurrentSelectedIndex].Position);
        }

        /// <summary>
        /// Changes selected slot based on the controls scroll wheel behavior
        /// </summary>
        private void UpdateSelectorIndex()
        {
            int newSelectedSlot = 0;
            Direction newDir = Direction.None;

            if (Controls.ScrollWheelIncreased)
                newDir = Direction.Down;
            else if (Controls.ScrollWheelDecreased)
                newDir = Direction.Up;
            else
                return;

            newSelectedSlot = ScrollHelper.GetIndexFromScroll(
                    newDir, InventorySlots.IndexOf(SelectedSlot),
                    InventorySlots.Count);

            //Selector shouldn't extend past main toolbar row if extended inventory is closed
            if (!ExtendedInventoryOpen)
            {
                if (newSelectedSlot == Capacity - 1)
                    newSelectedSlot = ExtendedInventoryCutOff - 1;
                else if (newSelectedSlot >= ExtendedInventoryCutOff)
                    newSelectedSlot = 0;


            }



            SelectedSlot = InventorySlots[newSelectedSlot];
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

