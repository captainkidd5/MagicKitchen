using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.Animations
{
    public class AnimatedSprite : Sprite
    {
        private int TotalFrames { get; set; }
        private int CurrentFrame { get; set; }
        public int FrameLastFrame { get; private set; }
        private SimpleTimer Timer { get; set; }
        private AnimationFrame[] AnimationFrames { get; set; }
        private bool Flip { get; set; }

        /// <summary>
        /// When animation has reached final frame, will circle around to this one. Default is -1 if you don' have one, but
        /// A scenario where it would be higher would be something like a player animation
        /// where the zero-th frame is actually the idle position
        /// </summary>
        private int ResetIndex { get; set; }
        private int SpriteSourceRectangleStartX { get; set; } //we need these two properties because otherwise altering
        //the source rectangle of the sprite will be additive, and its value will always grow or decrease forever.
        private int SpriteSourceRectangleStartY { get; set; }

        /// <summary>
        /// <see cref="DestructableTile"/>
        /// </summary>
        public bool Paused { get; set; }
        /// <summary>
        /// <see cref="DestructableTile"/>
        /// </summary>
        public bool HasLoopedAtLeastOnce { get; set; }

        /// <summary>
        /// This should be the idle frame sprite
        /// </summary>
        /// <param name="animationFrames">Must be at least 1 frame!</param>
        /// <param name="standardDuration">How long each frame should run for.</param>
        /// <param name="flip">Enable if the source rectangle should be mirrored.</param>
        internal AnimatedSprite(GraphicsDevice graphics, ContentManager content, ElementType spriteType,
            Rectangle detinationRectangle, Rectangle sourceRectangle, Texture2D texture, AnimationFrame[] animationFrames, float standardDuration, Color primaryColor,
             Vector2 origin, float scale, float rotation, Layers layer,
            bool randomizeLayers, bool flip, float? customLayer, int idleFrame =-1) :
            base(graphics, content, spriteType, detinationRectangle, sourceRectangle, texture, primaryColor, origin, scale, rotation,
                layer, randomizeLayers, flip, customLayer)
        {
            AnimationFrames = animationFrames;

            Flip = flip;
            SpriteSourceRectangleStartX = SourceRectangle.X;
            SpriteSourceRectangleStartY = SourceRectangle.Y;
            TotalFrames = AnimationFrames.Length - 1;
            ResetIndex = idleFrame;
            if (TotalFrames < 1)
                throw new Exception("total frames must exceed 0");

            Timer = new SimpleTimer(animationFrames[0].Duration);
        }

        public bool HasFrameChanged()
        {
            return CurrentFrame != FrameLastFrame;
        }
        public override void Update(GameTime gameTime, Vector2 position)
        {
            base.Update(gameTime, position);
            FrameLastFrame = CurrentFrame;
            AnimationFrame frame = AnimationFrames[CurrentFrame];
            
            if (!Paused && Timer.Run(gameTime))
            {
                IncreaseFrames();

                UpdateSourceRectangle(frame);

                Timer.SetNewTargetTime(frame.Duration);
                if (frame.Flip)
                    SpriteEffects = SpriteEffects.FlipHorizontally;
                else
                   SetEffectToDefault();
            }

            Position = new Vector2(position.X + frame.XOffSet, position.Y + frame.YOffSet * -1);
            DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, SourceRectangle.Width, SourceRectangle.Height);
        }




        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void UpdateSourceRectangle(AnimationFrame frame)
        {
            if (frame.SourceRectangle != null)
            {
                SwapSourceRectangle((Rectangle)frame.SourceRectangle);
            }
            else
            {
                base.SwapSourceRectangle(new Rectangle(
                                   SpriteSourceRectangleStartX + SourceRectangle.Width * frame.SpriteIndex,
                                   SpriteSourceRectangleStartY,
                                   SourceRectangle.Width,
                                   SourceRectangle.Height));
            }        
        }
        private void IncreaseFrames()
        {
            if (CurrentFrame >= TotalFrames)
            {
                HasLoopedAtLeastOnce = true;
                CurrentFrame = ResetIndex + 1;
            }
            else
                CurrentFrame++;
        }

        public void ForceSetFrame(Vector2 position, float layer)
        {
            ResetSpriteToRestingFrame();
            CustomLayer = layer; 
            Position = new Vector2(position.X + AnimationFrames[0].XOffSet, position.Y + AnimationFrames[0].YOffSet * -1);
            if (DestinationRectangle != null)
                DestinationRectangle = new Rectangle((int)Position.X + (int)Origin.X, (int)Position.Y + (int)Origin.Y, Width, Height);
        }
        /// <summary>
        /// Set animations to their default position. E.x. when the player stops running. Default is zero
        /// </summary>
        public void ResetSpriteToRestingFrame( )
        {

            
            if (ResetIndex < 0)
            {
                UpdateSourceRectangle(AnimationFrames[0]);
                return;
            }


            UpdateSourceRectangle(AnimationFrames[ResetIndex]);

        }
    }
}
