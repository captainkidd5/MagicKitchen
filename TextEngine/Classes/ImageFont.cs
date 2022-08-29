using DataModels.JsonConverters;
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
        internal readonly int FontDimension = 16;
        internal readonly int FontSpaceWidth = 8;
       
        internal Texture2D StandardFontTexture { get; set; }

        private Dictionary<FontType, Texture2D> _fontTextures;

        public ImageFont()
        {

        }

        internal Rectangle GetCharRect(char c)
        {
            LetterData point = LanguageManager.CurrentLanguage.CharPoints[c];
            return new Rectangle(point.X * FontDimension, point.Y * FontDimension, point.Width, point.Height);
        }

        /// <summary>
        /// Gets the spacing between characters. If no spacing is specified, return the default spacing
        /// </summary>
        internal int GetCharSpacing(char c)
        {
            LetterData point = LanguageManager.CurrentLanguage.CharPoints[c];
            return point.OverrideGap == 0 ? FontSpaceWidth : point.OverrideGap;
        }

        internal void LoadContent(Texture2D standardFontTexture)
        {
            StandardFontTexture = standardFontTexture;
            _fontTextures = new Dictionary<FontType, Texture2D>();
            _fontTextures.Add(FontType.Standard, standardFontTexture);
        }
        internal Texture2D GetTexture(FontType fontType)
        {
            return _fontTextures[fontType];
        }
    }
}
