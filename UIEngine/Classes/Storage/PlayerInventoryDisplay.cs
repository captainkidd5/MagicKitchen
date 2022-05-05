using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Button _openBigInventoryButton;
        private Rectangle _openBigInventoryUpArrowSourceRectangle = new Rectangle(112, 16, 16, 32);
        private Rectangle _closeBigInventoryUpArrowSourceRectangle = new Rectangle(128, 16, 16, 32);


        public PlayerInventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) : 
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = true;
        }

        public override void LoadNewEntityInventory(StorageContainer storageContainer, bool displayWallet)
        {

            StorageContainer = storageContainer;
            ExtendedInventoryCutOff = 8;
            if (StorageContainer.Capacity % ExtendedInventoryCutOff != 0)
                throw new Exception($"Inventory must form a full number of rows {StorageContainer.Capacity} / {ExtendedInventoryCutOff} does not have remainder of zero");
            DrawEndIndex = ExtendedInventoryCutOff;
            Rows = (int)Math.Floor((float)Capacity / (float)DrawEndIndex);
            Columns = DrawEndIndex;
            GenerateUI(displayWallet);
            SelectedSlot = InventorySlots[0];
            LoadSelectorSprite();
        }
        public override void LoadContent()
        {
            base.LoadContent();
            DrawEndIndex = ExtendedInventoryCutOff;
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
            base.Update(gameTime);


            if (IsOpen)
                Flags.Pause = true;
            _openBigInventoryButton.Update(gameTime);

            if (WasOpenLastFrame && !IsOpen)
                Flags.Pause = false;
        }

        protected override void CheckLogic(GameTime gameTime)
        {
            base.CheckLogic(gameTime);

            if (Controls.WasGamePadButtonTapped(GamePadActionType.X) || Controls.WasKeyTapped(Keys.Tab))
            {
                ToggleOpen();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
             DrawEndIndex = InventorySlots.Count;
            if (!IsOpen)
                DrawEndIndex = ExtendedInventoryCutOff;
            base.Draw(spriteBatch);


            _openBigInventoryButton.Draw(spriteBatch);
        }

       
        protected override void GenerateUI(bool displayWallet)
        {
            InventorySlots = new List<InventorySlotDisplay>();
            Vector2 slotPos = Vector2.Zero;
            for(int i = 0; i < StorageContainer.Capacity; i++)
            {
                //Always visible row
                if(i < ExtendedInventoryCutOff)
                {
                    slotPos = new Vector2(Position.X + i * _buttonWidth, Position.Y);
                    TotalBounds = new Rectangle(TotalBounds.X, TotalBounds.Y, TotalBounds.Width + _buttonWidth, _buttonWidth);
                }
                else
                {
                    int newIndex = i - ExtendedInventoryCutOff;
                    int row = (int)Math.Floor((float)newIndex / (float)ExtendedInventoryCutOff);
                    int column = newIndex % ExtendedInventoryCutOff;
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
            IsOpen = !IsOpen;
            //reset selector to 0 if just closed
            if(!IsOpen)
            {
                SelectedSlot = InventorySlots[0];
                if(Controls.ControllerConnected && UI.Cursor.IsHoldingItem)
                {
                    //Should drop the item if item is grabbed and player closes the inventory
                    //Todo have player drop item

                    UI.Cursor.OnItemDropped();

                }
            }

            SwitchSpriteFromToggleStatus();
        }

        private void SwitchSpriteFromToggleStatus()
        {
            if (IsOpen)
                _openBigInventoryButton.SwapBackgroundSprite(_closeBigInventoryUpArrowSourceRectangle);
            else
                _openBigInventoryButton.SwapBackgroundSprite(_openBigInventoryUpArrowSourceRectangle);

        }
    }
}
