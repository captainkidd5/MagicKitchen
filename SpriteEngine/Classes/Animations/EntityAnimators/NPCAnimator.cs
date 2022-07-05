using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.Animations.EntityAnimations
{
    internal class NPCAnimator : Animator
    {

        public NPCAnimator(AnimatedSprite[] animatedSprites, int? xOffset, int? yOffset)
            : base( xOffset, yOffset)
        {
            AnimatedSprites = animatedSprites;
        }

        internal override void Update(GameTime gameTime, Direction directionMoving, bool isMoving, Vector2 position, float speedRatio)
        {
            Vector2 positionOffSet = new Vector2(position.X - xOffset, position.Y - yOffset);
            float entityLayer = SpriteUtility.GetYAxisLayerDepth(position, new Rectangle(0, 0, xOffset * 2, yOffset));
            Position = position;
            bool resetToResting = !isMoving && WasMovingLastFrame;
            if (resetToResting)
            {
                AnimatedSprites[(int)directionMoving - 1].ResetToZero(Position, entityLayer);
            }
                if (isMoving)
            {
                AnimatedSprites[(int)directionMoving - 1].Update(gameTime, Position);

            }
            else
            {
                AnimatedSprites[(int)directionMoving - 1].ForceSetPosition(Position);

            }




            WasMovingLastFrame = isMoving;

        }

        internal override void Draw(SpriteBatch spriteBatch, bool submerged)
        {

            AnimatedSprites[(int)Entity.DirectionMoving - 1].Draw(spriteBatch);

        }
    }
}
