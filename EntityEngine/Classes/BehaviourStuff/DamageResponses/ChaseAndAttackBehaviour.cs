using DataModels;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledEngine.Classes;
using static DataModels.Enums;

namespace EntityEngine.Classes.BehaviourStuff.DamageResponses
{
    internal class ChaseAndAttackBehaviour : Behaviour
    {
        private readonly Entity _otherEntity;
        //When entity is this distance away from chased entity, stop attacking and chase again (basically, outside of hit zone)
        private int _distanceToReschase = 18;
        public ChaseAndAttackBehaviour(Entity entity, Entity otherEntity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager, float? timerFrequency) : base(entity, statusIcon, navigator, tileManager, timerFrequency)
        {
            _otherEntity = otherEntity;

            SimpleTimer.SetNewTargetTime(.25f);
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);
            if (Entity.DamageBody.Body.FixtureList[0].CollidesWith > tainicom.Aether.Physics2D.Dynamics.Category.None)
            {
                Entity.DeactivateDamageBody();
            }
            if (!Navigator.HasActivePath && Vector2.Distance(Entity.Position, _otherEntity.CenteredPosition) > _distanceToReschase)
            {
                if (SimpleTimer.Run(gameTime))
                {
                    if (Navigator.FindPathTo(Entity.Position, _otherEntity.CenteredPosition))
                    {
                        Navigator.SetTarget(_otherEntity.CenteredPosition);
                    }
                }
            }
            if (Navigator.HasActivePath && ! Entity.Animator.IsPerformingAnimation())
            {
                Navigator.FollowPath(gameTime, Entity.Position, ref velocity);
                if (Entity.Animator.CurrentActionType != ActionType.Walking)
                {
                    Entity.Animator.PerformAction(Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position,
                        _otherEntity.CenteredPosition), ActionType.Walking);

                }
            }
            else
            {
               
                if (Entity.Animator.CurrentActionType != ActionType.Attack)
                {
                    Entity.Animator.PerformAction(Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position,
                        _otherEntity.CenteredPosition), ActionType.Attack);
                    Entity.Halt();
                }
                else
                {
                    //keep playing attack animation
                    Entity.Animator.OverridePause = true;

                    if (((Entity as NPC).Animator as NPCAnimator).IsAttackFrame)
                    {

                        if (Entity.DamageBody.Body.FixtureList[0].CollidesWith <= tainicom.Aether.Physics2D.Dynamics.Category.None)
                        {
                            Entity.ActivateDamageBody(null);
                        }
                    }
              
                }


            }
        }

    }
}
