using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff
{
    public struct ParticleEmitterData
    {
        public ParticleData ParticleData = new();
        public float Angle = 0f;
        public float AngleVariance = 80f;
        public float LifespanMin = 0.1f;
        public float LifespanMax = .4f;
        public float SpeedMin = 60f;
        public float SpeedMax = 100f;
        public float Interval = .1f;
        public int EmitCount = 1;
        public float TotalLifeSpan = .6f;

        public ParticleEmitterData()
        {
        }
    }
}
