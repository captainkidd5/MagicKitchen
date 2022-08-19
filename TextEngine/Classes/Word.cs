using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    
    internal class Word
    {
        private string _str;
        private readonly FontType _fontType;
        private readonly ImageFont _imgFont;
        private Vector2 _pos;

        private Color _color;

        private Vector2 _scale;
        public Word(string str,FontType fontType, ImageFont imgFont, Color? color = null, Vector2? scale = null)
        {
            _str = str;
            _fontType = fontType;
            _imgFont = imgFont;

            _color = color ?? Color.White;
            _scale = scale ?? Vector2.One;
        }

        public void Update(Vector2 pos)
        {
            _pos = pos;
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            Vector2 currentCharPos = _pos;
            foreach(char c in _str)
            {
                spriteBatch.Draw(_imgFont.GetTexture(_fontType), currentCharPos, _imgFont.GetCharRect(c), _color, 0f, Vector2.Zero, _scale,SpriteEffects.None, layerDepth);

                currentCharPos = new Vector2(_pos.X + ImageFont.FontSpaceWidth, _pos.Y);
            }
        }
    }
}
