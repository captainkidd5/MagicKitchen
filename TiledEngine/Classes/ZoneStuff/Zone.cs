using Globals.Classes;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using SoundEngine.Classes;
using SoundEngine.Classes.SongStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace TiledEngine.Classes.ZoneStuff
{
    internal class Zone : Collidable, ISaveable
    {
        public string Property { get; private set; }
        public string Value { get; private set; }

        public Rectangle Rectangle { get; private set; }
        public Zone(string name, string value, Rectangle rectangle)
        {
            Property = name;
            Value = value;
            Rectangle = rectangle;

        }
        public Zone()
        {

        }
        protected override void CreateBody(Vector2 position)
        {
            Move(Vector2Helper.GetVector2FromRectangle(Rectangle));
            MainHullBody = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, Rectangle.Width, Rectangle.Height,
                new List<Category>() { (Category)PhysCat.SpecialZone },
          new List<Category>() { (Category)PhysCat.PlayerBigSensor },
          OnCollides, OnSeparates, isSensor: true, userData: this, ignoreGravity: true);
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }

        public virtual void Save(BinaryWriter writer)
        {
            writer.Write(Property);
            writer.Write(Value);
            RectangleHelper.WriteRectangle(writer, Rectangle);
        }

        public virtual void LoadSave(BinaryReader reader)
        {
            Property = reader.ReadString();
            Value = reader.ReadString();
            Rectangle = RectangleHelper.ReadRectangle(reader);
            CreateBody(Vector2Helper.GetVector2FromRectangle(Rectangle));
        }

        public void SetToDefault()
        {
            throw new NotImplementedException();
        }
    }
}
