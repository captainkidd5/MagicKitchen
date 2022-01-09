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

        private NineSliceSprite BackdropSprite { get; set; }
        private Inventory Inventory { get; set; }


        public ToolBar(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position) :
            base(interfaceSection, graphicsDevice, content, position)
        {

        }

        public void Load(Inventory inventory)
        {
            base.Load();

            Rectangle totalBackDropRectangleDimensions = new Rectangle(0, 240, 800, 64);
            Position = RectangleHelper.PlaceBottomLeftScreen(totalBackDropRectangleDimensions);
            BackdropSprite = SpriteFactory.CreateNineSliceSprite(Position, 
                totalBackDropRectangleDimensions.Width,
                totalBackDropRectangleDimensions.Height,
                null, null, null, null, Layers.background);

            //X and y actually don't matter, multiply by 10 because toolbar is 10 slots wide, at 64 pixels per slot
            Rectangle totalToolBarRectangle = new Rectangle(0, 0, 64 * 10, 64);

            Inventory = inventory ?? new Inventory(this, graphics, content,
                RectangleHelper.PlaceBottomCenterScreen(totalToolBarRectangle), PlayerManager.Player1.StorageContainer);

            ChildSections.Add(Inventory);
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            BackdropSprite.Update(gameTime, Position);

            if (!Flags.Pause)
                if (Controls.WasKeyTapped(Keys.Q))
                    TryEjectItem();

        }

        /// <summary>
        /// If player inventory selected slot contains an item, eject 1 count of it in front of the player.
        /// </summary>
        private void TryEjectItem()
        {
            Item itemToDrop = Inventory.SelectedSlot.RemoveOne();
            if (itemToDrop != null)
            {
                itemToDrop.Drop(PlayerManager.Player1.Position, PlayerManager.Player1.DirectionMoving, StageManager.CurrentStage.Items);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            BackdropSprite.Draw(spriteBatch);
            Inventory.Draw(spriteBatch);
        }
    }
}
