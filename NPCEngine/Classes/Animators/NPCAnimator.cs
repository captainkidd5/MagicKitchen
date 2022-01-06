using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.Animators
{
    internal class NPCAnimator : Animator
    {

        public NPCAnimator(AnimatedSprite[] animatedSprites)
        {
            AnimatedSprites = animatedSprites;
        }

        internal override void Update(GameTime gameTime, bool isMoving, Vector2 position, Direction currentDirection)
        {
            Vector2 positionOffSet = new Vector2(position.X - xOffset, position.Y - yOffset);
            float entityLayer = SpriteUtility.GetYAxisLayerDepth(position, new Rectangle(0, 0, xOffset * 2, yOffset));

            for(int i =0; i < AnimatedSprites.Length; i++)
            {

            }

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
