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
using DataModels.MapStuff;

namespace UIEngine.Classes.Storage
{
    /// <summary>
    /// Graphical and interactable wrapper of storage container
    /// </summary>
    internal class InventoryDisplay : MenuSection
    {
        public Item CurrentlySelectedItem => SelectedSlot.Item;

        protected static readonly int _buttonWidth = 64;
        protected int Rows;
        protected int Columns;

        protected WalletDisplay WalletDisplay { get; set; }
        protected StorageContainer StorageContainer { get; set; }

        protected InventorySlotDisplay[,] InventorySlots { get; set; }


       
        internal InventorySlotDisplay SelectedSlot { get; set; }

        internal new int Width { get { return XDrawEndIndex * _buttonWidth; } }


        public int Capacity { get { return StorageContainer.Capacity; } }
        protected int XDrawEndIndex { get; set; }

        protected int YDrawEndIndex { get; set; }

        protected StackPanel StackPanel { get; set; }

        public bool HasControl { get; protected set; }
        public bool ExtendedInventoryOpen
        {
            get;
            protected set;
        }
        protected bool WasExtendedOpenLastFrame { get; set; }

        public bool WasExtendedJustOpened => ExtendedInventoryOpen && !WasExtendedOpenLastFrame;

        protected int DrawCutOff { get; set; }

        private Sprite _selectorSprite;

        protected Rectangle BackgroundSourceRectangle { get; set; }

        protected Sprite BackdropSprite { get; set; }
        protected Vector2 BackgroundSpritePositionOffset { get; set; }

        public InventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
           base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

            NormallyActivated = false;
        }

        public void UpdateSelectorSprite(GameTime gameTime)
        {
            _selectorSprite.Update(gameTime, SelectedSlot.Position);

        }

        public void DrawSelectorSprite(SpriteBatch spriteBatch)
        {
            _selectorSprite.Draw(spriteBatch);

        }

        public override void Deactivate()
        {
            base.Deactivate();
            ExtendedInventoryOpen = false;

            //Should drop the item if item is grabbed and player closes the inventory
            if (Controls.ControllerConnected && UI.Cursor.IsHoldingItem)
                UI.Cursor.OnItemDropped();

        }

        public override void LoadContent()
        {
            //Overridden because total bounds not set, TODO ?
            //base.LoadContent();
        }
        public virtual void LoadNewEntityInventory(StorageContainer storageContainer, bool displayWallet)
        {

            StorageContainer = storageContainer;
            //Furniture data was passed in, use the required configuration from that
            if (StorageContainer.FurnitureData != null)
            {
                FurnitureData furnitureData = StorageContainer.FurnitureData;
                if (furnitureData.FurnitureType != FurnitureType.None)
                {
                    Rows = furnitureData.StorageRows;
                    Columns = furnitureData.StorageColumns;
                }
                else
                {
                    //TODO: Define custom types
                }
            }
            else //Else use standard rows and columns from this class
            {

                Rows = 3;
                Columns = 8;
            }
            DrawCutOff = Rows;
            GenerateUI(displayWallet);
            SelectedSlot = InventorySlots[RestingIndex.X, RestingIndex.Y];
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
            StackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            ClearGrid();
            InventorySlots = new InventorySlotDisplay[Rows, Columns];
            int slotIndex = 0;
            for (int i = 0; i < Rows; i++)
            {
                StackRow stackRow = new StackRow(Columns * _buttonWidth);
                for (int j = 0; j < Columns; j++)
                {
                    InventorySlotDisplay display = new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[slotIndex],
                    Position, GetLayeringDepth(UILayeringDepths.Low));
                    InventorySlots[i, j] = display;
                    AddSectionToGrid(display, i, j);
                    display.LoadContent();

                    stackRow.AddItem(display, StackOrientation.Left);
                    slotIndex++;
                }
                StackPanel.Add(stackRow);
            }

            if (displayWallet)
            {
                Vector2 walletDisplayPosition = new Vector2(0, 0);
                if (InventorySlots.GetLength(0) > 0)
                    walletDisplayPosition = InventorySlots[0, InventorySlots.GetLength(1)].Position;
                WalletDisplay = new WalletDisplay(this, graphics, content, walletDisplayPosition, GetLayeringDepth(UILayeringDepths.Low));
            }

            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Rows * _buttonWidth, Columns * _buttonWidth);
        }

        public virtual void GiveControl()
        {
            HasControl = true;
            SelectSlotAndMoveCursorIcon(Controls.ControllerConnected);
            if (Controls.ControllerConnected)
                Controls.ControllerSetUIMousePosition(InventorySlots[CurrentSelectedPoint.X, CurrentSelectedPoint.Y].Position);
        }
        public virtual void RemoveControl() => HasControl = false;

        public virtual void CloseExtendedInventory() => ExtendedInventoryOpen = false;
        public virtual void OpenExtendedInventory() {
            DrawCutOff = Rows;
                ExtendedInventoryOpen = true;
        } 

        public override void Update(GameTime gameTime)
        {
            Hovered = false;
            if (IsActive)
            {

                CheckOveriddenLogic(gameTime);

                if (ExtendedInventoryOpen && HasControl)
                    SelectedSlot.IsSelected = true;

                if (HasControl && Controls.ControllerConnected)
                    CheckButtonTaps();

                for (int i = 0; i < DrawCutOff; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        InventorySlotDisplay slot = InventorySlots[i, j];
                        if (slot != null)
                        {
                            slot.Update(gameTime);
                            if (slot.Hovered)
                                Hovered = true;
                        }

                    }

                }
                WalletDisplay?.Update(gameTime);
                WasExtendedOpenLastFrame = ExtendedInventoryOpen;
                UpdateSelectorIndex();
                if (BackdropSprite != null)
                    BackdropSprite.Update(gameTime, Position + BackgroundSpritePositionOffset);
            }
            CheckFramesActive();

        }
        protected override void SelectNext(Direction direction)
        {
            base.SelectNext(direction);
            SelectSlotAndMoveCursorIcon(Controls.ControllerConnected);
        }

        /// <summary>
        /// Move cursor should be used with game pad input only
        /// </summary>
        /// <param name="moveCursor"></param>
        protected void SelectSlotAndMoveCursorIcon(bool moveCursor = true)
        {
            SelectSlot(InventorySlots[CurrentSelectedPoint.X, CurrentSelectedPoint.Y]);
            if (moveCursor)
                Controls.ControllerSetUIMousePosition(InventorySlots[CurrentSelectedPoint.X, CurrentSelectedPoint.Y].Position);
        }

        /// <summary>
        /// Changes selected slot based on the controls scroll wheel behavior
        /// </summary>
        private void UpdateSelectorIndex()
        {
            Direction newDir = Direction.None;

            if (Controls.ScrollWheelIncreased)
                newDir = Direction.Left;
            else if (Controls.ScrollWheelDecreased)
                newDir = Direction.Right;
            else
                return;
            SelectNext(newDir);

        }

        protected override void DoSelection(Point newIndex)
        {
            if (!ExtendedInventoryOpen && newIndex.X >= DrawCutOff)
                return;
            base.DoSelection(newIndex);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                for (int i = 0; i < DrawCutOff; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (InventorySlots[i, j] != null)
                        {
                            InventorySlots[i, j].Draw(spriteBatch);
                        }
                    }
                }

                WalletDisplay?.Draw(spriteBatch);
                if (BackdropSprite != null)
                    BackdropSprite.Draw(spriteBatch);
            }

        }

    }
}

