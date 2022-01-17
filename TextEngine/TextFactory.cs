using Globals.Classes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine.Classes;
using static Globals.Classes.Settings;

namespace TextEngine
{
    public static class TextFactory
    {
        public static SpriteFont DefaultFont { get; set; }


        public static void Load(SpriteFont defaultFont)
        {
            DefaultFont = defaultFont;
        }

        //UI
        public static Text CreateUIText(String value, float layer, float? scale = null, SpriteFont spriteFont = null)
        {
            return new Text(value, scale ?? Settings.GameScale, spriteFont ?? DefaultFont, layer);
        }
    }
}
