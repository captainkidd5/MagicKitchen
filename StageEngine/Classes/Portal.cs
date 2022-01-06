using EntityEngine.Classes;
using EntityEngine.Classes.PlayerStuff;
using InputEngine.Classes.Input;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using StageEngine.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace StageEngine.Classes

{
    public class Portal : Collidable
    {
        internal readonly int xOffSet;
        internal readonly int yOffSet;

        public string From { get; set; }
        public string To { get; set; }
        public Rectangle Rectangle { get; set; }
        public bool MustBeClicked { get; set; }
        public Portal(Rectangle rectangle, string from, string to, int xOffSet, int yOffSet, bool mustBeClicked) : base()
        {
            Move(new Vector2(rectangle.X, rectangle.Y));
            Rectangle = rectangle;
            From = from;
            To = to;
            this.xOffSet = xOffSet;
            this.yOffSet = yOffSet;
            MustBeClicked = mustBeClicked;
        }
        public void Load(Vector2 position)
        {
            CreateBody(position);
        }
        protected override void CreateBody(Vector2 position)
        {
            base.CreateBody(position);
            AddPrimaryBody(PhysicsManager.CreateRectangularHullBody(BodyType.Dynamic, Position, Rectangle.Width, Rectangle.Height,
                new List<Category>() { Category.Portal }, new List<Category>() { Category.Player, Category.Cursor, Category.PlayerBigSensor, Category.NPCBigSensor }, OnCollides, OnSeparates));
;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(MainHullBody != null)
               MainHullBody.Body.Position = Position;
            if (PlayerInClickRange)
            {
                if (!MustBeClicked)
                {
                    if (PlayerManager.Player1.AbleToWarp)
                    {
                        StageManager.RequestSwitchStage(To, PortalManager.GetDestinationPosition(this));
                        PlayerManager.Player1.Warp(To, PortalManager.GetDestinationPosition(this), StageManager.GetStage(To).TileManager);

                    }

                    return;
                }
                if (MustBeClicked && PlayerInClickRange && MouseHovering)
                {
                    Controls.CursorIconType = CursorIconType.Door;
                    Controls.UpdateCursor();

                    if (Controls.IsClicked)
                    {

                        StageManager.RequestSwitchStage(To, PortalManager.GetDestinationPosition(this));
                        Controls.CursorIconType = CursorIconType.None;

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
                        entity.Warp(To, PortalManager.GetDestinationPosition(this), StageManager.GetStage(To).TileManager);

                        entity.IsInStage = entity.CurrentStageName == StageManager.CurrentStage.Name;
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
