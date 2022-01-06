using EntityEngine.Classes.NPCStuff;
using Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;

namespace EntityEngine.Classes.BehaviourStuff
{
    public abstract class Behaviour : IDebuggable
    {
        protected Entity Entity;
        protected StatusIcon StatusIcon;
        protected Navigator Navigator;

        public Behaviour(Entity entity,StatusIcon statusIcon, Navigator navigator)
        {
            Entity = entity;
           StatusIcon = statusIcon;
            Navigator = navigator;
        }

        public virtual void Update(GameTime gameTime, ref Vector2 velocity)
        {

        }

        protected void GetPath(Vector2 newPosition, string destinationStageName = null)
        {
            Vector2 targetPosition = Vector2.Zero;
            //Trying to find path to new stage!
            if (Entity.CurrentStageName != destinationStageName)
            {
                if (TileLoader.HasEdge(Entity.CurrentStageName, destinationStageName))
                {
                    string newStage = TileLoader.GetNextNodeStageName(Entity.CurrentStageName, destinationStageName);
                    if (string.IsNullOrEmpty(newStage))
                        throw new Exception($"Node may not be empty");
                    Rectangle portalDestinationRectangle = TileLoader.GetNextNodePortalRectangle(Entity.CurrentStageName, newStage);
                    targetPosition = new Vector2(portalDestinationRectangle.X + portalDestinationRectangle.Width / 2, portalDestinationRectangle.Y + portalDestinationRectangle.Height / 2);


                }
                else
                {
                    string nextStage = TileLoader.GetNextNodeStageName(Entity.CurrentStageName, destinationStageName);
                    if (string.IsNullOrEmpty(nextStage))
                    {
                        throw new Exception($"No intermediate stages between {Entity.CurrentStageName} and {destinationStageName}");
                    }
                    Rectangle portalDestinationRectangle = TileLoader.GetNextNodePortalRectangle(Entity.CurrentStageName, nextStage);
                    targetPosition = new Vector2(portalDestinationRectangle.X + portalDestinationRectangle.Width / 2, portalDestinationRectangle.Y + portalDestinationRectangle.Height / 2);

                }
            }
            else
            {
                //Find portal in same stage
                targetPosition = newPosition;
            }

            if (Navigator.FindPathTo(Entity.Position, targetPosition))
            {
                Navigator.SetTarget(targetPosition);
            }
            else
            {
                //give visual cue that couldn't find path.
                StatusIcon.SetStatus(StatusIconType.NoPath);
            }

        }

        public virtual void DrawDebug(SpriteBatch spriteBatch)
        {

        }

        public virtual void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            
        }
    }
}
