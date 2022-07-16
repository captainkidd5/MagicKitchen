using Globals.Classes.Chance;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff
{
    public class ParticleEmitter
    {

        private readonly ParticleEmitterData _data;
        private float _intervalLeft;
        private float _lifeSpanLeft;
        public bool IsFinished => _lifeSpanLeft <=0;
        private readonly IEmitter _emitter;

        public ParticleEmitter(IEmitter emitter, ParticleEmitterData data)
        {
            _emitter = emitter;
            _data = data;
            _intervalLeft = data.Interval;
            _lifeSpanLeft = data.TotalLifeSpan;
        }

        private void Emit(Vector2 pos)
        {
            ParticleData d = _data.ParticleData;
            d.lifespan = ChanceHelper.RandomFloat(_data.LifespanMin, _data.LifespanMax);
            d.speed = ChanceHelper.RandomFloat(_data.SpeedMin, _data.SpeedMax);
            d.angle = ChanceHelper.RandomFloat(_data.Angle - _data.AngleVariance, _data.Angle + _data.AngleVariance);

            Particle p = new Particle(pos, d);
            ParticleManager.AddParticle(p);
        }

        public void Update(GameTime gameTime)
        {
            _intervalLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_intervalLeft <= 0f)
            {
                _intervalLeft += _data.Interval;
                var pos = _emitter.EmitPosition;
                for (int i = 0; i < _data.EmitCount; i++)
                {
                    Emit(pos);
                }
            }
            _lifeSpanLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
    }

}
