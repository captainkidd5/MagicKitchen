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
        private PlayerInventoryDisplay _playerInventoryDisplay;
        private int _totalToolbarSlots = 10;
        private int _toolBarSlotWidth = 64;

        private Button _openRecipeBookButton;
        private Rectangle _openRecipeIcon = new Rectangle(160, 80, 32, 32);

        private int _totalWidth => _toolBarSlotWidth * _totalToolbarSlots;

        public ToolBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public void Load(StorageContainer playerStorageContainer)
        {
            //X and y actually don't matter, multiply by 10 because toolbar is 10 slots wide, at 64 pixels per slot
            Rectangle totalToolBarRectangle = new Rectangle(0, 0, _toolBarSlotWidth * _totalToolbarSlots, _toolBarSlotWidth);

            Position = RectangleHelper.PlaceBottomCenterScreen(totalToolBarRectangle) + new Vector2(0, -32);
           
            _stackPanel = new StackPanel(this, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));

            _playerInventoryDisplay = new PlayerInventoryDisplay(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low));
            _playerInventoryDisplay.LoadNewEntityInventory(playerStorageContainer, true);
            _playerInventoryDisplay.LoadContent();

            StackRow stackRow = new StackRow(_totalWidth);
            stackRow.AddItem(_playerInventoryDisplay, StackOrientation.Left);

            _openRecipeBookButton = new Button(_stackPanel, graphics, content, Position, GetLayeringDepth(UILayeringDepths.Low),
               _openRecipeIcon, new Action(()=> { UI.RecipeBook.Toggle(); }));
            stackRow.AddItem(_openRecipeBookButton, StackOrientation.Left);
            _stackPanel.Add(stackRow);


            TotalBounds = new Rectangle((int)_playerInventoryDisplay.Position.X, (int)_playerInventoryDisplay.Position.Y, totalToolBarRectangle.Width, totalToolBarRectangle.Height);



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
