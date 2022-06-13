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
using UIEngine.Classes.Components;

namespace UIEngine.Classes.EscMenuStuff
{
    internal class EscPrimary : MenuSection
    {

        private Rectangle _returnToMainMenuButtonBackgroundDimensions = new Rectangle(0, 0, 80, 96);
        private NineSliceButton _returnToMainMenuButton;

        private StackPanel _stackPanel;
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
            int offSet = (parentSection as EscMenu).TitleOffSet;
            Position = new Vector2(TotalBounds.X, TotalBounds.Y + offSet);


            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Medium));

            StackRow stackRow1 = new StackRow(TotalBounds.Width);
            _returnToMainMenuButton = UI.ButtonFactory.CreateNSliceTxtBtn(_stackPanel,
                RectangleHelper.CenterRectangleInRectangle(_returnToMainMenuButtonBackgroundDimensions, TotalBounds), GetLayeringDepth(UILayeringDepths.Low),
                new List<string>() { "Return to main menu" }, ReturnToMainMenu);

            _returnToMainMenuButton.AddConfirmationWindow($"Return to main menu?");
            stackRow1.AddItem(_returnToMainMenuButton, StackOrientation.Center);
            _returnToMainMenuButton.LoadContent();
            _stackPanel.Add(stackRow1);

            AddSectionToGrid(_returnToMainMenuButton, 0, 1);

            base.LoadContent();
        }
    }
}
