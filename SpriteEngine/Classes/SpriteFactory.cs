using DataModels;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.Animations;
using SpriteEngine.Classes.InterfaceStuff;
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
        private static GraphicsDevice Graphics { get; set; }

        private static ContentManager Content { get; set; }

        public static Texture2D StatusIconTexture { get; set; }

        public static Effect LightEffect { get; set; }

        private static Texture2D _lightMask;
        private static readonly Rectangle _lightSourceRectangle = new Rectangle(0, 0, 64, 64);

        public static void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            Graphics = graphics;
            Content = content;
            StatusIconTexture = content.Load<Texture2D>("entities/characters/statusicons");
            LightEffect = content.Load<Effect>("Effects/Lighting/lighteffect");
            _lightMask = content.Load<Texture2D>("Effects/Lighting/lightmask");

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
        public static NineSliceSprite CreateNineSliceTextSprite(Vector2 position, Text text,
            Texture2D texture,float layer, Color? primaryColor = null, Vector2? origin = null, Vector2? scale = null)
        {
            NineSlice newNineSlice = new NineSlice(position, texture,
                layer, text, primaryColor ?? Color.White, scale ?? Vector2.One);


            return new NineSliceSprite(Graphics, Content, position, newNineSlice, texture,
                primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Vector2.One, layer);
        }


        /// <summary>
        /// Animated sprite
        /// </summary>

        public static AnimatedSprite CreateWorldAnimatedSprite(Vector2 position, Rectangle startingSourceRectangle, Texture2D texture,
           AnimationFrame[] animationFrames, float standardDuration = .15f, Color? primaryColor = null,
            Vector2? origin = null, Vector2? scale = null, float rotation = 0f, Layers layer = Layers.buildings,
           bool randomizeLayers = true, bool flip = false, float? customLayer = null, int idleFrame = 0)
        {
            return new AnimatedSprite(Graphics, Content, ElementType.World, position, startingSourceRectangle,
                texture, animationFrames, standardDuration, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ??Vector2.One, rotation, layer,
                randomizeLayers, flip, customLayer, idleFrame);
        }

        public static AnimatedSprite AnimationInfoToWorldSprite(Vector2 position, AnimationInfo info,Texture2D texture, Rectangle startingSourceRectangle, int xOffSet, int yOffset)
        {
            AnimationFrame[] animationFrames = new AnimationFrame[info.FrameIndicies.Count];

            for (int i = 0; i < info.FrameIndicies.Count; i++)
            {
                animationFrames[i] = new AnimationFrame(info.FrameIndicies[i], xOffSet, yOffset,.15f,false);
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
    }
}
