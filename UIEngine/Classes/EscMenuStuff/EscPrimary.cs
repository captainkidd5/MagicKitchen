using Globals.Classes.Helpers;
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

namespace UIEngine.Classes.EscMenuStuff
{
    internal class EscPrimary : MenuSection
    {

        private Rectangle _returnToMainMenuButtonBackgroundDimensions = new Rectangle(0, 0, 80, 96);
        private NineSliceButton _returnToMainMenuButton;

        
        public EscPrimary(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position,
            float layerDepth) : base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        private void ReturnToMainMenu()
        {
            UI.ReturnToMainMenu();
            _returnToMainMenuButton.Deactivate();
            Deactivate();

        }
        public override void LoadContent()
        {
            ClearGrid();
            ChildSections.Clear();
            TotalBounds = parentSection.TotalBounds;
            _returnToMainMenuButton = UI.ButtonFactory.CreateNSliceTxtBtn(this,
                RectangleHelper.CenterRectangleInRectangle(_returnToMainMenuButtonBackgroundDimensions, TotalBounds), GetLayeringDepth(UILayeringDepths.Low),
                new List<string>() { "Return to main menu" }, ReturnToMainMenu);

            _returnToMainMenuButton.AddConfirmationWindow($"Return to main menu?");

            _returnToMainMenuButton.LoadContent();
            AddSectionToGrid(_returnToMainMenuButton, 0, 1);

            base.LoadContent();
        }
    }
}
