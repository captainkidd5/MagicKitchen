using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff
{
    public static class ParticleManager
    {
        private static readonly List<Particle> _particles = new();
        private static readonly List<ParticleEmitter> _particleEmitters = new();

        internal static Texture2D ParticleSheet;

        public static void Load(ContentManager content)
        {

        }
        public static void AddParticle(Particle p)
        {
            _particles.Add(p);
        }

        public static void AddParticleEmitter(ParticleEmitter e)
        {
            _particleEmitters.Add(e);
        }

        public static void UpdateParticles(GameTime gameTime)
        {
            foreach (var particle in _particles)
            {
                particle.Update(gameTime);
            }

            _particles.RemoveAll(p => p.isFinished);
        }

        public static void UpdateEmitters(GameTime gameTime)
        {
            foreach (var emitter in _particleEmitters)
            {
                emitter.Update(gameTime);
            }
        }

        public static void Update(GameTime gameTime)
        {
            UpdateParticles(gameTime);
            UpdateEmitters(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in _particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
