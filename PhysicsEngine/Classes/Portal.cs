using Globals.Classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace PhysicsEngine.Classes
{
    public class Portal : Collidable, ISaveable
    {

        public Portal(string from, string to)
        {
            From = from;
            To = to;
        }
        public string From { get; private set; }
        public string To { get; private set; }


        public static Portal GetPortal(ref string unparsedString)
        {
            string[] splitString = unparsedString.Split(',');
            string from = splitString[0];
            string to = splitString[1];

            unparsedString = from;
            return new Portal(from, to);



        }

        public void LoadSave(BinaryReader reader)
        {
            From = reader.ReadString();
            To = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }

        public void SetToDefault()
        {
            throw new NotImplementedException();
        }

        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
        }

        protected override bool OnClickBoxCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnClickBoxCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnClickBoxSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnClickBoxSeparates(fixtureA, fixtureB, contact);
        }

        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
