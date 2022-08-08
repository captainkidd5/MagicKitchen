using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace SpriteEngine.Classes.Animations
{
    public abstract class AnimatedSpriteBase : Sprite
    {
        protected byte TotalFrames { get; set; }
        public byte CurrentFrame { get; protected set; }
        public Direction Direction { get; set; } = Direction.Right;
        public byte FrameLastFrame { get; protected set; }
        public AnimationFrame[] AnimationFrames { get; protected set; }
        private bool Flip { get; set; }

        protected byte? TargetFrame { get; set; }
        /// <summary>
        /// If true, animation will play backwards upon reaching end frame, then forwards upon reaching start frame
        /// </summary>
        public bool PingPong { get; set; }

        /// <summary>
        /// When animation has reached final frame, will circle around to this one. Default is -1 if you don' have one, but
        /// A scenario where it would be higher would be something like a player animation
        /// where the zero-th frame is actually the idle position
        /// </summary>
        public byte ResetIndex { get; set; }
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


        protected SimpleTimer Timer { get; set; }

        /// <summary>
        /// This should be the idle frame sprite
        /// </summary>
        /// <param name="animationFrames">Must be at least 1 frame!</param>
        /// <param name="standardDuration">How long each frame should run for.</param>
        /// <param name="flip">Enable if the source rectangle should be mirrored.</param>
        internal AnimatedSpriteBase(GraphicsDevice graphics, ContentManager content, ElementType spriteType,
            Vector2 position, Rectangle sourceRectangle, Texture2D texture, AnimationFrame[] animationFrames, float standardDuration, Color primaryColor,
             Vector2 origin, Vector2 scale, float rotation, Layers layer,
            bool randomizeLayers, bool flip, float? customLayer, int idleFrame = 0) :
            base(graphics, content, spriteType, position, sourceRectangle, texture, primaryColor, origin, scale, rotation,
                layer, randomizeLayers, flip, customLayer)
        {
            AnimationFrames = animationFrames;

            Flip = flip;
            SpriteSourceRectangleStartX = SourceRectangle.X;
            SpriteSourceRectangleStartY = SourceRectangle.Y;
            TotalFrames = (byte)(AnimationFrames.Length - 1);
            ResetIndex = (byte)idleFrame;

            if (TotalFrames < 1)
                throw new Exception("total frames must exceed 0");
            Timer = new SimpleTimer(animationFrames[0].Duration);

        }

        public bool HasFrameChanged()
        {
            return CurrentFrame != FrameLastFrame;
        }
        public bool WillIncrementNextFrame(GameTime gameTime)
        {
            return Timer.WillIncrement(gameTime);
        }

        public override void Update(GameTime gameTime, Vector2 position, bool updatePeripheralActoins = true, float speedModifier = 1f)
        {
         
            base.Update(gameTime, position, updatePeripheralActoins, speedModifier);



        }
        public bool IsAtRestingFrame()
        {
            return CurrentFrame == ResetIndex;
        }
        public void SetFrame(int frame, Vector2 position)
        {
            CurrentFrame = (byte)frame;
            FrameLastFrame = CurrentFrame;
            UpdateSourceRectangle(AnimationFrames[CurrentFrame]);
            Timer.SetNewTargetTime(AnimationFrames[CurrentFrame].Duration);

            Position = new Vector2(position.X + AnimationFrames[CurrentFrame].XOffSet, position.Y + AnimationFrames[CurrentFrame].YOffSet * -1);


        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected void UpdateSourceRectangle(AnimationFrame frame)
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
        protected void IncreaseFrames()
        {
            if (CurrentFrame >= TotalFrames)
            {
                HasLoopedAtLeastOnce = true;
                if (PingPong)
                    Direction = Direction.Left;
                else
                    CurrentFrame = (byte)(ResetIndex + 1);
            }
            else
                CurrentFrame++;
        }
        protected void DecreaseFrames()
        {
            if (CurrentFrame <= 0)
            {
                HasLoopedAtLeastOnce = true;

                if (PingPong)
                    Direction = Direction.Right;
                else
                    CurrentFrame = (byte)(TotalFrames - 1);
            }
            else
                CurrentFrame--;
        }
        public void SetTargetFrame(int frame, bool force = false)
        {
            TargetFrame = (byte)frame;
            if (force)
            {
                CurrentFrame = (byte)TargetFrame.Value;

                UpdateSourceRectangle(AnimationFrames[CurrentFrame]);

            }
        }
        public void ResetToZero(Vector2 position, float layer)
        {
            ResetSpriteToRestingFrame();
            CustomLayer = layer;
            Position = new Vector2(position.X + AnimationFrames[0].XOffSet, position.Y + AnimationFrames[0].YOffSet * -1);

        }
        /// <summary>
        /// Set animations to their default position. E.x. when the player stops running. Default is zero
        /// </summary>
        public virtual void ResetSpriteToRestingFrame()
        {

            //Remember, reset index is defautled to -1, which most sprites use
            if (ResetIndex < 0 || ResetIndex == byte.MaxValue)
            {
                UpdateSourceRectangle(AnimationFrames[0]);
                CurrentFrame = 0;
                FrameLastFrame = CurrentFrame;

                return;
            }
            CurrentFrame = (byte)(ResetIndex);
            UpdateSourceRectangle(AnimationFrames[CurrentFrame]);
            FrameLastFrame = CurrentFrame;

        }
    }
}
