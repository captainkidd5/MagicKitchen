﻿using Globals.Classes;
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
            Rows = 3;
            Columns = 8;
            Selectables = new InterfaceSection[Rows, Columns];

            GenerateUI(displayWallet);
            
            SelectedSlot = InventorySlots[0,0];
            LoadSelectorSprite();
        }
        public override void LoadContent()
        {
            base.LoadContent();
            //DrawEndIndex = ExtendedInventoryCutOff;
            _openBigInventoryButton = UI.ButtonFactory.CreateButton(this,
                new Vector2(Position.X + TotalBounds.Width, Position.Y),LayerDepth,
                _openBigInventoryUpArrowSourceRectangle, new Action(ToggleOpen), scale:2f);
            _openBigInventoryButton.LoadContent();

            TotalBounds = new Rectangle(TotalBounds.X, TotalBounds.Y, TotalBounds.Width +
                _openBigInventoryButton.TotalBounds.Width, TotalBounds.Height);


            WalletDisplay = new WalletDisplay(this, graphics, content,
                new Vector2(Position.X + TotalBounds.Width ,
                _openBigInventoryButton.Position.Y), GetLayeringDepth(UILayeringDepths.Medium));


        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (ExtendedInventoryOpen)
                Flags.Pause = true;
            _openBigInventoryButton.Update(gameTime);

            if (WasExtendedOpenLastFrame && !ExtendedInventoryOpen)
                Flags.Pause = false;
        }

        protected override void CheckOveriddenLogic(GameTime gameTime)
        {
            base.CheckOveriddenLogic(gameTime);

            if (Controls.WasGamePadButtonTapped(GamePadActionType.Y) || Controls.WasKeyTapped(Keys.Tab))
            {
                ToggleOpen();
            }
        }
        public override void Activate()
        {
            base.Activate();
            GiveControl();
            CloseExtendedInventory();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // DrawEndIndex = InventorySlots.Count;
            //if (!ExtendedInventoryOpen)
            //    DrawEndIndex = ExtendedInventoryCutOff;
            base.Draw(spriteBatch);


            _openBigInventoryButton.Draw(spriteBatch);
        }

       
        protected override void GenerateUI(bool displayWallet)
        {
            InventorySlots = new InventorySlotDisplay[Rows, Columns];
            Vector2 slotPos = Vector2.Zero;
            Vector2 slotoffSet = new Vector2(0, -1 * (_buttonWidth * (Rows -1)));
            int containerSlotIndex = 0;
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    int yRowOffset = (i - 1) * _buttonWidth;
                    //Always visible row
                    if (i ==0)
                    {
                        yRowOffset = (Rows - 1) * _buttonWidth;

                    }


                    slotPos = new Vector2(Position.X + ((j * _buttonWidth)), Position.Y + slotoffSet.Y + yRowOffset);



                        InventorySlotDisplay slotDisplay = new InventorySlotDisplay(this, graphics, content,
                        StorageContainer.Slots[containerSlotIndex], slotPos,
                        GetLayeringDepth(UILayeringDepths.Low));
                    containerSlotIndex++;

                    ChildSections.Add(slotDisplay);
                    AddSectionToGrid(slotDisplay, i, j);
                    InventorySlots[i, j] = slotDisplay;


                }

            }
            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, Columns * _buttonWidth, Rows * _buttonWidth);


        }
        public override void CloseExtendedInventory()
        {
            base.CloseExtendedInventory();
            //Don't want to reset the index back to zero if current selection isn't in the extended inventory, that 
            //would just be annoying
            //if(CurrentSelectedIndex > ExtendedInventoryCutOff)
            //{
            //    CurrentSelectedIndex = 0;

            //    SelectedSlot = InventorySlots[CurrentSelectedIndex];
            //}
                
        }
        /// <summary>
        /// Button action for arrow sprite, swaps between two sprites and opens/closes extended inventory
        /// </summary>
        private void ToggleOpen()
        {
            ExtendedInventoryOpen = !ExtendedInventoryOpen;
            //reset selector to 0 if just closed
            if(!ExtendedInventoryOpen)
            {
                SelectedSlot = InventorySlots[0,0];
                if(Controls.ControllerConnected && UI.Cursor.IsHoldingItem)
                {
                    //Should drop the item if item is grabbed and player closes the inventory
            

                    UI.Cursor.OnItemDropped();

                }
            }

            SwitchSpriteFromToggleStatus();
        }

        private void SwitchSpriteFromToggleStatus()
        {
            if (ExtendedInventoryOpen)
                _openBigInventoryButton.SwapBackgroundSprite(_closeBigInventoryUpArrowSourceRectangle);
            else
                _openBigInventoryButton.SwapBackgroundSprite(_openBigInventoryUpArrowSourceRectangle);

        }
    }
}
