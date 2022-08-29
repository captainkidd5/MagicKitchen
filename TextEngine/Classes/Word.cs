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
        public string Str { get; private set; }
        private readonly FontType _fontType;
        private readonly ImageFont _imgFont;
        private Vector2 _pos;

        private Color _color;

        private Vector2 _scale;

        public float Height => _imgFont.FontDimension * _scale.Y;
        public float Width => Str.Length * _imgFont.FontDimension * _scale.X;

        public bool Empty => String.IsNullOrEmpty(Str);
        public Word(string str, FontType fontType, ImageFont imgFont, Color? color = null, Vector2? scale = null)
        {
            Str = str;
            _fontType = fontType;
            _imgFont = imgFont;

            _color = Color.Black;
            _scale = scale ?? Vector2.One;


        }


        public void Append(char c)
        {
            Str += c;
        }
        public void Append(string s)
        {
            Str += s;
        }


        /// <summary>
        /// Returns true if string is empty
        /// </summary>
        /// <returns></returns>
        public bool BackSpace()
        {
            if (Str.Length < 1)
                return true;

            Str = Str.Substring(0, Str.Length - 1);
            if (Str.Length < 1)
                return true;
            return false;
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
            foreach (char c in Str)
            {
                if (c == '\n')
                    continue;
                Rectangle charSourceRectangle = _imgFont.GetCharRect(c);
                spriteBatch.Draw(_imgFont.GetTexture(_fontType), currentCharPos, charSourceRectangle, _color, 0f, Vector2.Zero, _scale, SpriteEffects.None, layerDepth);

                currentCharPos = new Vector2(currentCharPos.X + _imgFont.GetCharSpacing(c) * _scale.X, _pos.Y);
            }
        }
    }
}
