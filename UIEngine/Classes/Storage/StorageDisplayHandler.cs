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
using UIEngine.Classes.CraftingMenuStuff;
using static DataModels.Enums;

namespace UIEngine.Classes.Storage
{
    internal class StorageDisplayHandler : InterfaceSection
    {
        private InventoryDisplay _secondaryInventoryDisplay;


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
            if(Controls.ControllerConnected && Controls.WasGamePadButtonTapped(GamePadActionType.TriggerRight))
            {
                SwapControl();
            }
            base.Update(gameTime);
            _currentlySelectedInventoryDisplay.UpdateSelectorSprite(gameTime);
            if (IsActive && !WasJustActivated)
            {

                if (Controls.IsClickedWorld || Controls.WasGamePadButtonTapped(GamePadActionType.Cancel))
                {
                    if (_currentlySelectedInventoryDisplay == _secondaryInventoryDisplay)
                        SwapControl();
                    DeactivateSecondaryDisplay();
                    _playerInventoryDisplay.CloseExtendedInventory();
                    Flags.Pause = false;


                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _currentlySelectedInventoryDisplay.DrawSelectorSprite(spriteBatch);
        }

        /// <summary>
        /// Swaps active inventory, useful for controller support where cursor cannot move between inventories
        /// </summary>
        private void SwapControl()
        {
            _currentlySelectedInventoryDisplay.RemoveControl();
            if (_currentlySelectedInventoryDisplay == _playerInventoryDisplay)
            {
                _currentlySelectedInventoryDisplay = _secondaryInventoryDisplay;
                _currentlySelectedInventoryDisplay.OpenExtendedInventory();

            }
            else
            {
                _currentlySelectedInventoryDisplay.CloseExtendedInventory();
                _currentlySelectedInventoryDisplay = _playerInventoryDisplay;

            }
            _currentlySelectedInventoryDisplay.GiveControl();

        }
        public void DeactivateSecondaryDisplay()
        {
            _secondaryInventoryDisplay.Deactivate();
        }
       
        public void ActivateSecondaryInventoryDisplay(StorageType t, StorageContainer storageContainer, bool displayWallet = false)
        {
            ChildSections.Remove(_secondaryInventoryDisplay);
            switch (t)

            {
                case StorageType.None:
                    throw new Exception($"must have storage type");
                case StorageType.Standard:
                    
                    _secondaryInventoryDisplay = new InventoryDisplay(this, graphics, content, _secondaryInventoryDisplay.Position,
                        _secondaryInventoryDisplay.LayerDepth);

                    break;
                case StorageType.Craftable:
                    _secondaryInventoryDisplay = new CraftingMenu(this, graphics, content, _secondaryInventoryDisplay.Position,
                        _secondaryInventoryDisplay.LayerDepth);

                    break;
                default:
                    throw new Exception($"must have storage type");

            }
            _secondaryInventoryDisplay.MovePosition(RectangleHelper.CenterRectangleOnScreen(_secondaryInventoryDisplay.TotalBounds));

            _secondaryInventoryDisplay.LoadNewEntityInventory(storageContainer, displayWallet);

            _secondaryInventoryDisplay.Activate();
            Activate();
            _playerInventoryDisplay.OpenExtendedInventory();
            _playerInventoryDisplay.GiveControl();
        }

        
    }
}
