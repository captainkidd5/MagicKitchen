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
    public class IntervalAnimatedSprite : AnimatedSpriteBase, ITimerSubscribeable
    {
        //Frames can have different intervals, increment this when the current timer changes but the number of milliseconds isn't right 
        private int _currentFrameBuildup;
        private int _intervalDuration;
        internal IntervalAnimatedSprite(GraphicsDevice graphics, ContentManager content,
            Settings.ElementType spriteType, Vector2 position, Rectangle sourceRectangle,
            Texture2D texture, AnimationFrame[] animationFrames, float standardDuration,
            Color primaryColor, Vector2 origin, Vector2 scale, float rotation, Settings.Layers layer,
            bool randomizeLayers, bool flip, float? customLayer, int idleFrame = -1) : base(graphics, content, spriteType,
                position, sourceRectangle, texture, animationFrames, standardDuration, primaryColor,
                origin, scale, rotation, layer, randomizeLayers, flip, customLayer, idleFrame)
        {
            _intervalDuration = (int)standardDuration;

            if (_intervalDuration % 100 != 0)
                throw new Exception($"duration must be a multiple of 100");

            Clock.SubscribeToInterval(this);


        }

        public override void Update(GameTime gameTime, Vector2 position, bool updatePeripheralActoins = true)
        {
            base.Update(gameTime, position, updatePeripheralActoins);
            if (updatePeripheralActoins)
            {



                Position = new Vector2(position.X + AnimationFrames[CurrentFrame].XOffSet, position.Y + AnimationFrames[CurrentFrame].YOffSet * -1);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public void TimerFrameChanged(object sender, EventArgs args)
        {


            if (!Paused)
            {
                if (AnimationFrames[CurrentFrame].Duration > _intervalDuration)
                {
                    _currentFrameBuildup += _intervalDuration;
                    return;
                }
                else
                    _currentFrameBuildup = 0;


                IncreaseFrames();
                FrameLastFrame = CurrentFrame;
                AnimationFrame frame = AnimationFrames[CurrentFrame];

                UpdateSourceRectangle(frame);

                if (frame.Flip)
                    SpriteEffects = SpriteEffects.FlipHorizontally;
                else
                    SetEffectToDefault();
            }
        }
    }
}
