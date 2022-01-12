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
using EntityEngine.Classes.PlayerStuff;
using StageEngine.Classes;

namespace UIEngine.Classes
{
    internal class ToolBar : InterfaceSection
    {

       // private NineSliceSprite _backDropSprite;
        private PlayerInventoryDisplay _playerInventoryDisplay;
        //private static readonly Rectangle _totalBackDropRectangleDimensions = new Rectangle(0, 0, 800, 64);
        private int _totalToolbarSlots = 10;
        private int _toolBarSlotWidth = 64;

        public ToolBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position) :
            base(interfaceSection, graphicsDevice, content, position)
        {

        }

        public override void Load()
        {
            base.Load();



            //X and y actually don't matter, multiply by 10 because toolbar is 10 slots wide, at 64 pixels per slot
            Rectangle totalToolBarRectangle = new Rectangle(0, 0, _toolBarSlotWidth * _totalToolbarSlots, _toolBarSlotWidth);

            _playerInventoryDisplay = new PlayerInventoryDisplay(this, graphics, content,
                RectangleHelper.PlaceBottomCenterScreen(totalToolBarRectangle));
            _playerInventoryDisplay.LoadNewEntityInventory(PlayerManager.Player1);
            _playerInventoryDisplay.Load();

            ChildSections.Add(_playerInventoryDisplay);
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);


            if (!Flags.Pause)
                if (Controls.WasKeyTapped(Keys.Q))
                    TryEjectItem();

        }

        /// <summary>
        /// If player inventory selected slot contains an item, eject 1 count of it in front of the player.
        /// </summary>
        private void TryEjectItem()
        {
            Item item = _playerInventoryDisplay.SelectedSlot.Item;
            if(item != null)
                _playerInventoryDisplay.SelectedSlot.Remove(1);
            //if (itemToDrop != null)
            //{
            //    itemToDrop.Drop(PlayerManager.Player1.Position, PlayerManager.Player1.DirectionMoving, StageManager.CurrentStage.Items);
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _playerInventoryDisplay.Draw(spriteBatch);
        }
    }
}
