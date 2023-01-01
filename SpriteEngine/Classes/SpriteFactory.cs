using DataModels;
using DataModels.NPCStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.InterfaceStuff;
using SpriteEngine.Classes.ParticleStuff;
using SpriteEngine.Classes.ShadowStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TextEngine.Classes;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes
{
    public static class SpriteFactory 
    {

        public static Color SubmergedColor = new Color(74, 92, 133);
        private static GraphicsDevice Graphics { get; set; }

        private static ContentManager Content { get; set; }

        public static Texture2D StatusIconTexture { get; set; }

        public static Effect LightEffect { get; set; }
        public static Effect FlashEffect { get; set; }

        private static Texture2D _lightMask;
        private static readonly Rectangle _lightSourceRectangle = new Rectangle(0, 0, 64, 64);

        internal static Texture2D HatTexture;
        internal static Texture2D HairTexture;
        internal static Texture2D EyesTexture;
        internal static Texture2D HeadTexture;
        internal static Texture2D ShouldersTexture;

        internal static Texture2D ArmsTexture;
        internal static Texture2D ShirtTexture;
        internal static Texture2D PantsTexture;
        internal static Texture2D ShoesTexture;

        public  static List<Color> SkinColors;


        public static Texture2D NPCSheet;

        public static Texture2D Props_1;
        public static Dictionary<ActionType, byte> PerformActionCustomizeableTriggers = new Dictionary<ActionType, byte>();
        public static void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            Graphics = graphics;
            Content = content;
            StatusIconTexture = content.Load<Texture2D>("Entities/Characters/StatusIcons");
            LightEffect = content.Load<Effect>("Effects/Lighting/lighteffect");
            FlashEffect = content.Load<Effect>("Effects/Shaders/FlashEffect");

            _lightMask = content.Load<Texture2D>("Effects/Lighting/lightmask");


            HatTexture = content.Load<Texture2D>("Entities/Hats");
            HairTexture = content.Load<Texture2D>("Entities/Hair");

            EyesTexture = content.Load<Texture2D>("Entities/Eyes");

            HeadTexture = content.Load<Texture2D>("Entities/Heads");
            ShouldersTexture = content.Load<Texture2D>("Entities/Shoulders");

            ArmsTexture = content.Load<Texture2D>("Entities/Arms");

            ShirtTexture = content.Load<Texture2D>("Entities/Shirts");

            PantsTexture = content.Load<Texture2D>("Entities/Pants");

            ShoesTexture = content.Load<Texture2D>("Entities/Shoes");
            SkinColors = new List<Color>()
            {
                new Color(141, 85, 36),
                new Color(198, 134, 66),
                new Color(224, 172, 105),
                new Color(241, 194, 125),
                new Color(255, 219, 172),

            };

            ParticleManager.Load(content);

            NPCSheet = content.Load<Texture2D>("Entities/NPC/NPCSheet");

            Props_1 = content.Load<Texture2D>("Entities/Props/Props_1");

            PerformActionCustomizeableTriggers = new Dictionary<ActionType, byte>();
            PerformActionCustomizeableTriggers.Add(ActionType.Interact, 2);
            PerformActionCustomizeableTriggers.Add(ActionType.Smash, 2);

        }
        public static Color GetRandomSkinTone()
        {
            return SkinColors[Settings.Random.Next(0, SkinColors.Count)];
        }
        public static LightSprite CreateLight(Vector2 position,Vector2 offSet, LightType lightType, float scale)
        {
            Sprite lightSprite = CreateWorldSprite(position, _lightSourceRectangle, _lightMask,
                origin: new Vector2(_lightSourceRectangle.Width / 2, _lightSourceRectangle.Height / 2),
                scale: new Vector2(2, 2) * scale,
                primaryColor: LightSprite.ColorFromLightType(lightType), customLayer: .99f);
            return new LightSprite(lightSprite, offSet);
        }
        public static Sprite CreateLight(Vector2 position)
        {
            //note: Color.LightBlue is nice here for more ominous nautical vibes
            return CreateWorldSprite(position, _lightSourceRectangle, _lightMask,origin: new Vector2(_lightSourceRectangle.Width/2, _lightSourceRectangle.Height/2),scale: new Vector2(4,4),primaryColor: Color.White, customLayer:.99f);
        }
        /// <summary>
        /// For World vector2 position
        /// </summary>
        public static Sprite CreateWorldSprite(Vector2 position,Rectangle sourceRectangle, Texture2D texture, Color? primaryColor = null,
             Vector2? origin = null, Vector2? scale = null, float rotation = 0f, Layers layer = Layers.buildings,
            bool randomizeLayers = true, bool flip = false, float? customLayer = null)
        {
            return new Sprite(Graphics, Content,ElementType.World, position, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Vector2.One, rotation, layer, 
                randomizeLayers, flip, customLayer) ;
        }
        public static DestinationRectangleSprite CreateDestinationSprite(int width, int height, Vector2 position, Rectangle sourceRectangle, Texture2D texture, ElementType elementType, Color? primaryColor = null,
             Vector2? origin = null, Vector2? scale = null, float rotation = 0f, Layers layer = Layers.buildings,
            bool randomizeLayers = true, bool flip = false, float? customLayer = null)
        {
            return new DestinationRectangleSprite(width, height,Graphics, Content, elementType, position, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Vector2.One, rotation, layer,
                randomizeLayers, flip, customLayer);
        }
        /// <summary>
        /// For UI vector2 position.  Note ui sprites are required to pass in a custom layer
        /// </summary>
        public static Sprite CreateUISprite(Vector2 position, Rectangle sourceRectangle, Texture2D texture,float customLayer, Color? primaryColor = null,
             Vector2? origin = null, Vector2? scale = null, float rotation = 0f, 
            bool randomizeLayers = false, bool flip = false)
        {
            return new Sprite(Graphics, Content, ElementType.UI, position, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Vector2.One, rotation, Layers.buildings, 
                randomizeLayers, flip, customLayer);
        }


        public static NineSliceSprite CreateNineSliceSprite(Vector2 position, int width, int height,
            Texture2D texture,float layer, Color? primaryColor = null, Vector2? origin = null, Vector2? scale = null)
        {
            NineSlice newNineSlice = new NineSlice(position, texture,
                layer, width, height, primaryColor ?? Color.White, scale ?? Vector2.One);


            return new NineSliceSprite(Graphics, Content, position, newNineSlice, texture,
                primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Vector2.One, layer);
        }

        /// <summary>
        /// Nine slice to support text
        /// </summary>
        /// <returns></returns>
        public static NineSliceSprite CreateNineSliceTextSprite(Vector2 position, List<Text> textList,
            Texture2D texture,float layer, Color? primaryColor = null, Vector2? origin = null, Vector2? scale = null)
        {

            Point p = GetTextListWidthAndHeight(textList);
            p.X += 32;
            p.Y += 32;

            NineSlice newNineSlice = new NineSlice(position, texture,
                layer, p.X, p.Y, primaryColor ?? Color.White, scale ?? Vector2.One);


            return new NineSliceSprite(Graphics, Content, position, newNineSlice, texture,
                primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Vector2.One, layer);
        }

        private static Point GetTextListWidthAndHeight(List<Text> text)
        {
            int width = 0;
            int height = 0;

            foreach(Text t in text)
            {
                if(t.Width > width)
                    width = (int)t.Width;

                height += (int)t.Height;
            }

            return new Point(width,height);
        }
        /// <summary>
        /// Animated sprite
        /// </summary>

        public static AnimatedSprite CreateWorldAnimatedSprite(Vector2 position, Rectangle startingSourceRectangle, Texture2D texture,
           AnimationFrame[] animationFrames, float standardDuration = .15f, Color? primaryColor = null,
            Vector2? origin = null, Vector2? scale = null, float rotation = 0f, Layers layer = Layers.buildings,
           bool randomizeLayers = true, bool flip = false, float? customLayer = null, int idleFrame = -1)
        {
            return new AnimatedSprite(Graphics, Content, ElementType.World, position, startingSourceRectangle,
                texture, animationFrames, standardDuration, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ??Vector2.One, rotation, layer,
                randomizeLayers, flip, customLayer, idleFrame);
        }
        public static AnimatedSprite CreateUIAnimatedSprite(Vector2 position, Rectangle startingSourceRectangle, Texture2D texture,
        AnimationFrame[] animationFrames, float standardDuration = .15f, Color? primaryColor = null,
         Vector2? origin = null, Vector2? scale = null, float rotation = 0f, Layers layer = Layers.buildings,
        bool randomizeLayers = true, bool flip = false, float? customLayer = null, int idleFrame = -1)
        {
            return new AnimatedSprite(Graphics, Content, ElementType.UI, position, startingSourceRectangle,
                texture, animationFrames, standardDuration, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Vector2.One, rotation, layer,
                randomizeLayers, flip, customLayer, idleFrame);
        }
        public static AnimatedSprite AnimationInfoToWorldSprite(Vector2 position, AnimationInfo info,Texture2D texture, Rectangle startingSourceRectangle, int xOffSet, int yOffset, bool flip)
        {
            AnimationFrame[] animationFrames = new AnimationFrame[info.FrameIndicies.Count];

            for (int i = 0; i < info.FrameIndicies.Count; i++)
            {
                animationFrames[i] = new AnimationFrame(info.FrameIndicies[i], xOffSet, yOffset,.15f, flip);
            }

            return CreateWorldAnimatedSprite(position, startingSourceRectangle, texture, animationFrames);
        }

        public static IntervalAnimatedSprite CreateWorldIntervalAnimatedSprite(Vector2 position, Rectangle startingSourceRectangle, Texture2D texture,
           AnimationFrame[] animationFrames, float standardDuration = .15f, Color? primaryColor = null,
            Vector2? origin = null, Vector2? scale = null, float rotation = 0f, Layers layer = Layers.buildings,
           bool randomizeLayers = true, bool flip = false, float? customLayer = null, int idleFrame = -1)
        {
            return new IntervalAnimatedSprite(Graphics, Content, ElementType.World, position, startingSourceRectangle,
                texture, animationFrames, standardDuration, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Vector2.One, rotation, layer,
                randomizeLayers, flip, customLayer, idleFrame);
        }

        public static Texture2D GetTextureFromNPCType(NPCType npcType)
        {
            if (npcType == NPCType.Enemy)
                return NPCSheet;
            else if (npcType == NPCType.Prop)
                return Props_1;
            else
                throw new Exception($"Invalid npc type {npcType.ToString()}");
        }
    }
}
