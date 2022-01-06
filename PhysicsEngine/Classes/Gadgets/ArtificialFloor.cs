using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace PhysicsEngine.Classes.Gadgets
{
    public class ArtificialFloor : PhysicsGadget
    {

        private static int width = 150;
        private static int height = 5;
        public Body FloorBody { get; set; }
        public ArtificialFloor(Collidable collidable, Category categoryToCollideWith) : base(collidable)
        {
            FloorBody = BodyFactory.CreateRectangle(PhysicsManager.VelcroWorld,width,height,1f,
                new Vector2(collidable.Position.X, collidable.Position.Y + 25), 0f, BodyType.Static);
            
            FloorBody.SetCollisionCategory(categoryToCollideWith);
            
            //collidable.MainHull.Body.CollidesWith = FloorBody;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Remove()
        {
            PhysicsManager.VelcroWorld.RemoveBody(FloorBody);
        }
    }
}
