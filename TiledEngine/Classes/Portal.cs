using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using UIEngine.Classes;

namespace TiledEngine.Classes
{
    public delegate void PortalClicked(Portal portal);

    public class Portal : Collidable, ISaveable
    {
        public event PortalClicked PortalClicked;
        public Rectangle Rectangle { get; set; }

        public Portal()
        {

        }
        protected virtual void OnPortalClicked()
        {
            PortalClicked?.Invoke(this);
        }
        public Portal(string from, string to, Rectangle rectangle)
        {
            From = from;
            To = to;
            Rectangle = rectangle;
            CreateBody(Vector2.Zero);

        }
        public string From { get; private set; }
        public string To { get; private set; }


        /// <summary>
        /// For TMX Objects in tile properties
        /// </summary>
        /// <returns></returns>
        public static Portal GetPortal(string unparsedString,int x, int y)
        {
            string[] splitString = unparsedString.Split(',');
            string from = splitString[0];
            string to = splitString[1];
            int width = int.Parse(splitString[2]);
            int height = int.Parse(splitString[3]);

            return new Portal(from, to, new Rectangle(x,y,width,height));
        }
        /// <summary>
        /// For object zones
        /// </summary>
        /// <returns></returns>
        public static Portal GetObjectGroupPortal(string unparsedString,int x, int y, int width, int height)
        {
            string[] splitString = unparsedString.Split(',');
            string from = splitString[0];
            string to = splitString[1];

            return new Portal(from, to, new Rectangle(x, y, width, height));
        }
        public void LoadSave(BinaryReader reader)
        {
            From = reader.ReadString();
            To = reader.ReadString();
            Rectangle = RectangleHelper.ReadRectangle(reader);
            CreateBody(Position);

        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            RectangleHelper.WriteRectangle(writer, Rectangle);
        }

        public void SetToDefault()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            if (MainHullBody != null)
               MainHullBody.Position = Position;

            if (WithinRangeOfPlayer(Controls.ControllerConnected))
            {
                if (IsHovered(Controls.ControllerConnected))
                {
                    //Console.WriteLine("test");
                    UI.Cursor.ChangeCursorIcon(CursorIconType.Door);

                    if (Controls.IsClickedWorld)
                        OnPortalClicked();

                }

            }
        }
       

        protected override void CreateBody(Vector2 position)
        {
            Move(Vector2Helper.GetVector2FromRectangle(Rectangle));
            MainHullBody = PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, Rectangle.Width, Rectangle.Height, new List<Category>() {
                (Category)PhysCat.Portal, (Category)PhysCat.ClickBox },
              new List<Category>() {
                  (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.Cursor, (Category)PhysCat.NPC},
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
            if (fixtureB.CollisionCategories.HasFlag(((Category)PhysCat.Cursor))){
                OnClickBoxCollides(fixtureA, fixtureB, contact);
            }
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories.HasFlag(((Category)PhysCat.Cursor)))
            {
                OnClickBoxSeparates(fixtureA, fixtureB, contact);
            }
            base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
