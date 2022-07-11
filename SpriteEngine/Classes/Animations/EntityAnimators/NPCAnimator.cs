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
    public class NPCAnimator : Animator
    {
        private AnimatedSprite _currentAnimation;
        public NPCAnimator(AnimatedSprite[] animatedSprites, int? xOffset, int? yOffset)
            : base( xOffset, yOffset)
        {
            AnimatedSprites = animatedSprites;
        }

        public override void Update(GameTime gameTime, Direction directionMoving, bool isMoving, Vector2 position, float speedRatio)
        {
            Vector2 positionOffSet = new Vector2(position.X , position.Y + yOffset /2);
            float entityLayer = SpriteUtility.GetYAxisLayerDepth(position, new Rectangle(0, 0, xOffset * 2, yOffset /2));
            Position = positionOffSet;
            bool resetToResting = !isMoving && WasMovingLastFrame;

            _currentAnimation = AnimatedSprites[(int)directionMoving - 1];
            if (resetToResting)
            {
                _currentAnimation.ResetToZero(Position, entityLayer);
            }
                if (isMoving)
            {
                _currentAnimation.Update(gameTime, Position);

            }
            else
            {
                _currentAnimation.ForceSetPosition(Position);

            }




            WasMovingLastFrame = isMoving;

        }

        public override void Draw(SpriteBatch spriteBatch, bool submerged)
        {
            if(_currentAnimation != null)
            {
                if (submerged)
                    _currentAnimation.SwapSourceRectangle(
                        new Rectangle(_currentAnimation.SourceRectangle.X, _currentAnimation.SourceRectangle.Y,
                        _currentAnimation.SourceRectangle.Width, yOffset/2));
                else
                    _currentAnimation.SwapSourceRectangle(
                     new Rectangle(_currentAnimation.SourceRectangle.X, _currentAnimation.SourceRectangle.Y,
                     _currentAnimation.SourceRectangle.Width, yOffset ));
                _currentAnimation.Draw(spriteBatch);

            }

        }
    }
}
