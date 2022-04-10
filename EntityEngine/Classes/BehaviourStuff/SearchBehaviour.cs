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
        private int _gIDGoingTo;
        private Layers _layerOfTile;
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
            if (!Navigator.HasActivePath)
            {
                if (_gIDGoingTo > 0)
                {

                    if (SimpleTimer.Run(gameTime))
                    {
                        Point? tilePoint = GetTilePoint(_gIDGoingTo, _layerOfTile);
                        if(tilePoint == null)
                        {
                            StatusIcon.SetStatus(StatusIconType.NoPath);
                            return;
                        }
                        Vector2 tilePos =  Vector2Helper.GetWorldPositionFromTileIndex(tilePoint.Value.X, tilePoint.Value.Y);
                        if (Navigator.FindPathTo(Entity.Position, tilePos))
                        {
                            Navigator.SetTarget(tilePos);
                        }
                    }
                }

            }
            if (Navigator.HasActivePath)
                Navigator.FollowPath(gameTime, Entity.Position, ref velocity);
            else
                Entity.Halt();
        }
        
    }
}
