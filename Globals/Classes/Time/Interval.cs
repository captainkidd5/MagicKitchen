using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Classes.Time
{
    /// <summary>
    /// A class with an event that fires to signify a certain interval is reached. Good for tile animations
    /// </summary>
    public class Interval
    {
        private SimpleTimer _timer;



        public int CurrentFrame { get; private set; } = 0;
        public Interval()
        {
            _timer = new SimpleTimer(1f);
        }

        public void Update(GameTime gameTime)
        {
            if (_timer.Run(gameTime))
            {

                    CurrentFrame++;
            }
        }
    }
}
