using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class LumenHandler
    {
        public int MaxLumens { get; set; } = 100;
        public int CurrentLumens { get; set; } = 100;

        protected HullBody LightSensor { get; set; }

        public bool Illuminated { get; private set; }

        private float _lumenRechargeRate = .5f;
        private SimpleTimer _lumenRechargeTimer;
        public LumenHandler(HullBody lightSensor)
        {
            LightSensor = lightSensor;
            _lumenRechargeTimer = new SimpleTimer(_lumenRechargeRate);
        }
        /// <summary>
        /// If player light sensor overlaps with any light, begin recharging. Recharges at interval, until 
        /// current lumens is equal to max lumens
        /// </summary>
        /// <param name="gameTime"></param>
        public void HandleLumens(GameTime gameTime)
        {
            Illuminated = false;

            if (LightSensor.Body.ContactList != null)
            {
                if (LightSensor.Body.ContactList.Contact.IsTouching)
                    Illuminated = true;
            }

            if (Illuminated)
            {
                RechargeLumens(gameTime);
            }
            else
            {
                DrainLumens(gameTime);
            }
        }

        private void RechargeLumens(GameTime gameTime)
        {
            if (CurrentLumens < MaxLumens)
            {

                if (_lumenRechargeTimer.Run(gameTime))
                {
                    CurrentLumens++;
                }
            }

        }

        private void DrainLumens(GameTime gameTime)
        {
            if (CurrentLumens > 0)
            {

                if (_lumenRechargeTimer.Run(gameTime))
                {
                    CurrentLumens--;
                }
            }

        }
    }
}
