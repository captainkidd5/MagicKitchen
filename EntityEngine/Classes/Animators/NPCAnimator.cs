﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteEngine.Classes;
using SpriteEngine.Classes.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.Animators
{
    internal class NPCAnimator : Animator
    {

        public NPCAnimator(Entity entity, AnimatedSprite[] animatedSprites) : base(entity)
        {
            AnimatedSprites = animatedSprites;
        }

        internal override void Update(GameTime gameTime, bool isMoving, Vector2 position, Direction currentDirection)
        {
            Vector2 positionOffSet = new Vector2(position.X - xOffset, position.Y - yOffset);
            float entityLayer = SpriteUtility.GetYAxisLayerDepth(position, new Rectangle(0, 0, xOffset * 2, yOffset));
            Position = position;
             AnimatedSprites[(int)currentDirection - 1].Update(gameTime, Position);
            

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            
                AnimatedSprites[(int)Entity.DirectionMoving - 1].Draw(spriteBatch);
            
        }
    }
}