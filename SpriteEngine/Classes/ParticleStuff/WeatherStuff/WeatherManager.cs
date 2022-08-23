using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff.WeatherStuff
{
    public class WeatherManager
    {
        public Weather CurrentWeather;
        public void Update(GameTime gameTime)
        {
            CurrentWeather.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentWeather.Draw(spriteBatch);
        }
    }
}
