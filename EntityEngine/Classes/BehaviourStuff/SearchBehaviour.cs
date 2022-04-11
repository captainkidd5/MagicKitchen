﻿using EntityEngine.Classes.CharacterStuff;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using static Globals.Classes.Settings;

namespace EntityEngine.Classes.BehaviourStuff
{
    internal class SearchBehaviour : Behaviour
    {
        private Point _wanderRange;
        private Point? _tileHeadingTo;
        private int _gIDGoingTo = 5343; //pumpkin
        private Layers _layerOfTile = Layers.foreground;

        private bool _readyToInteract;
        public SearchBehaviour(Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, Point? wanderRange, float? timerFrequency)
            : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            //Default range is 5
            _wanderRange = wanderRange ?? new Point(5, 5);
        }

        public void SetGidToFind(int newGID)
        {
            _gIDGoingTo = newGID;
        }
        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if (Entity.IsInStage)
            {
                if (_readyToInteract)
                {
                    Entity.Halt();
                    Entity.FaceTowardsOtherEntity(_tileHeadingTo.Value.ToVector2());

                    if (Entity.IsFacingTowardsOtherEntity(_tileHeadingTo.Value.ToVector2()))
                    {
                        Entity.InteractWithTile(_tileHeadingTo.Value, _layerOfTile);
                        _tileHeadingTo = null;
                        _readyToInteract = false;
                    }
                    return;

                }
                if (!Navigator.HasActivePath)
                {
                    if (_gIDGoingTo > 0)
                    {

                        if (SimpleTimer.Run(gameTime))
                        {
                            List<Point> tilePoints = GetTilePoints(_gIDGoingTo, _layerOfTile);
                            if (tilePoints == null)
                            {
                                StatusIcon.SetStatus(StatusIconType.NoPath);
                                return;
                            }
                            //Gets point of tile trying to find. Can't walk into tile so need to find a vacant point next to one.
                            List<Point> emptyPoints = new List<Point>();
                            int shortestDistance = -1;
                            Point? shortestPoint = null;
                            foreach (Point point in tilePoints)
                            {
                                Point? nearestClearPoint = Navigator.NearestClearPoint(point, 3);
                                if (nearestClearPoint != null)
                                {
                                    int distance = Navigator.PathDistance(point, nearestClearPoint.Value);
                                    if((shortestDistance >= 0 && distance < shortestDistance) || shortestDistance < 0)
                                    {
                                        shortestPoint = nearestClearPoint;
                                        
                                    }
                                }
                            }
                         
                            if (shortestPoint == null)
                            {
                                StatusIcon.SetStatus(StatusIconType.NoPath);
                                return;
                            }
                            else
                            {
                                _tileHeadingTo = shortestPoint.Value;
                                Vector2 tilePos = Vector2Helper.GetWorldPositionFromTileIndex(shortestPoint.Value.X, shortestPoint.Value.Y);
                                if (Navigator.FindPathTo(Entity.Position, tilePos))
                                {
                                    Navigator.SetTarget(tilePos);
                                }
                            }








                        }
                    }

                }
                if (Navigator.HasActivePath)
                {
                    if (Navigator.FollowPath(gameTime, Entity.Position, ref velocity))
                    {
                        _readyToInteract = true;

                    }

                }
                else
                {
                    _tileHeadingTo = null;

                    Entity.Halt();

                }
            }

        }

    }
}
