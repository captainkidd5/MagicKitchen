using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff.WeatherStuff
{
    public enum WeatherType
    {
        None = 0,
        SandStorm = 1,
    }
    public static class WeatherManager
    {
        public static Weather CurrentWeather;

        public static void SetWeather(WeatherType weatherType)
        {
            switch (weatherType)
            {
                case WeatherType.None:
                    break;
                case WeatherType.SandStorm:
                    CurrentWeather = new SandStormWeather();
                    break;
            }
        }
        public static void Update(GameTime gameTime)
        {
            if(CurrentWeather != null)
            CurrentWeather.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentWeather != null)
                CurrentWeather.Draw(spriteBatch);
        }
    }

}