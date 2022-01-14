using Globals.Classes;
using ItemEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Classes;

namespace UIEngine.Classes
{
    /// <summary>
    /// Usually displayed to the bottom right of the normal cursor. Adds info about things like items which are currently held
    /// </summary>
    internal class CursorItemToolTip
    {
        private Sprite ItemSprite { get; set; }

        private Vector2 _cursorOffset = new Vector2(16, 16);

        private Text _text;
        public CursorItemToolTip()
        {
            ItemSprite = SpriteFactory.CreateUISprite(Vector2.Zero, new Rectangle(0, 0, 16, 16),
                ItemFactory.ItemSpriteSheet, Color.White, scale: 1f, layer: Settings.Layers.front);
            _text = TextFactory.CreateUIText("tst", null, null, null);
        }
        public void Update(GameTime gameTime, Vector2 position)
        {
            ItemSprite.Update(gameTime, position + _cursorOffset);
            _text.Update(gameTime, position + _cursorOffset * 2);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            ItemSprite.Draw(spriteBatch);
            _text.Draw(spriteBatch, true);
        }

        public void UpdateItemCount(int newCount)
        {
            _text.SetFullString(newCount.ToString());
        }
        public void SwapSprite(Rectangle newSource, Texture2D texture, float scale)
        {
            ItemSprite.SwapSourceRectangle(newSource);
            ItemSprite.SwapScale(scale);
            ItemSprite.SwapTexture(texture);

        }
    }
}
