using Globals.Classes;
using Globals.Classes.Helpers;
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
        public Portal()
        {

        }
        public Portal(string from, string to, Vector2 pos)
        {
            From = from;
            To = to;
            Move(pos);
            CreateBody(pos);

        }
        public string From { get; private set; }
        public string To { get; private set; }


        public static Portal GetPortal(ref string unparsedString, int tileX, int tileY)
        {
            string[] splitString = unparsedString.Split(',');
            string from = splitString[0];
            string to = splitString[1];

            unparsedString = from;
           return new Portal(from, to, Vector2Helper.GetWorldPositionFromTileIndex(tileX, tileY));


        }

        public void LoadSave(BinaryReader reader)
        {
            From = reader.ReadString();
            To = reader.ReadString();
            Move(Vector2Helper.ReadVector2(reader));
            CreateBody(Position);

        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            Vector2Helper.WriteVector2(writer, Position);
        }

        public void SetToDefault()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void CreateBody(Vector2 position)
        {
            MainHullBody = PhysicsManager.CreateCircularHullBody(BodyType.Dynamic, Position, 6f, new List<Category>() { (Category)PhysCat.Portal },
              new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.Cursor, (Category)PhysCat.NPC},
              OnCollides, OnSeparates,userData: this);

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
