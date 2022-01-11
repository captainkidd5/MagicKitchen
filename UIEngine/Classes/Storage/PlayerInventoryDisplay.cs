using EntityEngine.Classes;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
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

        public override void LoadNewEntityInventory(Entity entity)
        {

            StorageContainer = entity.StorageContainer;

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
                UserInterface.ButtonTexture, null, layer: Settings.Layers.foreground);
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
            for(int b = 0; b < StorageContainer.Capacity; b++)
            {
                if(b < _extendedInventoryCutoff)
                {
                    slotPos = new Vector2(Position.X + b * _buttonWidth, Position.Y);
                }
                else
                {
                    int newIndex = b - _extendedInventoryCutoff;
                    slotPos = new Vector2(Position.X + newIndex * _buttonWidth, (Position.Y - ((Rows - 1) * _buttonWidth)) + newIndex % Rows);

                }

                InventorySlots.Add(new InventorySlot(this, graphics, content, StorageContainer.Slots[b], slotPos));
            }


            //for (int i = 0; i < _extendedInventoryCutoff; i++)
            //{
            //    InventorySlots.Add(new InventorySlot(this, graphics, content, StorageContainer.Slots[i],
            //        new Vector2(Position.X + i * _buttonWidth,
            //        Position.Y + i % Rows * _buttonWidth)));
            //}
            //int z = 0;
            //Vector2 extendedInvPosition = new Vector2(Position.X, Position.Y - _buttonWidth);
            //for(int j = _extendedInventoryCutoff - 1; j <StorageContainer.Slots.Count; j++)
            //{
            //    InventorySlots.Add(new InventorySlot(this, graphics, content, StorageContainer.Slots[j],
            //        new Vector2(extendedInvPosition.X + z * _buttonWidth,
            //        extendedInvPosition.Y + z % Rows * _buttonWidth)));
            //    z++;
            //}
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
