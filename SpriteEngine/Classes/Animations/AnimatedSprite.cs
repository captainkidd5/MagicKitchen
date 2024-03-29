﻿using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataModels.Enums;

namespace SpriteEngine.Classes.Animations
{
    public class AnimatedSprite : AnimatedSpriteBase
    {
        public bool Repeat { get; set; } = true;
        public AnimatedSprite(GraphicsDevice graphics, ContentManager content, Settings.ElementType spriteType, Vector2 position,
            Rectangle sourceRectangle, Texture2D texture, AnimationFrame[] animationFrames, float standardDuration, Color primaryColor,
            Vector2 origin, Vector2 scale,
            float rotation, Layers layer, bool randomizeLayers, bool flip, float? customLayer, int idleFrame = 0) 
            : base(graphics, content, spriteType, position, sourceRectangle, texture, animationFrames, standardDuration, primaryColor, origin, scale, rotation, layer, randomizeLayers, flip, customLayer, idleFrame)
        {

        }
        public override void ForceSetPosition(Vector2 position)
        {
            Position = new Vector2(position.X + AnimationFrames[CurrentFrame].XOffSet, position.Y + AnimationFrames[CurrentFrame].YOffSet * -1);

        }
        public override void Update(GameTime gameTime, Vector2 position, bool updatePeripheralActoins = true, float speedModifier = 1f)
        {
            base.Update(gameTime, position, updatePeripheralActoins, speedModifier);
            if (updatePeripheralActoins)
            {

                FrameLastFrame = CurrentFrame;
                AnimationFrame frame = AnimationFrames[CurrentFrame];

                if (!Paused && Timer.Run(gameTime))
                {
                    if (TargetFrame == null)
                    {
                        if (Direction == DataModels.Enums.Direction.Right)
                            IncreaseFrames();
                        else
                            DecreaseFrames();
                    }
                    else
                    {
                        GoToTargetFrame();

                    }
                    frame = AnimationFrames[CurrentFrame];


                    UpdateSourceRectangle(frame);

                    Timer.SetNewTargetTime(frame.Duration / speedModifier);
                    if (frame.Flip)
                        SpriteEffects = SpriteEffects.FlipHorizontally;
                    else
                        SetEffectToDefault();
                    if (!Repeat && HasLoopedAtLeastOnce)
                        Paused = true;
                }

            }
            else if(FrameLastFrame != CurrentFrame)
            {
                ResetSpriteToRestingFrame();
            }
            Position = new Vector2(position.X + AnimationFrames[CurrentFrame].XOffSet, position.Y + AnimationFrames[CurrentFrame].YOffSet * -1);

        }

        /// <summary>
        /// Will animate towards the target frame, then return to normal behaviour
        /// </summary>
        private void GoToTargetFrame()
        {
            if (CurrentFrame < TargetFrame)
                IncreaseFrames();
            else if (CurrentFrame > TargetFrame)
                DecreaseFrames();
            else if (CurrentFrame == TargetFrame)
            {
                TargetFrame = null;
                Paused = true;
            }
        }

        public override void ResetSpriteToRestingFrame()
        {
            base.ResetSpriteToRestingFrame();

            Timer.ResetToZero();
        }
    }
}
