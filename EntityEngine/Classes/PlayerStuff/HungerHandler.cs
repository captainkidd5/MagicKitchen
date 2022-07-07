using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class HungerHandler : ISaveable
    {
        public ushort MaxHunger { get; set; } = 100;
        public ushort CurrentHunger { get; set; } = 100;



        private static float _baseHungerDrainRate = .5f;
        private float _currentHungerDrainRate = .5f;
        private SimpleTimer _hungerRechargeTimer;

        public HungerHandler()
        {
        }
        public void Load()
        {
            _hungerRechargeTimer = new SimpleTimer(_currentHungerDrainRate);

        }

        public void HandleHunger(GameTime gameTime)
        {

            DrainHunger(gameTime);


        }



        private void DrainHunger(GameTime gameTime)
        {
            if (CurrentHunger > 0)
            {

                if (_hungerRechargeTimer.Run(gameTime))
                {
                    CurrentHunger--;
                }
            }

        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(CurrentHunger);
            writer.Write(MaxHunger);
        }

        public void LoadSave(BinaryReader reader)
        {
            CurrentHunger = reader.ReadUInt16();
            MaxHunger = reader.ReadUInt16();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void SetToDefault()
        {
            CurrentHunger = MaxHunger;
            _currentHungerDrainRate = _baseHungerDrainRate;
        }
    }
}
