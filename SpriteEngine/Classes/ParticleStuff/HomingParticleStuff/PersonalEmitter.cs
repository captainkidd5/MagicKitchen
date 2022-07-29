using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff.HomingParticleStuff
{
    public class PersonalEmitter
    {
        public List<HomingParticle> Particles{ get; set; }

        public PersonalEmitter()
        {
            Particles = new List<HomingParticle>();
        }
        public void AddParticle(Vector2 thisEntityPos)
        {
            ParticleData data = new ParticleData()
            {

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
                if(p.HasReachedTarget)
                    Particles.RemoveAt(i);
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
