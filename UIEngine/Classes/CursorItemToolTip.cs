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

namespace UIEngine.Classes
{
    /// <summary>
    /// Usually displayed to the bottom right of the normal cursor. Adds info about things like items which are currently held
    /// </summary>
    internal class CursorItemToolTip
    {
        private Sprite ItemSprite { get; set; }
        private Rectangle _sourceRectangle;

        private Vector2 _cursorOffset = new Vector2(16, 16);

        public CursorItemToolTip()
        {
            _sourceRectangle = new Rectangle(0, 0, 16, 16);
            ItemSprite = SpriteFactory.CreateUISprite(Vector2.Zero, _sourceRectangle,
                ItemFactory.ItemSpriteSheet, Color.White, scale: 1f, layer: Settings.Layers.front);
        }
        public void Update(GameTime gameTime, Vector2 position)
        {

            ItemSprite.Update(gameTime, position + _cursorOffset);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
          //  if(ItemSprite.
            ItemSprite.Draw(spriteBatch);
        }

        public void SwapSprite(Rectangle newSource, Texture2D texture, float scale)
        {
            ItemSprite.SwapSourceRectangle(newSource);
            ItemSprite.SwapScale(scale);
            ItemSprite.SwapTexture(texture);

        }
    }
}
