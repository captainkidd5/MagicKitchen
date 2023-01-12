
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace PhysicsEngine.Classes
{

    /// <summary>
    /// Wrapper class for Velcro Physics Body! <see cref="TileObjectHelper"/> .
    /// </summary>
    public class HullBody
    {

        //X and Y Offset: moves the body with the Icollidanble position, but allows
        //for you to not have to stick it directly on the Icollidable position when it moves
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        //Velcro physics body
        public Body Body { get; set; }


        public bool Destroyed { get; private set; }

        public Vector2 Position { get { return Body.Position; } set { if (Body.World == null) { Debug.Assert(Body.World != null, $"world null"); return; } Body.SetTransform(new Vector2(value.X + XOffset, value.Y + YOffset), 0f); } }



  

        internal HullBody(Body body)
        {
            Body = body;

        }
      
        public void Update(GameTime gameTime)
        {
            
        }



        public virtual void DestroyFromPhysicsWorld( )
        {
            PhysicsManager.VelcroWorld.RemoveAsync(Body);
            Destroyed = true;

        }

    }
}
