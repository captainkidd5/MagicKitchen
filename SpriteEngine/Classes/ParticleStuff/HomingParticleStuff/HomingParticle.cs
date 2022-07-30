using Globals.Classes;
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
        float gravity = -2f;
        Vector2 velocity;
        Vector2 impulse;
        public bool HasReachedTarget { get; private set; }
        
        public HomingParticle(Vector2 pos, ParticleData data) : base(pos, data)
        {
            impulse = new Vector2(Settings.Random.Next(-50,50), Settings.Random.Next(-50, 50));
        }

        protected override void MoveParticle(GameTime gameTime, Vector2? targetPos)
        {
            LifeSpanLeft = 10;
            //velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
           // height -= velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Vector2Helper.MoveTowardsVector(targetPos.Value, Position, ref velocity, gameTime, 8))
            {
                HasReachedTarget = true;
            }
            //gravity++;
            impulse = new Vector2(impulse.X * .99f, impulse.Y * .99f);

            Vector2 impulseVelocity = impulse * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = new Vector2(Position.X += velocity.X, Position.Y += velocity.Y );
            Position += impulseVelocity;
            //base.MoveParticle(gameTime, targetPos);
        }
    }
}
