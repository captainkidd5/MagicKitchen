using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes
{
    /// <summary>
    /// A ... simple timer class
    /// </summary>
    public class SimpleTimer
    {
        private float _currentTime;
        /// <summary>
        /// If true, will reset to zero every time the target time is reached.
        /// </summary>
        private bool RunContinuously { get; set; }
        public float TargetTime { get; private set; }

        public SimpleTimer(float targetTime, bool runContinuously = true)
        {
            this.TargetTime = targetTime;
            RunContinuously = runContinuously;
        }

        /// <summary>
        /// Returns true if time has reached target time.
        /// </summary>
        public bool Run(GameTime gameTime, float multiplier = 1f)
        {
            bool isDone = Test();

            if (!isDone)
                _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds * multiplier;


            return isDone;
        }

        public void ResetToZero()
        {
            _currentTime = 0f;
        }

        public void SetNewTargetTime(float targetTime)
        {
            TargetTime = targetTime;
        }

        private bool Test()
        {
            if (_currentTime >= TargetTime)
            {
                if(RunContinuously)
                    ResetToZero();
                return true;
            }
            return false;
        }
    }
}
