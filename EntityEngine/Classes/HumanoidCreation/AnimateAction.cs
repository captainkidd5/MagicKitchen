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
        public AnimatedSprite[] Animations { get; set; }
        public bool Repeat { get; }



        
        public AnimateAction(AnimatedSprite[] animations, bool repeat)
        {
            Animations = animations;
            Repeat = repeat;
   
        }
        public void Update(GameTime gameTime, Direction direction,bool hasDirectionChanged, Vector2 position, float layer, bool isMoving)
        {
            if (hasDirectionChanged)
                SetRestingFrame(direction);

            Animations[(int)direction -1].Update(gameTime, position, isMoving);

            //So that the bodypart overlaps correctly, and is drawn relative to the entity's y position on the map.
            Animations[(int)direction - 1].CustomLayer = layer;
        }
        public void Draw(SpriteBatch spriteBatch, Direction direction)
        {
            Animations[(int)direction - 1].Draw(spriteBatch);

        }
        public void SetRestingFrame(Direction direction)
        {
            Animations[(int)direction -1].ResetSpriteToRestingFrame();
        }
        internal virtual void ChangeColor(Color color)
        {

            foreach (var item in Animations)
            {

                item.UpdateColor(color);


            }

        }
    }
}
