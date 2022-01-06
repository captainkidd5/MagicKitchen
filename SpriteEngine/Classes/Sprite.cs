using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes
{
    public class Sprite : BaseSprite
    {


        public Rectangle SourceRectangle { get; private set; }

        public float Rotation { get; set; }

        public float? CustomLayer { get; set; }


        internal Sprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType, Vector2 position, Rectangle sourceRectangle, Texture2D texture, Color primaryColor,
             Vector2 origin, float scale, float rotation, Layers layer,
            bool randomizeLayers, bool flip, float? customLayer) : base(graphics,content, position, texture, primaryColor ,origin,scale,layer)
        {
            SharedConstructor(spriteType, sourceRectangle, rotation, layer, randomizeLayers, flip, customLayer);
        }

  

        internal Sprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType, Rectangle destinationRectangle, Rectangle sourceRectangle, Texture2D texture, Color primaryColor,
             Vector2 origin, float scale, float rotation, Layers layer, 
            bool randomizeLayers, bool flip, float? customLayer) : base(graphics, content, texture,destinationRectangle, primaryColor, origin, scale, layer)
        {
            SharedConstructor(spriteType, sourceRectangle, rotation, layer, randomizeLayers, flip, customLayer);
        }

        private void SharedConstructor(ElementType spriteType, Rectangle sourceRectangle, float rotation, Layers layer, bool randomizeLayers, bool flip, float? customLayer)
        {
            SpriteType = spriteType;
            SourceRectangle = sourceRectangle;


            if (randomizeLayers)
                this.LayerDepth = SpriteUtility.GetSpriteVariedLayerDepth(layer);
            else
                this.LayerDepth = Settings.GetLayerDepth(layer);

            if (customLayer != null)
                CustomLayer = customLayer;
            if (flip)
                SpriteEffectsAnchor = SpriteEffects.FlipHorizontally;
            else
                SpriteEffectsAnchor = SpriteEffects.None;

            Rotation = rotation;
        }
        public void Load(ContentManager contentManager)
        {

        }

        public override void Update(GameTime gameTime, Vector2 position)
        {
            base.Update(gameTime, position);

            Position = position;
        }


        /// <summary>
        /// We only use y axis layer depth if it's a world sprite.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(SpriteType == ElementType.World)
                DrawWorld(spriteBatch);         
            else
                DrawUI(spriteBatch);       
        }

        private void DrawUI(SpriteBatch spriteBatch)
        {
            if (DestinationRectangle == null)
                spriteBatch.Draw(Texture, Position, SourceRectangle, PrimaryColor, Rotation, Origin, Scale, SpriteEffects, CustomLayer ?? LayerDepth);

            else
                spriteBatch.Draw(Texture, (Rectangle)DestinationRectangle, SourceRectangle, PrimaryColor, Rotation, Origin, SpriteEffects, CustomLayer ?? LayerDepth);
        }

        private void DrawWorld(SpriteBatch spriteBatch)
        {
            if (DestinationRectangle == null)
                spriteBatch.Draw(Texture, Position, SourceRectangle, PrimaryColor, Rotation, Origin, Scale, SpriteEffects, CustomLayer ?? GetYAxisLayerDepth());

            else
                spriteBatch.Draw(Texture, (Rectangle)DestinationRectangle, SourceRectangle, PrimaryColor, Rotation, Origin, SpriteEffects, CustomLayer ?? GetYAxisLayerDepth());
        }


            /// <summary>
            /// Returns a layer depth which is relative to how far up or down the sprite is on the map.
            /// Useful for allowing the sprite to move "in front" or "behind of" other objects.
            /// </summary>
            private float GetYAxisLayerDepth()
        {
            return SpriteUtility.GetYAxisLayerDepth(Position, SourceRectangle);
        }
        public void SwapSourceRectangle(Rectangle newRectangle)
        {
            SourceRectangle = newRectangle;
        }

        public void SwapTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public void SwapScale(float scale)
        {
            Scale = scale;
        }
    }
}
