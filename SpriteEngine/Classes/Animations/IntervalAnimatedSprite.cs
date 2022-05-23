using Globals.Classes;
using Globals.Classes.Time;
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
    public class IntervalAnimatedSprite : AnimatedSpriteBase
    {
        //Frames can have different intervals, increment this when the current timer changes but the number of milliseconds isn't right 
        private float _intervalDuration;
        internal IntervalAnimatedSprite(GraphicsDevice graphics, ContentManager content,
            Settings.ElementType spriteType, Vector2 position, Rectangle sourceRectangle,
            Texture2D texture, AnimationFrame[] animationFrames, float standardDuration,
            Color primaryColor, Vector2 origin, Vector2 scale, float rotation, Settings.Layers layer,
            bool randomizeLayers, bool flip, float? customLayer, int idleFrame = 0) : base(graphics, content, spriteType,
                position, sourceRectangle, texture, animationFrames, standardDuration, primaryColor,
                origin, scale, rotation, layer, randomizeLayers, flip, customLayer, idleFrame)
        {
            _intervalDuration = standardDuration;
        }

        public override void Update(GameTime gameTime, Vector2 position, bool updatePeripheralActoins = true)
        {
            base.Update(gameTime, position, updatePeripheralActoins);
            if (updatePeripheralActoins)
            {

                CheckIfIncreaseFrame();

                Position = new Vector2(position.X + AnimationFrames[CurrentFrame].XOffSet, position.Y + AnimationFrames[CurrentFrame].YOffSet * -1);
            }

        }
        public void CheckIfIncreaseFrame()
        {
            if (!Paused)
            {
                byte frame = (byte)(Clock.GetInterval(AnimationFrames[CurrentFrame].Duration).CurrentFrame % AnimationFrames.Length);
                if (frame != CurrentFrame)
                {
                    CurrentFrame = frame;
                    FrameLastFrame = CurrentFrame;

                    UpdateSourceRectangle(AnimationFrames[CurrentFrame]);

                    if (AnimationFrames[CurrentFrame].Flip)
                        SpriteEffects = SpriteEffects.FlipHorizontally;
                    else
                        SetEffectToDefault();
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
