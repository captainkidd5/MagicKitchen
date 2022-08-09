using DataModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace SpriteEngine.Classes.Animations.BodyPartStuff
{

    public class AnimateAction
    {
        public readonly ActionType ActionType;
        private readonly BodyPiece _bodyPiece;

        private AnimatedSprite[] _animations;
        private AnimatedSprite _currentAnimation;
        private bool _repeat;

        public byte FrameLastFrame => _currentAnimation.FrameLastFrame;
        public bool HasFrameChanged => _currentAnimation.HasFrameChanged();

        public int CurrentFrame => _currentAnimation.CurrentFrame;

        public AnimateAction(ActionType actionType, BodyPiece bodyPiece, AnimatedSprite[] animations, bool repeat)
        {
            ActionType = actionType;
            _bodyPiece = bodyPiece;
            _animations = animations;
            _repeat = repeat;

        }
        public void Update(GameTime gameTime, Direction direction, bool hasDirectionChanged, Vector2 position, float layer, bool isMoving, float speedModifier)
        {
            _currentAnimation = _animations[(int)direction - 1];
            if (_repeat && hasDirectionChanged)
                SetRestingFrame(direction);
            if (_repeat)
            {
               
                if (isMoving)
                {
                    //entity just began moving in a certain direction, start them off on the first non resting frame
                    if (_currentAnimation.IsAtRestingFrame())
                    {
                        _currentAnimation.SetFrame(_currentAnimation.ResetIndex + 1, position);
                    }


                }
                _currentAnimation.Update(gameTime, position, isMoving, speedModifier);

            }
            else
            {
                
                
                _currentAnimation.Update(gameTime, position, true, speedModifier);

            }

            //So that the bodypart overlaps correctly, and is drawn relative to the entity's y position on the map.
            _currentAnimation.CustomLayer = layer;

           

            if (SpriteFactory.PerformActionCustomizeableTriggers.ContainsKey(ActionType))
            if (!_repeat && _currentAnimation.HasLoopedAtLeastOnce && ActionType > ActionType.Walking)
            {
                _currentAnimation.HasLoopedAtLeastOnce = false;
                _bodyPiece.ChangeParentSet(ActionType.Walking);
            }

        }
        public void SetPosition(Vector2 newPos)
        {
            foreach (AnimatedSprite animatedSprite in _animations)
            {

                animatedSprite.ForceSetPosition(new Vector2(newPos.X, newPos.Y));


            }
        }
        public void Draw(SpriteBatch spriteBatch, Direction direction, SubmergenceLevel submergenceLevel)
        {

            //Do not draw pants or shoes if player is submerged
            if (submergenceLevel > SubmergenceLevel.None)
            {
                Type t = _bodyPiece.GetType();
                if (t == typeof(Shoes) || t == typeof(Pants))
                    return;
                if(submergenceLevel > SubmergenceLevel.Shallow)
                {
                    if (t == typeof(Shirt) || t == typeof(Arms) || t == typeof(Shoulders))
                        return;
                }
            }
           
            _animations[(int)direction - 1].Draw(spriteBatch);
          

        }
        public void SetRestingFrame(Direction direction)
        {
           // if (direction != Direction.None)
           // {
           if(ActionType == ActionType.Interact)
                Console.WriteLine("test");
                foreach (AnimatedSprite animatedSprite in _animations)
                {
                    animatedSprite.ResetSpriteToRestingFrame();
  
                }
           // }
        }
        internal virtual void ChangeColor(Color color)
        {

            foreach (AnimatedSprite item in _animations)
            {

                item.UpdateColor(color);


            }

        }
    }
}
