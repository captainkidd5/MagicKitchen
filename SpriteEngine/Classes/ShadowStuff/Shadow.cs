using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ShadowStuff
{
    public enum ShadowSize
    {
        None =0,
        Small = 1,
        Medium = 2
    }
    public class Shadow
    {
        private static Rectangle s_smallRectangle = new Rectangle(32, 48, 16, 16);

        public Sprite Sprite { get; set; }
        
        public Shadow(Vector2 position, ShadowSize shadowSize, Texture2D texture, float layerDepth)
        {
            Sprite = SpriteFactory.CreateWorldSprite(position, s_smallRectangle, texture, customLayer: layerDepth);
        }
        public void Update(GameTime gameTime, Vector2 position)
        {
            Sprite.Update(gameTime, position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
