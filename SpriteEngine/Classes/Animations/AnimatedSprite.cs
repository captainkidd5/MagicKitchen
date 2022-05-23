﻿using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.Animations
{
    public class AnimatedSprite : AnimatedSpriteBase
    {
        private SimpleTimer Timer { get; set; }
        public bool Repeat { get; set; } = true;
        internal AnimatedSprite(GraphicsDevice graphics, ContentManager content, Settings.ElementType spriteType, Vector2 position,
            Rectangle sourceRectangle, Texture2D texture, AnimationFrame[] animationFrames, float standardDuration, Color primaryColor,
            Vector2 origin, Vector2 scale,
            float rotation, Settings.Layers layer, bool randomizeLayers, bool flip, float? customLayer, int idleFrame = 0) 
            : base(graphics, content, spriteType, position, sourceRectangle, texture, animationFrames, standardDuration, primaryColor, origin, scale, rotation, layer, randomizeLayers, flip, customLayer, idleFrame)
        {
            Timer = new SimpleTimer(animationFrames[0].Duration);

        }
        public override void Update(GameTime gameTime, Vector2 position, bool updatePeripheralActoins = true)
        {
            base.Update(gameTime, position, updatePeripheralActoins);
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



                    UpdateSourceRectangle(frame);

                    Timer.SetNewTargetTime(frame.Duration);
                    if (frame.Flip)
                        SpriteEffects = SpriteEffects.FlipHorizontally;
                    else
                        SetEffectToDefault();
                    if (!Repeat && HasLoopedAtLeastOnce)
                        Paused = true;
                }

                Position = new Vector2(position.X + frame.XOffSet, position.Y + frame.YOffSet * -1);
            }
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
    }
}
