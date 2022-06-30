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

namespace EntityEngine.Classes.HumanoidCreation
{
    internal class AnimateAction
    {
        private readonly BodyPiece _bodyPiece;

        private AnimatedSprite[] _animations;
        private bool _repeat;
        public bool Interruptable => _repeat;



        public AnimateAction(BodyPiece bodyPiece, AnimatedSprite[] animations, bool repeat)
        {
            _bodyPiece = bodyPiece;
            _animations = animations;
            _repeat = repeat;

        }
        public void Update(GameTime gameTime, Direction direction, bool hasDirectionChanged, Vector2 position, float layer, bool isMoving)
        {
            AnimatedSprite anim = _animations[(int)direction - 1];
            if (_repeat && hasDirectionChanged)
                SetRestingFrame(direction);
            if (_repeat)
            {
                if (isMoving)
                {
                    //entity just began moving in a certain direction, start them off on the first non resting frame
                    if (anim.IsAtRestingFrame())
                    {
                        anim.SetFrame(anim.ResetIndex + 1, position);
                    }

                    anim.Update(gameTime, position, isMoving);

                }

            }
            else
            {
                if (_bodyPiece.GetType() == typeof(Arms))
                {
                    if(anim.WillIncrementNextFrame(gameTime))
                     Console.WriteLine("test");
                }
                anim.Update(gameTime, position, true);

            }


            //So that the bodypart overlaps correctly, and is drawn relative to the entity's y position on the map.
            anim.CustomLayer = layer;

            if (!_repeat && anim.HasLoopedAtLeastOnce)
            {
                anim.HasLoopedAtLeastOnce = false;
                anim.ResetSpriteToRestingFrame();
                _bodyPiece.ChangeParentSet(ActionType.Walking);
            }

        }
        public void SetPosition(Vector2 newPos)
        {
            foreach (AnimatedSprite animatedSprite in _animations)
            {

                animatedSprite.ForceSetPosition(new Vector2(newPos.X - 8, newPos.Y - 32));


            }
        }
        public void Draw(SpriteBatch spriteBatch, Direction direction)
        {
        
            _animations[(int)direction - 1].Draw(spriteBatch);
          

        }
        public void SetRestingFrame(Direction direction)
        {
            if (direction != Direction.None)
                _animations[(int)direction - 1].ResetSpriteToRestingFrame();
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
