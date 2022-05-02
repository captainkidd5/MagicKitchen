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
        internal Sprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType, Vector2 position,
            Rectangle sourceRectangle, Texture2D texture, Color primaryColor, Vector2 origin, Vector2 scale,
            float rotation, Layers layer, bool randomizeLayers, bool flip, float? customLayer)
            : base(graphics, content, spriteType, position, sourceRectangle, texture, primaryColor, origin, scale, rotation, randomizeLayers, flip, customLayer)
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

        protected virtual void DrawUI(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(Texture,  Position + OffSet, SourceRectangle, PrimaryColor, Rotation, Origin, Scale, SpriteEffects, CustomLayer ?? LayerDepth);


        }

        protected virtual void DrawWorld(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(Texture, Position + OffSet, SourceRectangle, PrimaryColor, Rotation, Origin, Scale, SpriteEffects, CustomLayer ?? GetYAxisLayerDepth());

        }


         
    }
}
