using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff.HomingParticleStuff
{
    public delegate void ParticleReachedDestination();
    public class PersonalEmitter
    {
        public event ParticleReachedDestination PReached;
        public List<HomingParticle> Particles{ get; set; }

        public PersonalEmitter()
        {
            Particles = new List<HomingParticle>();
        }
        public void AddParticle(Vector2 thisEntityPos)
        {
            ParticleData data = new ParticleData()
            {
                sizeStart = 8f,
                sizeEnd = 2f,
                colorStart = new Color(255, 255, 255, 255),
                colorEnd = new Color(255, 255, 255, 100)
            };
            HomingParticle particle = new HomingParticle(thisEntityPos, data);
            Particles.Add(particle);
        }
        public void Update(GameTime gameTime, Vector2 otherEntityPos)
        {
            for(int i = Particles.Count -1; i > 0; i--)
            {
                HomingParticle p = Particles[i];
                p.Update(gameTime, otherEntityPos);
                if (p.HasReachedTarget)
                {
                    Particles.RemoveAt(i);
                    PReached?.Invoke();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = Particles.Count - 1; i > 0; i--)
            {
                Particles[i].Draw(spriteBatch);
            }
        }

    }
}
