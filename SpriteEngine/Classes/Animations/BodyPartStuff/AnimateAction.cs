﻿using DataModels;
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
        public void Update(GameTime gameTime, Direction direction, bool hasDirectionChanged, Vector2 position, float layer, bool isMoving, float speedModifier)
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

                    anim.Update(gameTime, position, isMoving, speedModifier);

                }

            }
            else
            {
   
                anim.Update(gameTime, position, true, speedModifier);

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

                animatedSprite.ForceSetPosition(new Vector2(newPos.X, newPos.Y));


            }
        }
        public void Draw(SpriteBatch spriteBatch, Direction direction, bool submerged)
        {

            //Do not draw pants or shoes if player is submerged
            if (submerged)
            {
                Type t = _bodyPiece.GetType();
                if (t == typeof(Shoes) || t == typeof(Pants))
                    return;
            }
           
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
