using EntityEngine.Classes.CharacterStuff;
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

                            //Gets point of tile trying to find. Can't walk into tile so need to find a vacant point next to one.
                            Point? tilePoint = GetTilePoint(_gIDGoingTo, _layerOfTile);
                            if (tilePoint == null)
                            {
                                StatusIcon.SetStatus(StatusIconType.NoPath);
                                return;
                            }
                            _tileHeadingTo = tilePoint;
                            Point? nearestClearPoint = Navigator.NearestClearPoint(tilePoint.Value, 3);
                            if (nearestClearPoint != null)
                            {
                                Vector2 tilePos = Vector2Helper.GetWorldPositionFromTileIndex(nearestClearPoint.Value.X, nearestClearPoint.Value.Y);
                                if (Navigator.FindPathTo(Entity.Position, tilePos))
                                {
                                    Navigator.SetTarget(tilePos);
                                }
                            }
                            else
                            {
                                StatusIcon.SetStatus(StatusIconType.NoPath);
                                return;
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
