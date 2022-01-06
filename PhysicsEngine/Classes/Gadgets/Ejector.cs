using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsEngine.Classes.Gadgets
{
    public class Ejector : PhysicsGadget
    {
        private readonly Vector2 ejectAwayFrom;
        private readonly float speed;
        public Ejector(Collidable collidable, Vector2 ejectAwayFrom, float speed = 1f) : base(collidable)
        {
            this.ejectAwayFrom = ejectAwayFrom;
            this.speed = speed;
        }
        public override void Update(GameTime gameTime)
        {
            //collidable.Move(collidableEjectAwayFrom.CenteredPosition, gameTime, speed);
        }

        public override void Trigger()
        {
            base.Trigger();
            collidable.MainHullBody.Body.ApplyLinearImpulse(ejectAwayFrom);
            collidable.MainHullBody.Body.IgnoreGravity = false;
            collidable.MainHullBody.Body.GravityScale = 5f;
            collidable.MainHullBody.Body.Restitution = .65f;

            collidable.MainHullBody.Body.Mass = 2;

        }

    }
}
