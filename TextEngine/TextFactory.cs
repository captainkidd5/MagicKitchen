using DataModels.DialogueStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
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


        public static Texture2D CurrentFontLanguage { get; set; }

        public static ImageFont ImageFont { get; set; }
        public static void Load(ContentManager content)
        {
            s_content = content;
            

        }

        private static void OnLanguageChanged()
        {
            CurrentFontLanguage = s_content.Load<Texture2D>($"UI/Fonts/{LanguageManager.CurrentLanguage.Name}/StandardFont");

            ImageFont = new ImageFont();
            ImageFont.LoadContent(CurrentFontLanguage);
        }
       
        //UI
        public static Text CreateUIText(String value, ImageFont? imageFont = null, FontType? fontType = null,Color? color = null, Vector2? scale = null)
        {
            return new Text(value, imageFont ?? ImageFont, fontType?? FontType.Standard, color ?? Color.White,scale ?? Vector2.One);
        }

        
    }
}
