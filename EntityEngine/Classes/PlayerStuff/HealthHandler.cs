using DataModels.ItemStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using SoundEngine.Classes;
using SpriteEngine.Classes.ParticleStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityEngine.Classes.PlayerStuff
{
    internal class HealthHandler : ISaveable
    {
        public byte MaxHealth { get; set; } = 100;
        public byte CurrentHealth { get; set; } = 100;

        //zero hunger will not bring player below x% health
        private float _percentPlayerNoLongerTakesHungerDamage = .2f;

        private static float _baseHealthRegenRate = 5f;
        private float _currentHealthRegenRate;
        private SimpleTimer _healthRegenTimer;

        private Player _player;
        private HungerHandler _hungerHandler;
        public HealthHandler()
        {
        }
        public void Load(Player player, HungerHandler hungerHandler)
        {
            _player = player;
            _currentHealthRegenRate = _baseHealthRegenRate;

            _healthRegenTimer = new SimpleTimer(_currentHealthRegenRate);
            _hungerHandler = hungerHandler;
        }

        public void HandleHealth(GameTime gameTime)
        {

            if (_healthRegenTimer.Run(gameTime))
            {
                if (_hungerHandler.CurrentHunger > 0 &&CurrentHealth < MaxHealth)
                {

                    _hungerHandler.CurrentHunger--;
                    CurrentHealth++;
                }
                else if(_hungerHandler.CurrentHunger <= 0)
                {
                    if(CurrentHealth > MaxHealth * _percentPlayerNoLongerTakesHungerDamage) 
                     _player.TakeDamage(null, 5);

                }
            }

        }




        public void Save(BinaryWriter writer)
        {
            writer.Write(CurrentHealth);
            writer.Write(MaxHealth);
        }

        public void LoadSave(BinaryReader reader)
        {
            CurrentHealth = reader.ReadByte();
            MaxHealth = reader.ReadByte();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void SetToDefault()
        {
            CurrentHealth = MaxHealth;
            _currentHealthRegenRate = _baseHealthRegenRate;
        }
    }
}
