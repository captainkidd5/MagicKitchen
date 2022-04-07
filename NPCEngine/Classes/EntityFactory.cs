using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityEngine.Classes
{

    public static class EntityFactory
    {
        internal static Texture2D HatTexture;
        internal static Texture2D HairTexture;
        internal static Texture2D EyesTexture;
        internal static Texture2D HeadTexture;
        internal static Texture2D ArmsTexture;
        internal static Texture2D ShirtTexture;
        internal static Texture2D PantsTexture;
        internal static Texture2D ShoesTexture;

        internal static Color[] StartingGrayScale;

        internal static List<Color> SkinColors;
        public static void Load(ContentManager content)
        {
            HatTexture = content.Load<Texture2D>("Entities/Hats");
            HairTexture = content.Load<Texture2D>("Entities/Hair");

            EyesTexture = content.Load<Texture2D>("Entities/Eyes");

            HeadTexture = content.Load<Texture2D>("Entities/Heads");

            ArmsTexture = content.Load<Texture2D>("Entities/Arms");

            ShirtTexture = content.Load<Texture2D>("Entities/Shirts");

            PantsTexture = content.Load<Texture2D>("Entities/Pants");

            ShoesTexture = content.Load<Texture2D>("Entities/Shoes");
            SetStartingGrayscale();

          SkinColors = new List<Color>()
            {
                new Color(141, 85, 36),
                new Color(198, 134, 66),
                new Color(224, 172, 105),
                new Color(241, 194, 125),
                new Color(255, 219, 172),

            };
        }

        public static Color GetRandomSkinTone()
        {
            return SkinColors[Settings.Random.Next(0, SkinColors.Count)];
        }

        /// <summary>
        /// Starting grayscale is in multiples of 17 for rgb values
        /// </summary>
        private static void SetStartingGrayscale()
        {
            StartingGrayScale = new Color[16];
            for (int i = 0; i < 16; i++)
            {
                StartingGrayScale[i] = new Color(i * 17, i * 17, i * 17, 255);
            }
        }
    }
}
