using EntityEngine.Classes.NPCStuff;
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
        public WanderBehaviour(Entity entity,StatusIcon statusIcon, Navigator navigator, Point? wanderRange) : base(entity,statusIcon, navigator)
        {
            //Default range is 5
            _wanderRange = wanderRange ?? new Point(5, 5);
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if (!Navigator.HasActivePath)
            {
                Vector2 newRandomPos = GetNewWanderPosition();
                if (Navigator.FindPathTo(Entity.Position, newRandomPos))
                {
                    Navigator.SetTarget(newRandomPos);
                }
            }
        }
        private Vector2 GetNewWanderPosition()
        {
            return new Vector2(Settings.Random.Next(0, Navigator.MaxValue - 1) * Settings.TileSize, Settings.Random.Next(0, Navigator.MaxValue - 1) * Settings.TileSize);
        }
    }
}
