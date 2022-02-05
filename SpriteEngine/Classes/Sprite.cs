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
        public Sprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType, Vector2 position,
            Rectangle sourceRectangle, Texture2D texture, Color primaryColor, Vector2 origin, float scale,
            float rotation, Layers layer, bool randomizeLayers, bool flip, float? customLayer)
            : base(graphics, content, spriteType, position, sourceRectangle, texture, primaryColor, origin, scale, rotation, randomizeLayers, flip, customLayer)
        {
            CheckIfRandomizeLayers(layer, randomizeLayers);

        }

        public Sprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType, Rectangle destinationRectangle,
            Rectangle sourceRectangle, Texture2D texture, Color primaryColor, Vector2 origin, float scale, float rotation,
            Layers layer, bool randomizeLayers, bool flip, float? customLayer)
            : base(graphics, content, spriteType, destinationRectangle, sourceRectangle, texture, primaryColor, origin, scale, rotation, randomizeLayers, flip, customLayer)
        {
            CheckIfRandomizeLayers(layer, randomizeLayers);
        }

        private void CheckIfRandomizeLayers(Layers layer, bool randomizeLayers)
        {
            if (randomizeLayers)
                this.LayerDepth = SpriteUtility.GetSpriteVariedLayerDepth(layer);
            else
                this.LayerDepth = Settings.GetLayerDepth(layer);
        }

        public void Load(ContentManager contentManager)
        {

        }

        public override void Update(GameTime gameTime, Vector2 position, bool updatePeripheralActoins = true)
        {
            base.Update(gameTime, position, updatePeripheralActoins);

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


         
    }
}
