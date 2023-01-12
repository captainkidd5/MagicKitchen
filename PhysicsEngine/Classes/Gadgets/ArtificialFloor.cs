using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace PhysicsEngine.Classes.Gadgets
{
    public class ArtificialFloor : PhysicsGadget

    {

        private static int _width = 150;
        private static int _height = 5;
        internal bool HasTouchAtLeastOnce { get; private set; }
        public HullBody FloorBody { get; set; }
        public ArtificialFloor(Collidable collidable) : base(collidable)
        {
            FloorBody = PhysicsManager.CreateRectangularHullBody(BodyType.Static, GetRandomPosition(), _width,_height, new List<Category>() { (Category)PhysCat.ArtificialFloor } ,
                new List<Category>() { (Category)PhysCat.Item}, OnCollides, OnSeparates, restitution: 5f, friction:.15f, userData: this);

        }

        private Vector2 GetRandomPosition()
        {
            return new Vector2(CollidableToInteractWith.Position.X, CollidableToInteractWith.Position.Y + PhysicsManager.Random.Next(0, 25));
        }
        public override void Update(GameTime gameTime)
        {
        }
       
        public override void Destroy()
        {
            FloorBody.DestroyFromPhysicsWorld();
        }
    }
}
