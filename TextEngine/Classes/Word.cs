using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    public enum FontType
    {
        None = 0,
        Standard = 1,
        Emphasized = 2
    }
    internal class Word
    {
        private string _str;
        private readonly FontType _fontType;
        private readonly ImageFont _imgFont;

        
        public Word(string str,FontType fontType, ImageFont imgFont)
        {
            _str = str;
            _fontType = fontType;
            _imgFont = imgFont;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(char c in _str)
            {
                spriteBatch.Draw(_imgFont.Texture, )
            }
        }
    }
}
