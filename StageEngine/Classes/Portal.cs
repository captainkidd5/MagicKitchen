using EntityEngine.Classes;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes;
using Globals.Classes.Helpers;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using StageEngine.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UIEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace StageEngine.Classes

{
    public class Portal : Collidable, ISaveable
    {
        private readonly PortalManager _portalManager;
        private readonly StageManager _stageManager;
        private readonly EntityManager _entityManager;

        internal string From { get; set; }
        internal string To { get; set; }
        internal int PortalxOffSet;
        internal int PortalyOffSet;

     
        private Rectangle Rectangle { get; set; }
        private bool _mustBeClicked;
        private Direction _directionToFace;
        public Portal(PortalManager portalManager,StageManager stageManager,EntityManager entityManager,
            Rectangle rectangle, string from, string to, int xOffSet, int yOffSet,Direction directionToFace, bool mustBeClicked) : base()
        {
            _portalManager = portalManager;
            _stageManager = stageManager;
            _entityManager = entityManager;
            Rectangle = rectangle;
            From = from;
            To = to;
            PortalxOffSet = xOffSet;
            PortalyOffSet = yOffSet;
            _mustBeClicked = mustBeClicked;
            _directionToFace = directionToFace;
        }
        public Portal(PortalManager portalManager, StageManager stageManager, EntityManager entityManager)
        {
            _portalManager = portalManager;
            _stageManager = stageManager;
            _entityManager = entityManager;
        }
        public void Load(Vector2 position)
        {
            Move(new Vector2(Rectangle.X, Rectangle.Y));

            CreateBody(Position);
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
            AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, Rectangle.Width, Rectangle.Height,
                new List<Category>() { Category.Portal }, new List<Category>() { Category.Player,Category.NPC, Category.Cursor, Category.PlayerBigSensor, Category.NPCBigSensor }, OnCollides, OnSeparates, ignoreGravity:true));
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
                if (From == _entityManager.Player1.CurrentStageName)
                {
                   
                    if (_mustBeClicked && PlayerInClickRange && MouseHovering)
                    {
                        UI.Cursor.CursorIconType = CursorIconType.Door;
                        //Controls.UpdateCursor();

                        if (Controls.IsClicked)
                        {

                            _stageManager.RequestSwitchStage(To, _portalManager.GetDestinationPosition(this));
                            UI.Cursor.CursorIconType = CursorIconType.None;
                            _entityManager.Player1.StartWarp(To, _portalManager.GetDestinationPosition(this),
                                _stageManager.GetStage(To).TileManager, _stageManager.GetStage(To).ItemManager, _directionToFace);

                        }
                    }
                }
            }
           
        }

        
        protected override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);

            //If NPC big sensor collides with portal, warp them to that stage
            if (fixtureB.CollisionCategories.HasFlag(Category.NPCBigSensor))
            {
                Entity entity = (fixtureB.Body.UserData as Entity);
                if(From == entity.CurrentStageName)
                {
                    //DO NOT WANT TO HANDLE COLLISIONS ACROSS SEPARATE STAGES! Make sure entity and portal are in the same stage.
                    //Ex: player should not be warping to home from within another house, even if the portal is technically at 50,50 in both places.
                   
                        entity.StartWarp(To, _portalManager.GetDestinationPosition(this), _stageManager.GetStage(To).TileManager, _stageManager.GetStage(To).ItemManager, _directionToFace);

                        entity.IsInStage = To == _stageManager.CurrentStage.Name;
                        
                    
                }
                
                    
            }
            else if(!_mustBeClicked &&  fixtureB.CollisionCategories.HasFlag(Category.Player))
            {
                Entity entity = (fixtureB.Body.UserData as Entity);
                if (From == entity.CurrentStageName)
                {

                    _stageManager.RequestSwitchStage(To, _portalManager.GetDestinationPosition(this));
                _entityManager.Player1.StartWarp(To, _portalManager.GetDestinationPosition(this),
                    _stageManager.GetStage(To).TileManager, _stageManager.GetStage(To).ItemManager, _directionToFace);
                }


            }
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
