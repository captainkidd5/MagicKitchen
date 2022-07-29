using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteEngine.Classes.ParticleStuff.HomingParticleStuff
{
    public class HomingParticle : Particle
    {
        float gravity = -500f;
        Vector2 velocity;
        float height;
        public bool HasReachedTarget { get; private set; }
        
        public HomingParticle(Vector2 pos, ParticleData data) : base(pos, data)
        {
        }

        protected override void MoveParticle(GameTime gameTime, Vector2? targetPos)
        {
            LifeSpanLeft = 10;
          //  velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
           // height -= velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Vector2Helper.MoveTowardsVector(targetPos.Value, Position, ref velocity, gameTime, 8))
            {
                Console.WriteLine("test");
                HasReachedTarget = true;
            }
            //base.MoveParticle(gameTime, targetPos);
        }
    }
}
