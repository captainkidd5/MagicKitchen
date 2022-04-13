﻿using EntityEngine.Classes.CharacterStuff;
using Globals;
using Globals.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;
using static DataModels.Enums;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.BehaviourStuff
{
    public abstract class Behaviour : IDebuggable
    {
        protected Entity Entity;
        protected StatusIcon StatusIcon;
        protected Navigator Navigator;
        protected TileManager TileManager;
        protected SimpleTimer SimpleTimer;

        protected float TimerFrequency;
        public Behaviour(Entity entity,StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency)
        {
            Entity = entity;
           StatusIcon = statusIcon;
            Navigator = navigator;
            TileManager = tileManager;
            timerFrequency = timerFrequency ?? 5f;
            SimpleTimer = new SimpleTimer(timerFrequency.Value, true);


        }

        public void SwitchStage(TileManager tileManager)
        {
            TileManager = tileManager;

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

        /// <summary>
        /// Gets list of tile points in given radius with given gid
        /// </summary>

        public List<Point> GetTilePoints(int gid, Layers layer, SearchType searchType = SearchType.Radial)
        {
            if (searchType == SearchType.Radial)
                return TileManager.LocateTile_RadialSearch(gid, layer, Entity.TileOn, 10);

            //else if (searchType == SearchType.Grid)
            //    return TileManager.LocateTile_GridSearch(gid, layer, Entity.TileOn, 10);
            else
                throw new Exception($"Invalid searchtype {searchType.ToString()}");

        }
        public virtual void DrawDebug(SpriteBatch spriteBatch)
        {
            if (Entity.IsInStage)
                Navigator.DrawDebug(spriteBatch, Color.Green);
            else
                Navigator.DrawDebug(spriteBatch, Color.Red);
        }

        public virtual void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            
        }
    }
}