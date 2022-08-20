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

namespace UIEngine.Classes.CursorStuff
{
    /// <summary>
    /// Usually displayed to the bottom right of the normal cursor. Adds info about things like items which are currently held
    /// </summary>
    internal class CursorItemToolTip
    {
        private Sprite ItemSprite { get; set; }

        private Vector2 _cursorOffset = new Vector2(16, 16);

        private Text _text;
        private float _customLayer = .9f;
        public CursorItemToolTip()
        {
            ItemSprite = SpriteFactory.CreateUISprite(Vector2.Zero, new Rectangle(0, 0, 16, 16),
                ItemFactory.ItemSpriteSheet, _customLayer, Color.White, null);
            _text = TextFactory.CreateUIText("tst", _customLayer);
        }
        public void Update(GameTime gameTime, Vector2 position)
        {
            ItemSprite.Update(gameTime, position + _cursorOffset);
            _text.Update(position + _cursorOffset * 2);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            ItemSprite.Draw(spriteBatch);
            _text.Draw(spriteBatch);
        }

        public void UpdateItemCount(int newCount)
        {
            _text.ClearAndSet(newCount.ToString());
        }
        public void SwapSprite(Rectangle newSource, Texture2D texture, Vector2 scale)
        {
            ItemSprite.SwapSourceRectangle(newSource);
            ItemSprite.SwapScale(scale);
            ItemSprite.SwapTexture(texture);

        }
    }
}
