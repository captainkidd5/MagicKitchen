using DataModels.DialogueStuff;
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
        private static ContentManager s_content;

        public static BitmapFont BitmapFont { get; set; }

        public static Texture2D CurrentFontLanguage { get; set; }

        public static ImageFont ImageFont { get; set; }
        public static void Load(ContentManager content)
        {
            s_content = content;
            BitmapFont = content.Load<BitmapFont>("UI/Fonts/test_font_1");
            

        }

        private static void OnLanguageChanged()
        {
            CurrentFontLanguage = s_content.Load<Texture2D>($"UI/Fonts/{LanguageManager.CurrentLanguage.Name}/StandardFont");

            ImageFont = new ImageFont();
            ImageFont.LoadContent(CurrentFontLanguage);
        }
        public static float SingleCharacterWidth(float scale = 1f)
        {
            return BitmapFont.MeasureString("o").Width * scale;

        }
        //UI
        public static Text CreateUIText(String value, float layer, float scale = 1f)
        {
            return new Text(value, scale, ImageFont, layer);
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

            for(int i = 0; i < texts.Count; i++)
            {
                newString += texts[i].FullString;
                if (i < texts.Count - 1)
                    newString += "\n";
            }
           
            return CreateUIText(newString, layer);

        }
    }
}
