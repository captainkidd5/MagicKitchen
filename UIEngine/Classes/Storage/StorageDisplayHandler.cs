﻿using DataModels.MapStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using ItemEngine.Classes.StorageStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Classes.Storage.Configurations;
using static DataModels.Enums;

namespace UIEngine.Classes.Storage
{
    internal class StorageDisplayHandler : InterfaceSection
    {
        private InventoryDisplay _secondaryInventoryDisplay;
        public Item PlayerSelectedItem => _playerInventoryDisplay.CurrentlySelectedItem;

        private PlayerInventoryDisplay _playerInventoryDisplay;
        private int _playerInventoryTotalSlots = 10;
        private int _playerSlotWidth = 64;


        private InventoryDisplay _currentlySelectedInventoryDisplay;
        public StorageDisplayHandler(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = true;
        }

        public override void LoadContent()
        {
            //base.LoadContent();
        }
        public void Load(StorageContainer playerStorageContainer)
        {
            _secondaryInventoryDisplay = new InventoryDisplay(this, graphics, content, null,
               GetLayeringDepth(UILayeringDepths.Medium));
            _secondaryInventoryDisplay.LoadContent();
            _secondaryInventoryDisplay.Deactivate();
            //X and y actually don't matter, multiply by 10 because toolbar is 10 slots wide, at 64 pixels per slot

            Rectangle totalToolBarRectangle = new Rectangle(0, 0, _playerSlotWidth * _playerInventoryTotalSlots, _playerSlotWidth);
            Vector2 playerInventoryPosition = RectangleHelper.PlaceBottomCenterScreen(totalToolBarRectangle);
            playerInventoryPosition = new Vector2(playerInventoryPosition.X, playerInventoryPosition.Y - Settings.Gutter);
            _playerInventoryDisplay = new PlayerInventoryDisplay(this, graphics, content, playerInventoryPosition, GetLayeringDepth(UILayeringDepths.Low));
            _playerInventoryDisplay.LoadNewEntityInventory(playerStorageContainer, true);
            _playerInventoryDisplay.LoadContent();
            _playerInventoryDisplay.GiveControl();
            _currentlySelectedInventoryDisplay = _playerInventoryDisplay;
        }

        public override void Activate()
        {
            base.Activate();
        }
        public override void Update(GameTime gameTime)
        {
            if(_secondaryInventoryDisplay.IsActive && Controls.ControllerConnected)
            {
                if(Controls.WasGamePadButtonTapped(GamePadActionType.TriggerRight))
                    SwapControl(_secondaryInventoryDisplay);
                else if (Controls.WasGamePadButtonTapped(GamePadActionType.TriggerLeft))
                {
                    SwapControl(_playerInventoryDisplay);

                }
            }
            base.Update(gameTime);
            if (IsActive && !_secondaryInventoryDisplay.WasJustActivated)
            {
                _currentlySelectedInventoryDisplay.UpdateSelectorSprite(gameTime);

                if (Controls.IsClickedWorld || Controls.WasGamePadButtonTapped(GamePadActionType.Cancel) ||
                    Controls.WasGamePadButtonTapped(GamePadActionType.Y))
                {
                    if (_currentlySelectedInventoryDisplay == _secondaryInventoryDisplay)
                        SwapControl(_playerInventoryDisplay);
                    if (_secondaryInventoryDisplay.IsActive || Controls.WasGamePadButtonTapped(GamePadActionType.Cancel))
                        _playerInventoryDisplay.CloseExtendedInventory();
                    DeactivateSecondaryDisplay();

                    
                    Flags.Pause = false;


                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(IsActive)
            _currentlySelectedInventoryDisplay.DrawSelectorSprite(spriteBatch);
        }

        /// <summary>
        /// Swaps active inventory, useful for controller support where cursor cannot move between inventories
        /// </summary>
        private void SwapControl(InventoryDisplay inventoryDisplay)
        {
            //Do nothing if tried to swap to currently controlled inventory
            if (_currentlySelectedInventoryDisplay == inventoryDisplay)
                return;

            _currentlySelectedInventoryDisplay.RemoveControl();

            if (inventoryDisplay == _playerInventoryDisplay)
            {

                _currentlySelectedInventoryDisplay = _playerInventoryDisplay;

            }
            else if( inventoryDisplay == _secondaryInventoryDisplay)
            {
                _currentlySelectedInventoryDisplay = _secondaryInventoryDisplay;

                _currentlySelectedInventoryDisplay.OpenExtendedInventory();
                //_currentlySelectedInventoryDisplay.CloseExtendedInventory();

            }
            _currentlySelectedInventoryDisplay.GiveControl();

        }
        public void DeactivateSecondaryDisplay()
        {
            _secondaryInventoryDisplay.Deactivate();
        }
       
        public void ActivateSecondaryInventoryDisplay(FurnitureType t, StorageContainer storageContainer, bool displayWallet = false)
        {
            ChildSections.Remove(_secondaryInventoryDisplay);
            switch (t)

            {
                case FurnitureType.None:
                    throw new Exception($"must have storage type");
                case FurnitureType.StorableFurniture:
                    
                    _secondaryInventoryDisplay = new InventoryDisplay(this, graphics, content, _secondaryInventoryDisplay.Position,
                        _secondaryInventoryDisplay.LayerDepth);

                    break;
                case FurnitureType.DiningTable:
                    _secondaryInventoryDisplay = new DiningTableDisplay(this, graphics, content, _secondaryInventoryDisplay.Position,
                        _secondaryInventoryDisplay.LayerDepth);

                    break;
                case FurnitureType.Mixer:
                    _secondaryInventoryDisplay = new MixerDisplay(this, graphics, content, _secondaryInventoryDisplay.Position,
                        _secondaryInventoryDisplay.LayerDepth);
                    break;
                case FurnitureType.CraftingTable:
                    _secondaryInventoryDisplay = new FurnaceTableDisplay(this, graphics, content, _secondaryInventoryDisplay.Position,
                        _secondaryInventoryDisplay.LayerDepth);
                    break;
                default:

                    throw new Exception($"must have storage type");

            }
            _secondaryInventoryDisplay.MovePosition(RectangleHelper.CenterRectangleOnScreen(_secondaryInventoryDisplay.TotalBounds));
            _secondaryInventoryDisplay.MovePosition(_secondaryInventoryDisplay.Position + new Vector2(128, - 128));
            _secondaryInventoryDisplay.LoadNewEntityInventory(storageContainer, displayWallet);

            _secondaryInventoryDisplay.Activate();
            //Activate();
            _playerInventoryDisplay.OpenExtendedInventory();
            _playerInventoryDisplay.GiveControl();
        }

        
    }
}
