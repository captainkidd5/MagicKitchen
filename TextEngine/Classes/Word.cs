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

        public float Height => ImageFont.FontDimension * _scale.Y;
        public float Width => _str.Length * ImageFont.FontDimension;

        public bool Empty => String.IsNullOrEmpty(_str);
        public Word(string str,FontType fontType, ImageFont imgFont, Color? color = null, Vector2? scale = null)
        {
            _str = str;
            _fontType = fontType;
            _imgFont = imgFont;

            _color = color ?? Color.White;
            _scale = scale ?? Vector2.One;

         
        }
        

        public void Append(char c)
        {
            _str += c;
        }
        /// <summary>
        /// Pos should be the start position of the first character of the word
        /// </summary>
        /// <param name="pos"></param>
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
