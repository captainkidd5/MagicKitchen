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

namespace UIEngine.Classes
{
    internal class ToolBar : InterfaceSection
    {

       // private NineSliceSprite _backDropSprite;
        private PlayerInventoryDisplay _playerInventoryDisplay;
        //private static readonly Rectangle _totalBackDropRectangleDimensions = new Rectangle(0, 0, 800, 64);
        private int _totalToolbarSlots = 10;
        private int _toolBarSlotWidth = 64;

        public ToolBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, float layerDepth) :
            base(interfaceSection, graphicsDevice, content, position, layerDepth)
        {

        }

        public void Load(StorageContainer playerStorageContainer)
        {



            //X and y actually don't matter, multiply by 10 because toolbar is 10 slots wide, at 64 pixels per slot
            Rectangle totalToolBarRectangle = new Rectangle(0, 0, _toolBarSlotWidth * _totalToolbarSlots, _toolBarSlotWidth);

            _playerInventoryDisplay = new PlayerInventoryDisplay(this, graphics, content,
                RectangleHelper.PlaceBottomCenterScreen(totalToolBarRectangle) + new Vector2(0, -32), LayerDepth);
            _playerInventoryDisplay.LoadNewEntityInventory(playerStorageContainer);
            _playerInventoryDisplay.LoadContent();
            TotalBounds = new Rectangle((int)_playerInventoryDisplay.Position.X, (int)_playerInventoryDisplay.Position.Y, totalToolBarRectangle.Width, totalToolBarRectangle.Height);

        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            if(Hovered)
                Console.WriteLine("test");

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _playerInventoryDisplay.Draw(spriteBatch);
        }
    }
}
