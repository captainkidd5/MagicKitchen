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
        private static GraphicsDevice Graphics { get; set; }

        private static ContentManager Content { get; set; }

        public static void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
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
        /// For UI vector2 position.  Note ui sprites are required to pass in a custom layer
        /// </summary>
        public static Sprite CreateUISprite(Vector2 position, Rectangle sourceRectangle, Texture2D texture,float customLayer, Color? primaryColor = null,
             Vector2? origin = null, float? scale = null, float rotation = 0f, 
            bool randomizeLayers = true, bool flip = false)
        {
            return new Sprite(Graphics, Content, ElementType.UI, position, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, rotation, Layers.buildings, 
                randomizeLayers, flip, customLayer);
        }
        /// <summary>
        /// For UI Destination Rectangle. Note ui sprites are required to pass in a custom layer
        /// </summary>
        public static Sprite CreateUISprite(Rectangle destinationRectangle, Rectangle sourceRectangle, Texture2D texture,float customLayer, Color? primaryColor,
             Vector2? origin = null, float? scale = null, float rotation = 0f, 
            bool randomizeLayers = true, bool flip = false)
        {
            return new Sprite(Graphics, Content, ElementType.UI, destinationRectangle, sourceRectangle, texture, primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, rotation, Layers.buildings, 
                randomizeLayers, flip, customLayer);
        }

        public static NineSliceSprite CreateNineSliceSprite(Vector2 position, int width, int height,
            Texture2D texture,float layer, Color? primaryColor = null, Vector2? origin = null, float? scale = null)
        {
            NineSlice newNineSlice = new NineSlice(position, texture,
                layer, width, height, primaryColor ?? Color.White);


            return new NineSliceSprite(Graphics, Content, position, newNineSlice, texture,
                primaryColor ?? Color.White, origin ?? Vector2.Zero, scale ?? Settings.GameScale, layer);
        }

        /// <summary>
        /// Nine slice to support text
        /// </summary>
        /// <returns></returns>
        public static NineSliceSprite CreateNineSliceSprite(Vector2 position, Text text,
            Texture2D texture, Color? primaryColor = null, Vector2? origin = null, float? scale = null,
            Layers layer = Layers.background)
        {
            NineSlice newNineSlice = new NineSlice(position, texture,
                layer, text, primaryColor ?? Color.White);


            return new NineSliceSprite(Graphics, Content, position, newNineSlice, texture,
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
