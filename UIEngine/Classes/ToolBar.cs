using UIEngine.Classes.Storage;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteEngine.Classes;
using SpriteEngine.Classes.InterfaceStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;
using UIEngine.Classes.Components;
using UIEngine.Classes.ButtonStuff;

namespace UIEngine.Classes
{
    internal class ToolBar : InterfaceSection
    {
        private StackPanel _stackPanel;
        private Button _openRecipeBookButton;
        private Rectangle _openRecipeIcon = new Rectangle(160, 80, 32, 32);

        

        public ToolBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public override void LoadContent()
        {
            Rectangle totalRectangle = new Rectangle(0, 0, Settings.ScreenWidth / 4, 32);
            Position = RectangleHelper.PlaceBottomLeftScreen(totalRectangle) + new Vector2(0, -32);
           
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));



            StackRow stackRow = new StackRow(totalRectangle.Width);
  

            _openRecipeBookButton = new Button(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
               _openRecipeIcon, new Action(()=> { UI.RecipeBook.Toggle(); }));
            stackRow.AddItem(_openRecipeBookButton, StackOrientation.Left);
            _stackPanel.Add(stackRow);


            TotalBounds = new Rectangle((int)Position.X, (int)Position.Y, totalRectangle.Width, totalRectangle.Height);

            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);


        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
          //  _playerInventoryDisplay.Draw(spriteBatch);
        }
    }
}
