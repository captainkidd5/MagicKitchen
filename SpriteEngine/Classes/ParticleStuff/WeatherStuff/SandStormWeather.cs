using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff.WeatherStuff
{
    internal class SandStormWeather : Weather
    {
        private float _emitterGenerationInterval = .1f;

        private SimpleTimer _simpleTimer;
        public SandStormWeather()
        {


            ParticleData = new ParticleData()
            {
                lifespan = 6f,
                
                colorStart = Color.White,
                colorEnd = Color.White,
                opacityStart = 1f,
                opacityEnd = 0f,
                sizeStart = 40f,
                sizeEnd = 50f,
                speed = 200f,
                angle = 200f,
                YVelocityMax = 5,
                YVelocityMin = -5,

                SourceRectangle = new Rectangle(0, 32, 32, 32),
                
                EnableGravity = false
            };

            EmitterData = new ParticleEmitterData()
            {
                LifespanMax = 8f,
                LifespanMin = 4f,
                TotalLifeSpan = .4f,
                ParticleData = ParticleData,
                Angle = 300f,
                AngleVariance = 4f,
                SpeedMax = 1000f,
                SpeedMin = 900f
                
            };

            _simpleTimer = new SimpleTimer(_emitterGenerationInterval);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_simpleTimer.Run(gameTime))
            {
                WeatherEmitter weatherEmitter = new WeatherEmitter(GetEmitPosition());
                ParticleManager.AddParticleEmitter(new ParticleEmitter(weatherEmitter, EmitterData, .9f));
            }
        }
        protected override Vector2 GetEmitPosition()
        {
            return new Vector2(Settings.ScreenWidth, Settings.Random.Next(0, Settings.ScreenHeight));
        }
    }
}
