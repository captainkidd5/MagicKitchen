using Microsoft.Xna.Framework;
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
    public class ImageFont
    {
        public static readonly int FontDimension = 32;
        public static readonly int FontSpaceWidth = 64;

        public Texture2D StandardFontTexture { get; set; }

        private Dictionary<FontType, Texture2D> _fontTextures;

        public ImageFont()
        {

        }

        public Rectangle GetCharRect(char c)
        {
            Point point = LanguageManager.CurrentLanguage.CharPoints[c];
            return new Rectangle(point.X * FontDimension, point.Y * FontDimension, FontDimension, FontDimension);
        }
        public void LoadContent(Texture2D standardFontTexture)
        {
            StandardFontTexture = standardFontTexture;
            _fontTextures = new Dictionary<FontType, Texture2D>();
            _fontTextures.Add(FontType.Standard, standardFontTexture);
        }
        public Texture2D GetTexture(FontType fontType)
        {
            return _fontTextures[fontType];
        }
    }
}
