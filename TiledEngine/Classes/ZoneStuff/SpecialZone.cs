using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.ZoneStuff
{
    public class SpecialZone : Collidable
    {
        public string Name { get; }
        public Rectangle Rectangle { get; }

        public SpecialZone(string name, Rectangle rectangle)
        {
            Name = name;
            Rectangle = rectangle;
        }

        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
            AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, position,
             Rectangle.Width, Rectangle.Height, new List<Category>() { Category.SpecialZone },
             new List<Category>() { Category.Player, Category.Item, Category.NPC, Category.PlayerBigSensor },
             OnCollides,OnSeparates));
        }
    }
}
