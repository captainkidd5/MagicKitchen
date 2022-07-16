using Globals.Classes.Chance;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff.TextParticleStuff
{
    public class TextParticleEmitter : ParticleEmitter
    {
        private string _text;
        public TextParticleEmitter(string text, IEmitter emitter, ParticleEmitterData data) : base(emitter, data)
        {
            _text = text;
        }

        protected override void Emit(Vector2 pos)
        {
            ParticleData d = Data.ParticleData;
            d.lifespan = ChanceHelper.RandomFloat(Data.LifespanMin, Data.LifespanMax);
            d.speed = ChanceHelper.RandomFloat(Data.SpeedMin, Data.SpeedMax);
            d.angle = ChanceHelper.RandomFloat(Data.Angle - Data.AngleVariance, Data.Angle + Data.AngleVariance);

            TextParticle p = new TextParticle(_text, pos, d);
            ParticleManager.AddParticle(p);
        }
        public override void Update(GameTime gameTime)
        {
            Emit(Emitter.EmitPosition);

            LifeSpanLeft = -1;
        }
    }
}
