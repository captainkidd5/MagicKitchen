using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteEngine.Classes.Animations
{
    public class AnimationSet
    {
        private AnimatedSprite[] AnimatedSprites { get; set; }

        public AnimationSet(AnimatedSprite[] animatedSprites)
        {
            AnimatedSprites = animatedSprites;
        }

        public void Update(GameTime gameTime, Vector2 position) 
        {
            for(int i=0; i < AnimatedSprites.Length; i++)
            {
                AnimatedSprites[i].Update(gameTime, position);
            }
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            for (int i = 0; i < AnimatedSprites.Length; i++)
            {
                AnimatedSprites[i].Draw(spriteBatch);
            }
        }
    }
}
