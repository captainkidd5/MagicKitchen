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
        private const int MaxFrames = 100;
        public Interval()
        {
            _timer = new SimpleTimer(.001f);
        }

        public float CurrentTime => _timer.CurrentTime;
        public void Update(GameTime gameTime)
        {
            if (_timer.Run(gameTime))
            {
                if (CurrentFrame >= MaxFrames)
                    CurrentFrame = 0;         
                else
                    CurrentFrame++;
            }
        }
    }
}
