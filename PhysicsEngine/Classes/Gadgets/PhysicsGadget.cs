using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsEngine.Classes.Gadgets
{
    public abstract class PhysicsGadget
    {
        protected Collidable CollidableToInteractWith;

        public PhysicsGadget(Collidable collidable)
        {
            this.CollidableToInteractWith = collidable;
        }
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Triggers main action of this gadget.
        /// </summary>
        public virtual void Trigger()
        {

        }

        public virtual void Destroy()
        {
        }
    }
}
