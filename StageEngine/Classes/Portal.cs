using EntityEngine.Classes;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using StageEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using UIEngine.Classes;
using static DataModels.Enums;

namespace StageEngine.Classes

{
    public class Portal : Collidable, ISaveable
    {
        private readonly PortalManager _portalManager;
        private readonly StageManager _stageManager;

        internal string From { get; set; }
        internal string To { get; set; }
        internal int PortalxOffSet;
        internal int PortalyOffSet;

     
        public Rectangle Rectangle { get; private set; }
        private bool _mustBeClicked;
        private Direction _directionToFace;
        public Portal(PortalManager portalManager,StageManager stageManager,
            Rectangle rectangle, string from, string to, int xOffSet, int yOffSet,Direction directionToFace, bool mustBeClicked) : base()
        {
            _portalManager = portalManager;
            _stageManager = stageManager;
            Rectangle = rectangle;
            From = from;
            To = to;
            PortalxOffSet = xOffSet;
            PortalyOffSet = yOffSet;
            _mustBeClicked = mustBeClicked;
            _directionToFace = directionToFace;
        }
        public Portal(PortalManager portalManager, StageManager stageManager)
        {
            _portalManager = portalManager;
            _stageManager = stageManager;
        }
        public void Load()
        {
            Move(new Vector2(Rectangle.X, Rectangle.Y));

            CreateBody(Position);
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
            AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, Rectangle.Width, Rectangle.Height,
                new List<Category>() { (Category)PhysCat.Portal }, new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.NPC,
                    (Category)PhysCat.Cursor, (Category)PhysCat.PlayerBigSensor, (Category)PhysCat.NPCBigSensor, (Category)PhysCat.FrontalSensor },
                OnCollides, OnSeparates, ignoreGravity:true));
;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            if(MainHullBody != null)
               MainHullBody.Body.Position = Position;

            MainHullBody.Body.AngularVelocity = 0f;
            if (PlayerInClickRange)
            {
               
            }
           
        }

        
        protected override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

         
            return base.OnCollides(fixtureA, fixtureB, contact);

        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
           base.OnSeparates(fixtureA, fixtureB, contact);
        }



        public void Save(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);

            RectangleHelper.WriteRectangle(writer, Rectangle);
            writer.Write(PortalxOffSet);
            writer.Write(PortalyOffSet);
            writer.Write(_mustBeClicked);

            writer.Write((int)_directionToFace);



        }

        public void LoadSave(BinaryReader reader)
        {
            From = reader.ReadString();
            To = reader.ReadString();
            Rectangle = RectangleHelper.ReadRectangle(reader);
            PortalxOffSet = reader.ReadInt32();
            PortalyOffSet = reader.ReadInt32();
            _mustBeClicked = reader.ReadBoolean();
            _directionToFace = (Direction)reader.ReadInt32();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            From = null;
            To = null;
            _mustBeClicked =false;
        }
    }
}
