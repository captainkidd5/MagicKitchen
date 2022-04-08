using EntityEngine.Classes.CharacterStuff;
using Globals.Classes;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityEngine.Classes.BehaviourStuff
{
    internal class WanderBehaviour : Behaviour
    {
        private Point _wanderRange;
        public WanderBehaviour(Entity entity, StatusIcon statusIcon, Navigator navigator, Point? wanderRange, float? timerFrequency) : base(entity, statusIcon, navigator, timerFrequency)
        {
            //Default range is 5
            _wanderRange = wanderRange ?? new Point(5, 5);
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if (!Navigator.HasActivePath)
            {
                if (SimpleTimer.Run(gameTime))
                {
                    Vector2 newRandomPos = GetNewWanderPosition();
                    if (Navigator.FindPathTo(Entity.Position, newRandomPos))
                    {
                        Navigator.SetTarget(newRandomPos);
                    }
                }
            }
            if (Navigator.HasActivePath)
                Navigator.FollowPath(gameTime, Entity.Position, ref velocity);
            else
                Entity.Halt();
        }
        private Vector2 GetNewWanderPosition()
        {
            int lowX = Entity.TileOn.X;
            if (lowX - _wanderRange.X < 0)
                lowX = 0;
            else
                lowX -= _wanderRange.X;

            int highX = Entity.TileOn.X;
            if (highX + _wanderRange.X > Navigator.MaxValue - 1)
                highX = Navigator.MaxValue - 1;
            else 
                highX += _wanderRange.X;

            int lowY = Entity.TileOn.Y;
            if (lowY - _wanderRange.Y < 0)
                lowY = 0;
            else
                lowY -= _wanderRange.Y;

            int highY = Entity.TileOn.Y;
            if (highY + _wanderRange.Y > Navigator.MaxValue - 1)
                highY = Navigator.MaxValue - 1;
            else
                highX += _wanderRange.Y;

            return new Vector2(Settings.Random.Next(lowX, highX) * Settings.TileSize,
                Settings.Random.Next(lowY, highY) * Settings.TileSize);
        }
    }
}
