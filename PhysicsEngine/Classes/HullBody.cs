
using Microsoft.Xna.Framework;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace PhysicsEngine.Classes
{

    
    public enum ShapeType
    {
        Rectangle = 1,
        Circle = 2
    }
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

        //Penumbra hull
        public Hull Hull { get; set; }
        public Vector2 Position { get { return Body.Position; } set { Body.SetTransform(new Vector2(value.X + XOffset, value.Y + YOffset), 0f); } }

        //Penumbra light

        public Light Light { get; set; }

  

        internal HullBody(Body body,Hull hull)
        {
            Body = body;
            Hull = hull;
        }
      
        public void Update(GameTime gameTime)
        {
            Hull.Position = Body.Position;
        }


        public virtual void Destroy( )
        {
            PhysicsManager.VelcroWorld.RemoveBody(Body);

            if(Hull != null)
                PhysicsManager.Penumbra.Hulls.Remove(Hull);

            if(Light != null)
                PhysicsManager.Penumbra.Lights.Remove(Light);

        }

    }
}
