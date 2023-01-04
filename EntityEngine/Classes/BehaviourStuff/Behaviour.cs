using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
using Globals;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes;
using TiledEngine.Classes.ZoneStuff;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.BehaviourStuff
{
    public abstract class Behaviour : IDebuggable
    {
        protected NPC Entity;
        protected StatusIcon StatusIcon;
        protected TileManager TileManager;

        protected SimpleTimer SimpleTimer;
        protected float TimerFrequency;

        protected SimpleTimer TimeInSameSpotTimer;
        protected float SameSpotTimeBeforeUnstuck = 3f;

        protected BehaviourManager BehaviourManager { get; }

        public Behaviour(BehaviourManager behaviourManager, NPC entity,StatusIcon statusIcon, TileManager tileManager, float? timerFrequency)
        {
            BehaviourManager = behaviourManager;
            Entity = entity;
           StatusIcon = statusIcon;
            TileManager = tileManager;
            timerFrequency = timerFrequency ?? 5f;
            SimpleTimer = new SimpleTimer(timerFrequency.Value, true);

            TimeInSameSpotTimer = new SimpleTimer(SameSpotTimeBeforeUnstuck, true);
        }

        public void SwitchStage(TileManager tileManager)
        {
            TileManager = tileManager;

        }

        protected bool IsStuck(GameTime gameTime)
        {
            if (!Entity.HasPointChanged)
            {
                if (TimeInSameSpotTimer.Run(gameTime))
                {
                    return true;
                }
            }
            else
            {
                TimeInSameSpotTimer.ResetToZero();
            }
            return false;
        }
        public virtual void Update(GameTime gameTime, ref Vector2 velocity)
        {

        }

        protected void GetPath(Vector2 newPosition, string destinationName, bool isZone)
        {
            Vector2 targetPosition = Vector2.Zero;

            if (isZone)
            {
                Console.WriteLine("test");
                Zone zone = MapLoader.ZoneManager.GetZone(destinationName.Split(',')[0], destinationName.Split(',')[1]);
                destinationName = zone.StageName;
            }
            //Trying to find path to new stage!
            if (Entity.CurrentStageName != destinationName)
            {
                if (MapLoader.HasEdge(Entity.CurrentStageName, destinationName))
                {
                    string newStage = MapLoader.GetNextNodeStageName(Entity.CurrentStageName, destinationName);
                    if (string.IsNullOrEmpty(newStage))
                        throw new Exception($"Node may not be empty");
                    Rectangle portalDestinationRectangle = MapLoader.GetNextPortalRectangle(Entity.CurrentStageName, newStage);
                    targetPosition = new Vector2(portalDestinationRectangle.X + portalDestinationRectangle.Width / 2, portalDestinationRectangle.Y + portalDestinationRectangle.Height / 2);


                }
                else
                {
                    string nextStage = MapLoader.GetNextNodeStageName(Entity.CurrentStageName, destinationName);
                    if (string.IsNullOrEmpty(nextStage))
                    {
                        throw new Exception($"No intermediate stages between {Entity.CurrentStageName} and {destinationName}");
                    }
                    Rectangle portalDestinationRectangle = MapLoader.GetNextPortalRectangle(Entity.CurrentStageName, nextStage);
                    targetPosition = new Vector2(portalDestinationRectangle.X + portalDestinationRectangle.Width / 2, portalDestinationRectangle.Y + portalDestinationRectangle.Height / 2);

                }
            }
            else
            {
                //Find portal in same stage
                targetPosition = newPosition;
            }

            if (Entity.FindPathTo(targetPosition))
            {
                Entity.SetNavigatorTarget(targetPosition);
            }
            else
            {
                //give visual cue that couldn't find path.
                StatusIcon.SetStatus(StatusIconType.NoPath);
            }

        }

        /// <summary>
        /// Gets list of tile points in given radius with given gid
        /// </summary>

        public List<Point> GetTilePoints(int gid, Layers layer, SearchType searchType = SearchType.Radial)
        {
            if (searchType == SearchType.Radial)
                return TileManager.TileLocationHelper.LocateTile_RadialSearch(gid, layer, Entity.TileOn, 10);

            //else if (searchType == SearchType.Grid)
            //    return TileManager.LocateTile_GridSearch(gid, layer, Entity.TileOn, 10);
            else
                throw new Exception($"Invalid searchtype {searchType.ToString()}");

        }
        public virtual void DrawDebug(SpriteBatch spriteBatch)
        {
 
                Entity.DrawDebug(spriteBatch);
      
        }

        public virtual bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        public virtual bool OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }
    }
}
 