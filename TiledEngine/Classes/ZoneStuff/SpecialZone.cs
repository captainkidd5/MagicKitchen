using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace TiledEngine.Classes.ZoneStuff
{
    public class SpecialZone : Collidable, ISaveable
    {
        public string Name { get; private set; }
        public Rectangle Rectangle { get; private set; }

        public SpecialZone() : base()
        {

        }
        public SpecialZone(string name, Rectangle rectangle)
        {
            Name = name;
            Rectangle = rectangle;
            Move(new Vector2(Rectangle.X, Rectangle.Y));
        }

        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
            AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Static, position,
             Rectangle.Width, Rectangle.Height, new List<Category>() { Category.SpecialZone },
             new List<Category>() { Category.Player, Category.Item, Category.NPC, Category.PlayerBigSensor },
             OnCollides,OnSeparates));
        }

        public void Save(BinaryWriter writer)
        {
          writer.Write(Name);
            Vector2Helper.WriteVector2(writer, Position);
            RectangleHelper.WriteRectangle(writer, Rectangle);
        }

        public void LoadSave(BinaryReader reader)
        {
            Name = reader.ReadString();
           Move(Vector2Helper.ReadVector2(reader));
            Rectangle = RectangleHelper.ReadRectangle(reader);
            CreateBody(Position);
        }
    }
}
