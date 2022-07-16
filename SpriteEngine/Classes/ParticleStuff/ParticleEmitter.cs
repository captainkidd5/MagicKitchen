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
    private readonly IEmitter _emitter;

    public ParticleEmitter(IEmitter emitter, ParticleEmitterData data)
    {
        _emitter = emitter;
        _data = data;
        _intervalLeft = data.interval;
    }

    private void Emit(Vector2 pos)
    {
        ParticleData d = _data.particleData;
        d.lifespan = ChanceHelper.RandomFloat(_data.lifespanMin, _data.lifespanMax);
        d.speed = ChanceHelper.RandomFloat(_data.speedMin, _data.speedMax);
        d.angle = ChanceHelper.RandomFloat(_data.angle - _data.angleVariance, _data.angle + _data.angleVariance);

        Particle p = new(pos, d);
        ParticleManager.AddParticle(p);
    }

    public void Update(GameTime gameTime)
    {
        _intervalLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        while (_intervalLeft <= 0f)
        {
            _intervalLeft += _data.interval;
            var pos = _emitter.EmitPosition;
            for (int i = 0; i < _data.emitCount; i++)
            {
                Emit(pos);
            }
        }
    }
    }

}
