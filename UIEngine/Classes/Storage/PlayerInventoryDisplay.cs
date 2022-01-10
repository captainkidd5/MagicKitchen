using EntityEngine.Classes;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Globals.Classes.Settings;

namespace UIEngine.Classes.Storage
{
    internal class PlayerInventoryDisplay : InventoryDisplay
    {
        private Sprite _selectorSprite;

        public PlayerInventoryDisplay(InterfaceSection interfaceSection, GraphicsDevice graphicsDevice, ContentManager content, Vector2? position, Entity entity) : base(interfaceSection, graphicsDevice, content, position, entity)
        {
            _selectorSprite = SpriteFactory.CreateUISprite(SelectedSlot.Position, new Rectangle(272, 0, 64, 64),
                UserInterface.ButtonTexture, null, layer: Settings.Layers.foreground);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateSelectorIndex();
            _selectorSprite.Update(gameTime, SelectedSlot.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _selectorSprite.Draw(spriteBatch);
        }

        /// <summary>
        /// Changes selected slot based on the controls scroll wheel behavior
        /// </summary>
        private void UpdateSelectorIndex()
        {
            if (Controls.ScrollWheelIncreased)
            {
                SelectedSlot = InventorySlots[ScrollHelper.GetIndexFromScroll(
                    Direction.Up, InventorySlots.IndexOf(SelectedSlot),
                    InventorySlots.Count)];
            }
            else if (Controls.ScrollWheelDecreased)
            {
                SelectedSlot = InventorySlots[ScrollHelper.GetIndexFromScroll(
                    Direction.Down, InventorySlots.IndexOf(SelectedSlot),
                    InventorySlots.Count)];
            }
        }
    }
}
