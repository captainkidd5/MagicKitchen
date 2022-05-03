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

        public StorageDisplayHandler(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {
            NormallyActivated = false;
        }
        public override void LoadContent()
        {
            _secondaryInventoryDisplay = new InventoryDisplay(this, graphics, content, null,
               GetLayeringDepth(UILayeringDepths.Medium));
            _secondaryInventoryDisplay.Deactivate();
            
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive && !WasJustActivated)
            {
                if (Controls.IsClickedWorld)
                {
                    Deactivate();

                }
            }
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

            _secondaryInventoryDisplay.LoadNewEntityInventory(storageContainer, displayWallet);

            _secondaryInventoryDisplay.Activate();
            _secondaryInventoryDisplay.MovePosition(RectangleHelper.CenterRectangleOnScreen(_secondaryInventoryDisplay.TotalBounds));
            Activate();
        }

        
    }
}
