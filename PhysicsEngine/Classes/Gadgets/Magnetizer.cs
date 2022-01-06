using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsEngine.Classes.Gadgets
{
    /// <summary>
    /// Moves collidable towards give position
    /// </summary>
    public class Magnetizer : PhysicsGadget
    {
        private readonly Collidable collidableToMoveTowards;
        private readonly float speed;

        public Magnetizer(Collidable collidable, Collidable collidableToMoveTowards, float speed = 1f) :base(collidable)
        {
            this.collidableToMoveTowards = collidableToMoveTowards;
            this.speed = speed;
        }
        public override void Update(GameTime gameTime)
        {
           // collidable.MoveTowardsVector(collidableToMoveTowards.Position, gameTime, speed);
        }

    }
}
