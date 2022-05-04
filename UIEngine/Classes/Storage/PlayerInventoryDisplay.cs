using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.ButtonStuff;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace UIEngine.Classes.Storage
{
    internal class PlayerInventoryDisplay : InventoryDisplay
    {
        private Sprite _selectorSprite;
        private Button _openBigInventoryButton;
        private Rectangle _openBigInventoryUpArrowSourceRectangle = new Rectangle(112, 16, 16, 32);
        private Rectangle _closeBigInventoryUpArrowSourceRectangle = new Rectangle(128, 16, 16, 32);

        private int _extendedInventoryCutoff = 8;


        private bool _isOpen;
        private bool _oldOpen;
        public PlayerInventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = true;
        }

        public override void LoadNewEntityInventory(StorageContainer storageContainer, bool displayWallet)
        {

            StorageContainer = storageContainer;

            if (StorageContainer.Capacity % _extendedInventoryCutoff != 0)
                throw new Exception($"Inventory must form a full number of rows {StorageContainer.Capacity} / {_extendedInventoryCutoff} does not have remainder of zero");
            DrawEndIndex = _extendedInventoryCutoff;
            Rows = (int)Math.Floor((float)Capacity / (float)DrawEndIndex);
            Columns = DrawEndIndex;
            GenerateUI(displayWallet);
            SelectedSlot = InventorySlots[0];
        }
        public override void LoadContent()
        {
            base.LoadContent();
            _selectorSprite = SpriteFactory.CreateUISprite(SelectedSlot.Position, new Rectangle(272, 0, 64, 64),
                UI.ButtonTexture, GetLayeringDepth(UILayeringDepths.Medium),null);
            DrawEndIndex = _extendedInventoryCutoff;
            _openBigInventoryButton = UI.ButtonFactory.CreateButton(this,
                new Vector2(Position.X + Width, Position.Y),LayerDepth,
                _openBigInventoryUpArrowSourceRectangle, new Action(ToggleOpen), scale:2f);
            _openBigInventoryButton.LoadContent();

            TotalBounds = new Rectangle(TotalBounds.X, TotalBounds.Y, TotalBounds.Width + _openBigInventoryButton.TotalBounds.Width, TotalBounds.Height);


            WalletDisplay = new WalletDisplay(this, graphics, content,
                new Vector2(_openBigInventoryButton.Position.X + _openBigInventoryButton.Width * 4,
                _openBigInventoryButton.Position.Y), GetLayeringDepth(UILayeringDepths.Medium));


        }
        public override void Update(GameTime gameTime)
        {
            _oldOpen = _isOpen;
            base.Update(gameTime);


            if (_isOpen)
                Flags.Pause = true;
            UpdateSelectorIndex();
            _selectorSprite.Update(gameTime, SelectedSlot.Position);
            _openBigInventoryButton.Update(gameTime);

            if (_oldOpen && !_isOpen)
                Flags.Pause = false;
        }

        protected override void CheckLogic(GameTime gameTime)
        {
            base.CheckLogic(gameTime);

            if (Controls.WasGamePadButtonTapped(GamePadActionType.X))
            {
                ToggleOpen();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
             DrawEndIndex = InventorySlots.Count;
            if (!_isOpen)
                DrawEndIndex = _extendedInventoryCutoff;
            base.Draw(spriteBatch);


            _selectorSprite.Draw(spriteBatch);
            _openBigInventoryButton.Draw(spriteBatch);
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
            if (!_isOpen)
            {
                if (newSelectedSlot == Capacity - 1)
                    newSelectedSlot = _extendedInventoryCutoff - 1;
                else if (newSelectedSlot >= _extendedInventoryCutoff)
                    newSelectedSlot = 0;

                
            }
               


            SelectedSlot = InventorySlots[newSelectedSlot];
        }
        protected override void GenerateUI(bool displayWallet)
        {
            InventorySlots = new List<InventorySlotDisplay>();
            Vector2 slotPos = Vector2.Zero;
            for(int i = 0; i < StorageContainer.Capacity; i++)
            {
                //Always visible row
                if(i < _extendedInventoryCutoff)
                {
                    slotPos = new Vector2(Position.X + i * _buttonWidth, Position.Y);
                    TotalBounds = new Rectangle(TotalBounds.X, TotalBounds.Y, TotalBounds.Width + _buttonWidth, _buttonWidth);
                }
                else
                {
                    int newIndex = i - _extendedInventoryCutoff;
                    int row = (int)Math.Floor((float)newIndex / (float)_extendedInventoryCutoff);
                    int column = newIndex % _extendedInventoryCutoff;
                    slotPos = new Vector2(Position.X + ((column * _buttonWidth)), ((Position.Y - ((Rows - 1) * _buttonWidth) + _buttonWidth * row)));

                }

                InventorySlots.Add(new InventorySlotDisplay(this, graphics, content, StorageContainer.Slots[i], slotPos,GetLayeringDepth(UILayeringDepths.Low)));
            }

            //ChildSections.AddRange(InventorySlots);

        }
        /// <summary>
        /// Button action for arrow sprite, swaps between two sprites and opens/closes extended inventory
        /// </summary>
        private void ToggleOpen()
        {
            _isOpen = !_isOpen;
            //reset selector to 0 if just closed
            if(!_isOpen)
                SelectedSlot = InventorySlots[0];

            SwitchSpriteFromToggleStatus();
        }

        private void SwitchSpriteFromToggleStatus()
        {
            if (_isOpen)
                _openBigInventoryButton.SwapBackgroundSprite(_closeBigInventoryUpArrowSourceRectangle);
            else
                _openBigInventoryButton.SwapBackgroundSprite(_openBigInventoryUpArrowSourceRectangle);

        }
    }
}
