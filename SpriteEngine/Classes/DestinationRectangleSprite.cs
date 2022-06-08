using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace SpriteEngine.Classes
{
    public class DestinationRectangleSprite : Sprite
    {
        public int RectangleWidth { get; set; }
        public int RectangleHeight { get; set; }


        internal DestinationRectangleSprite(int width, int height, GraphicsDevice graphics, ContentManager content,
            Settings.ElementType spriteType, Vector2 position, Rectangle sourceRectangle,
            Texture2D texture, Color primaryColor, Vector2 origin, Vector2 scale, float rotation,
            Layers layer, bool randomizeLayers, bool flip, float? customLayer) :
            base(graphics, content, spriteType, position, sourceRectangle, texture, primaryColor,
                origin, scale, rotation, layer, randomizeLayers, flip, customLayer)
        {
            RectangleWidth = width;
            RectangleHeight = height;
        }

        protected override void DrawUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)(Position.X + OffSet.X), (int)(Position.Y + OffSet.Y), RectangleWidth, RectangleHeight), SourceRectangle,
               PrimaryColor, Rotation, Origin, SpriteEffects, CustomLayer ?? LayerDepth);

        }

        protected override void DrawWorld(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,new Rectangle((int)(Position.X + OffSet.X), (int)(Position.Y + OffSet.Y), RectangleWidth, RectangleHeight), SourceRectangle,
                PrimaryColor, Rotation, Origin, SpriteEffects, CustomLayer ?? GetYAxisLayerDepth());

        }

    
    }
}
