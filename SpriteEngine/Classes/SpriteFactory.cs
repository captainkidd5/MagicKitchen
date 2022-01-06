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
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes
{
    public static class SpriteFactory 
    {
        private static Texture2D NineSliceTexture { get; set; }
        private static GraphicsDevice Graphics { get; set; }

        private static ContentManager Content { get; set; }

        public static void LoadContent(GraphicsDevice graphics, ContentManager content, Texture2D nineSliceTexture)
        {
            NineSliceTexture = nineSliceTexture;
            Graphics = graphics;
            Content = content;
        }
        /// <summary>
        /// For World vector2 position
        /// </summary>
        public static Sprite CreateWorldSprite(Vector2 position,Rectangle sourceRectangle, Texture2D texture, Color? primaryColor = null,
             Vector2? origin = null, float? scale = null, float rotation = 0f, Layers layer = Layers.buildings,
            bool randomizeLayers = true, bool flip = false, float? customLayer = null)
        {
            return new Sprite(Graphics, Content,ElementType.World, position, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, rotation, layer, 
                randomizeLayers, flip, customLayer) ;
        }

        /// <summary>
        /// For World Destination Rectangle
        /// </summary>
        public static Sprite CreateWorldSprite(Rectangle destinationRectangle, Rectangle sourceRectangle, Texture2D texture, Color? primaryColor = null,
             Vector2? origin = null, float? scale = null, float rotation = 0f, Layers layer = Layers.buildings, 
            bool randomizeLayers = true, bool flip = false, float? customLayer = null)
        {
            return new Sprite(Graphics, Content, ElementType.World, destinationRectangle, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, rotation, layer,
                randomizeLayers, flip, customLayer);
        }
        /// <summary>
        /// For UI vector2 position
        /// </summary>
        public static Sprite CreateUISprite(Vector2 position, Rectangle sourceRectangle, Texture2D texture, Color? primaryColor = null,
             Vector2? origin = null, float? scale = null, float rotation = 0f, Layers layer = Layers.buildings, 
            bool randomizeLayers = true, bool flip = false, float? customLayer = null)
        {
            return new Sprite(Graphics, Content, ElementType.UI, position, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, rotation, layer, 
                randomizeLayers, flip, customLayer);
        }
        /// <summary>
        /// For UI Destination Rectangle
        /// </summary>
        public static Sprite CreateUISprite(Rectangle destinationRectangle, Rectangle sourceRectangle, Texture2D texture, Color? primaryColor,
             Vector2? origin = null, float? scale = null, float rotation = 0f, Layers layer = Layers.buildings, 
            bool randomizeLayers = true, bool flip = false, float? customLayer = null)
        {
            return new Sprite(Graphics, Content, ElementType.UI, destinationRectangle, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, rotation, layer, 
                randomizeLayers, flip, customLayer);
        }

        public static UINineSliceSprite CreateNineSliceSprite(Vector2 position, int width, int height,
            Texture2D? texture = null, Color? primaryColor = null, Vector2? origin = null, float? scale = null,
            Layers layer = Layers.background)
        {
            NineSlice newNineSlice = new NineSlice(position, texture ?? NineSliceTexture,
                layer, width, height, primaryColor ?? Color.White);


            return new UINineSliceSprite(Graphics, Content, position, newNineSlice, texture ?? NineSliceTexture,
                primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, layer);
        }

        /// <summary>
        /// Nine slice to support text
        /// </summary>
        /// <returns></returns>
        public static UINineSliceSprite CreateNineSliceSprite(Vector2 position, Text text,
            Texture2D? texture = null, Color? primaryColor = null, Vector2? origin = null, float? scale = null,
            Layers layer = Layers.background)
        {
            NineSlice newNineSlice = new NineSlice(position, texture ?? NineSliceTexture,
                layer, text, primaryColor ?? Color.White);


            return new UINineSliceSprite(Graphics, Content, position, newNineSlice, texture ?? NineSliceTexture,
                primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, layer);
        }


        /// <summary>
        /// Animated sprite using RECTANGLES
        /// </summary>

        public static AnimatedSprite CreateWorldAnimatedSprite(Rectangle detinationRectangle, Rectangle startingSourceRectangle, Texture2D texture,
           AnimationFrame[] animationFrames, float standardDuration = .15f, Color? primaryColor = null,
            Vector2? origin = null, float? scale = null, float rotation = 0f, Layers layer = Layers.buildings,
           bool randomizeLayers = true, bool flip = false, float? customLayer = null, int idleFrame = -1)
        {
            return new AnimatedSprite(Graphics, Content, ElementType.World, detinationRectangle, startingSourceRectangle,
                texture, animationFrames, standardDuration, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, rotation, layer,
                randomizeLayers, flip, customLayer, idleFrame);
        }
    }
}
