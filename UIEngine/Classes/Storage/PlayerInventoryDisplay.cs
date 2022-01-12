using Globals.Classes;
using Globals.Classes.Helpers;
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
        public PlayerInventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position) : base(interfaceSection, graphicsDevice, content, position)
        {
            
        }

        public override void LoadNewEntityInventory(StorageContainer storageContainer)
        {

            StorageContainer = storageContainer;

            if (StorageContainer.Capacity % _extendedInventoryCutoff != 0)
                throw new Exception($"Inventory must form a full number of rows {StorageContainer.Capacity} / {_extendedInventoryCutoff} does not have remainder of zero");
            DrawEndIndex = _extendedInventoryCutoff;
            Rows = (int)Math.Floor((float)Capacity / (float)DrawEndIndex);
            Columns = DrawEndIndex;
            GenerateUI();
            SelectedSlot = InventorySlots[0];
        }
        public override void Load()
        {
            base.Load();
            _selectorSprite = SpriteFactory.CreateUISprite(SelectedSlot.Position, new Rectangle(272, 0, 64, 64),
                UI.ButtonTexture, null, layer: Settings.Layers.foreground);
            DrawEndIndex = _extendedInventoryCutoff;
            _openBigInventoryButton = new Button(this, graphics, content, new Vector2(Position.X + Width, Position.Y), _openBigInventoryUpArrowSourceRectangle, null, null,null, new Action(ToggleOpen));
            _openBigInventoryButton.Load();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            UpdateSelectorIndex();
            _selectorSprite.Update(gameTime, SelectedSlot.Position);
            _openBigInventoryButton.Update(gameTime);
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
        protected override void GenerateUI()
        {
            InventorySlots = new List<InventorySlot>();
            Vector2 slotPos = Vector2.Zero;
            for(int i = 0; i < StorageContainer.Capacity; i++)
            {
                //Always visible row
                if(i < _extendedInventoryCutoff)
                {
                    slotPos = new Vector2(Position.X + i * _buttonWidth, Position.Y);
                }
                else
                {
                    int newIndex = i - _extendedInventoryCutoff;
                    int row = (int)Math.Floor((float)newIndex / (float)_extendedInventoryCutoff);
                    int column = newIndex % _extendedInventoryCutoff;
                    slotPos = new Vector2(Position.X + ((column * _buttonWidth)), ((Position.Y - ((Rows - 1) * _buttonWidth) + _buttonWidth * row)));

                }

                InventorySlots.Add(new InventorySlot(this, graphics, content, StorageContainer.Slots[i], slotPos));
            }

            ChildSections.AddRange(InventorySlots);

        }
        /// <summary>
        /// Button action for arrow sprite, swaps between two sprites and opens/closes extended inventory
        /// </summary>
        private void ToggleOpen()
        {
            _isOpen = !_isOpen;
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
