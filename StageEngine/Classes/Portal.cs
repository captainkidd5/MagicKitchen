using EntityEngine.Classes;
using EntityEngine.Classes.PlayerStuff;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using StageEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using UIEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using static Globals.Classes.Settings;

namespace StageEngine.Classes

{
    public class Portal : Collidable
    {
        private readonly PortalManager _portalManager;
        private readonly StageManager _stageManager;
        private readonly PlayerManager _playerManager;
        internal readonly int xOffSet;
        internal readonly int yOffSet;

        internal string From { get; set; }
        internal string To { get; set; }
        private Rectangle Rectangle { get; set; }
        private bool _mustBeClicked;
        private Direction _directionToFace;
        public Portal(PortalManager portalManager,StageManager stageManager,PlayerManager playerManager,
            Rectangle rectangle, string from, string to, int xOffSet, int yOffSet,Direction directionToFace, bool mustBeClicked) : base()
        {
            Move(new Vector2(rectangle.X, rectangle.Y));
            _portalManager = portalManager;
            _stageManager = stageManager;
            _playerManager = playerManager;
            Rectangle = rectangle;
            From = from;
            To = to;
            this.xOffSet = xOffSet;
            this.yOffSet = yOffSet;
            _mustBeClicked = mustBeClicked;
            _directionToFace = directionToFace;
        }
        public void Load(Vector2 position)
        {
            CreateBody(position);
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
            AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, Rectangle.Width, Rectangle.Height,
                new List<Category>() { Category.Portal }, new List<Category>() { Category.Player,Category.NPC, Category.Cursor, Category.PlayerBigSensor, Category.NPCBigSensor }, OnCollides, OnSeparates));
;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            if(MainHullBody != null)
               MainHullBody.Body.Position = Position;
            if (PlayerInClickRange)
            {
                if (From == _playerManager.Player1.CurrentStageName)
                {
                    if (!_mustBeClicked)
                    {
                        if (_playerManager.Player1.AbleToWarp)
                        {
                            _stageManager.RequestSwitchStage(To, _portalManager.GetDestinationPosition(this));
                            _playerManager.Player1.StartWarp(To, _portalManager.GetDestinationPosition(this),
                                _stageManager.GetStage(To).TileManager, _stageManager.GetStage(To).ItemManager, _directionToFace);

                        }

                        return;
                    }
                    if (_mustBeClicked && PlayerInClickRange && MouseHovering)
                    {
                        UI.Cursor.CursorIconType = CursorIconType.Door;
                        //Controls.UpdateCursor();

                        if (Controls.IsClicked)
                        {

                            _stageManager.RequestSwitchStage(To, _portalManager.GetDestinationPosition(this));
                            UI.Cursor.CursorIconType = CursorIconType.None;

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
                    if (entity.AbleToWarp)
                    {
                        entity.StartWarp(To, _portalManager.GetDestinationPosition(this), _stageManager.GetStage(To).TileManager, _stageManager.GetStage(To).ItemManager, _directionToFace);

                        entity.IsInStage = To == _stageManager.CurrentStage.Name;
                        
                    }
                }
                
                    
            }
        }

        protected override void OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
           base.OnSeparates(fixtureA, fixtureB, contact);
        }

    }
}
