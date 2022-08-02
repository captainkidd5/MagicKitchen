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

        protected readonly ParticleEmitterData Data;
        private readonly float? _customLayer;
        private float _intervalLeft;
        protected float LifeSpanLeft;
        public bool IsFinished => LifeSpanLeft <=0;
        protected readonly IEmitter Emitter;

        public ParticleEmitter(IEmitter emitter, ParticleEmitterData data, float? customLayer = null)
        {
            Emitter = emitter;
            Data = data;
            _customLayer = customLayer;
            _intervalLeft = data.Interval;
            LifeSpanLeft = data.TotalLifeSpan;
        }

        protected virtual void Emit(Vector2 pos)
        {
            ParticleData d = Data.ParticleData;
            d.lifespan = ChanceHelper.RandomFloat(Data.LifespanMin, Data.LifespanMax);
            d.speed = ChanceHelper.RandomFloat(Data.SpeedMin, Data.SpeedMax);
            d.angle = ChanceHelper.RandomFloat(Data.Angle - Data.AngleVariance, Data.Angle + Data.AngleVariance);

            Particle p = new Particle(pos, d, _customLayer);
            ParticleManager.AddParticle(p);
        }

        public virtual void Update(GameTime gameTime)
        {
            _intervalLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_intervalLeft <= 0f)
            {
                _intervalLeft += Data.Interval;
                var pos = Emitter.EmitPosition;
                for (int i = 0; i < Data.EmitCount; i++)
                {
                    Emit(pos);
                }
            }
            LifeSpanLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
    }

}
