using EntityEngine.Classes.CharacterStuff;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;

namespace EntityEngine.Classes.BehaviourStuff.Agression
{
    internal class ChaseAndAttackBehaviour : Behaviour
    {
        private readonly Entity _otherEntity;
        public ChaseAndAttackBehaviour(Entity entity,Entity otherEntity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency) : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            _otherEntity = otherEntity;


        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if (!Navigator.HasActivePath)
            {
                if (SimpleTimer.Run(gameTime))
                {
                    if (Navigator.FindPathTo(Entity.Position, _otherEntity.CenteredPosition))
                    {
                        Navigator.SetTarget(_otherEntity.CenteredPosition);
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
