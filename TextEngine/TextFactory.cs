using Globals.Classes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine.Classes;
using static Globals.Classes.Settings;

namespace TextEngine
{
    public static class TextFactory
    {
        public static BitmapFont BitmapFont { get; set; }


        public static void Load(ContentManager content)
        {
            BitmapFont = content.Load<BitmapFont>("UI/Fonts/test_font_1");



        }

        //UI
        public static Text CreateUIText(String value, float layer, float? scale = null, BitmapFont spriteFont = null)
        {
            return new Text(value, scale ?? Settings.GameScale, spriteFont ?? BitmapFont, layer);
        }

        /// <summary>
        /// Concats all text into single text separated by new line character
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static Text CombineText(List<Text> texts, float layer)
        {
            string newString = string.Empty;
            foreach(Text text in texts)
            {
                newString += text.FullString;
                newString += "\n";
            }
            return CreateUIText(newString, layer);

        }
    }
}
