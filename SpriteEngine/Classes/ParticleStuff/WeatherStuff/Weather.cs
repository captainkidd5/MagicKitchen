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
        public void Update(GameTime gameTime)
        {
            for(int i = _emitters.Count - 1; i >= 0; i--)
            {
                WeatherEmitter emitter = _emitters[i];
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = _emitters.Count - 1; i >= 0; i--)
            {
                WeatherEmitter emitter = _emitters[i];
            }
        }
    }
}
