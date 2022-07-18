using DataModels;
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
using static DataModels.Enums;

namespace EntityEngine.Classes.BehaviourStuff.Agression
{
    internal class ChaseAndAttackBehaviour : Behaviour
    {
        private readonly Entity _otherEntity;
        public ChaseAndAttackBehaviour(Entity entity,Entity otherEntity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency) : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            _otherEntity = otherEntity;

            SimpleTimer.SetNewTargetTime(.25f);
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if (!Navigator.HasActivePath && Vector2.Distance(Entity.Position, _otherEntity.CenteredPosition) > 18)
            {
                if (SimpleTimer.Run(gameTime))
                {
                    if (Navigator.FindPathTo(Entity.Position, _otherEntity.CenteredPosition ))
                    {
                        Navigator.SetTarget(_otherEntity.CenteredPosition );
                    }
                }
            }
            if (Navigator.HasActivePath)
            {
                Navigator.FollowPath(gameTime, Entity.Position, ref velocity);
                if(Entity.Animator.CurrentActionType != ActionType.Walking)
                {
                    Entity.Animator.PerformAction(Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position, _otherEntity.CenteredPosition), ActionType.Walking);

                }
            }
            else
            {
                if (Entity.Animator.CurrentActionType != ActionType.Attack)
                {
                    Entity.Animator.PerformAction(Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position, _otherEntity.CenteredPosition), ActionType.Attack);
                    Entity.Halt();
                }
                else
                {
                    //keep playing attack animation
                    Entity.Animator.OverridePause = true;

                }


            }
        }
    
    }
}
