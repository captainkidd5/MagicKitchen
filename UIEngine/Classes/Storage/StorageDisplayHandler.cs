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
using UIEngine.Classes.CraftingMenuStuff;
using static DataModels.Enums;

namespace UIEngine.Classes.Storage
{
    internal class StorageDisplayHandler : InterfaceSection
    {
        internal InventoryDisplay SecondaryInventoryDisplay { get; set; }

        public StorageDisplayHandler(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice,
            ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }
        public override void LoadContent()
        {
            SecondaryInventoryDisplay = new InventoryDisplay(this, graphics, content, null,
               GetLayeringDepth(UILayeringDepths.Medium));
            SecondaryInventoryDisplay.Deactivate();
            
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive && !WasJustActivated)
            {
                if (!Hovered && Controls.IsClickedWorld)
                {
                    Deactivate();

                }
            }
        }
        public void ActivateSecondaryInventoryDisplay(StorageType t, StorageContainer storageContainer, bool displayWallet = false)
        {
            ChildSections.Remove(SecondaryInventoryDisplay);
            switch (t)

            {
                case StorageType.None:
                    throw new Exception($"must have storage type");
                case StorageType.Standard:
                    
                    SecondaryInventoryDisplay = new InventoryDisplay(this, graphics, content, SecondaryInventoryDisplay.Position,
                        SecondaryInventoryDisplay.LayerDepth);

                    break;
                case StorageType.Craftable:
                    SecondaryInventoryDisplay = new CraftingMenu(this, graphics, content, SecondaryInventoryDisplay.Position,
                        SecondaryInventoryDisplay.LayerDepth);

                    break;
                default:
                    throw new Exception($"must have storage type");

            }

            SecondaryInventoryDisplay.LoadNewEntityInventory(storageContainer, displayWallet);

            SecondaryInventoryDisplay.Activate();
            SecondaryInventoryDisplay.MovePosition(RectangleHelper.CenterRectangleOnScreen(SecondaryInventoryDisplay.TotalBounds));
            Activate();
        }

        
    }
}
