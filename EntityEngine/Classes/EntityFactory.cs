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
        internal static Texture2D ShouldersTexture;

        internal static Texture2D ArmsTexture;
        internal static Texture2D ShirtTexture;
        internal static Texture2D PantsTexture;
        internal static Texture2D ShoesTexture;

        internal static Texture2D NPCSheet;

        internal static Texture2D Props_1;

        internal static List<Color> SkinColors;
        public static void Load(ContentManager content)
        {
            HatTexture = content.Load<Texture2D>("Entities/Hats");
            HairTexture = content.Load<Texture2D>("Entities/Hair");

            EyesTexture = content.Load<Texture2D>("Entities/Eyes");

            HeadTexture = content.Load<Texture2D>("Entities/Heads");
            ShouldersTexture = content.Load<Texture2D>("Entities/Shoulders");

            ArmsTexture = content.Load<Texture2D>("Entities/Arms");

            ShirtTexture = content.Load<Texture2D>("Entities/Shirts");

            PantsTexture = content.Load<Texture2D>("Entities/Pants");

            ShoesTexture = content.Load<Texture2D>("Entities/Shoes");

            NPCSheet = content.Load<Texture2D>("Entities/NPC/NPCSheet");

            Props_1 = content.Load<Texture2D>("Entities/Props/Props_1");

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

    }
}
