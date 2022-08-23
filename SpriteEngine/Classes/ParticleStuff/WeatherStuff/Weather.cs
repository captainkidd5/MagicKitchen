using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff.WeatherStuff
{
    public abstract class Weather
    {
       private List<WeatherEmitter> _emitters;
        protected ParticleEmitterData EmitterData { get; set; }
        protected ParticleData ParticleData { get; set; }

        public Weather()
        {

        }
        public virtual void Update(GameTime gameTime)
        {
            //for(int i = _emitters.Count - 1; i >= 0; i--)
            //{
            //    WeatherEmitter emitter = _emitters[i];
            //}
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //for (int i = _emitters.Count - 1; i >= 0; i--)
            //{
            //    WeatherEmitter emitter = _emitters[i];
            //}
        }
        protected virtual Vector2 GetEmitPosition()
        {
            return Vector2.Zero;
        }
    }
}
