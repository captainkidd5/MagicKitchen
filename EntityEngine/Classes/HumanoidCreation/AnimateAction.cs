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

        public AnimatedSprite[] Animations { get; set; }
        public bool Repeat { get; }




        public AnimateAction(BodyPiece bodyPiece, AnimatedSprite[] animations, bool repeat)
        {
            _bodyPiece = bodyPiece;
            Animations = animations;
            Repeat = repeat;

        }
        public void Update(GameTime gameTime, Direction direction, bool hasDirectionChanged, Vector2 position, float layer, bool isMoving)
        {
            AnimatedSprite anim = Animations[(int)direction - 1];
            if (Repeat && hasDirectionChanged)
                SetRestingFrame(direction);
            if (Repeat)
            {
                if(isMoving)
                {
                    if (anim.IsAtRestingFrame())
                    {
                        anim.SetFrame(anim.ResetIndex + 1, position);
                    }
                    anim.Update(gameTime, position, isMoving);

                }

            }
            else
            {
                anim.Update(gameTime, position, true);

            }


            //So that the bodypart overlaps correctly, and is drawn relative to the entity's y position on the map.
            anim.CustomLayer = layer;

            if (!Repeat && anim.HasLoopedAtLeastOnce)
            {
                anim.HasLoopedAtLeastOnce = false;

                _bodyPiece.ChangeParentSet(ActionType.Walking);
            }

        }
        public void SetPosition(Vector2 newPos)
        {
            foreach (AnimatedSprite animatedSprite in Animations)
            {

                animatedSprite.ForceSetPosition(new Vector2(newPos.X -8, newPos.Y - 32));


            }
        }
        public void Draw(SpriteBatch spriteBatch, Direction direction)
        {
            Animations[(int)direction - 1].Draw(spriteBatch);
           

        }
        public void SetRestingFrame(Direction direction)
        {
            if (direction != Direction.None)
                Animations[(int)direction - 1].ResetSpriteToRestingFrame();
        }
        internal virtual void ChangeColor(Color color)
        {

            foreach (AnimatedSprite item in Animations)
            {

                item.UpdateColor(color);


            }

        }
    }
}
