﻿using DataModels;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
using EntityEngine.Classes.PlayerStuff;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using PhysicsEngine.Classes;
using PhysicsEngine.Classes.Pathfinding;
using SpriteEngine.Classes.Animations.EntityAnimations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TiledEngine.Classes;
using static DataModels.Enums;

namespace EntityEngine.Classes.BehaviourStuff.DamageResponses
{
    internal class ChaseAndAttackBehaviour : Behaviour
    {
        private readonly NPC _otherEntity;
        //When entity is this distance away from chased entity, stop attacking and chase again (basically, outside of hit zone)
        private int _distanceToReschase = 18;

        public bool InAttackRange
        {
            get;
            set;
        }
        public ChaseAndAttackBehaviour(BehaviourManager behaviourManager, NPC entity, NPC otherEntity, StatusIcon statusIcon, TileManager tileManager, float? timerFrequency) :
            base(behaviourManager, entity, statusIcon, tileManager, timerFrequency)
        {
            _otherEntity = otherEntity;
            SimpleTimer.SetNewTargetTime(4f);
            _otherEntity.TilePositionChanged += OnChasedEntityPointChanged;
        }

        private void OnChasedEntityPointChanged(Point otherPoint)
        {
            Entity.ResetNavigator();

            if (_otherEntity.GetType() != typeof(Player))
            {

                return;
            }
            if (Entity.FindPathTo(_otherEntity.CenteredPosition))
            {
                Entity.SetNavigatorTarget(_otherEntity.CenteredPosition);
            }
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            Entity.Speed = Entity.BaseSpeed * 3f;

            base.Update(gameTime, ref velocity);
            if(_otherEntity.FlaggedForRemoval)
            {
                Entity.ResumeDefaultBehaviour();
                _otherEntity.TilePositionChanged -= OnChasedEntityPointChanged;
            }
            if (Entity.DamageBody.Body.FixtureList[0].CollidesWith > tainicom.Aether.Physics2D.Dynamics.Category.None)
            {
                Entity.DeactivateDamageBody();
            }
     
            if (InAttackRange)
            {
                //Todo, check to make sure entity is within a certain radius of other entity
                if (Entity.Animator.CurrentActionType == ActionType.Attack)
                {
                    //keep playing attack animation
                    Entity.Animator.OverridePause = true;

                    if (((Entity as NPC).Animator as NPCAnimator).JustChangedToAttackFrame)
                    {

                        if (Entity.DamageBody.Body.FixtureList[0].CollidesWith <= tainicom.Aether.Physics2D.Dynamics.Category.None)
                        {
                            Entity.ActivateDamageBody(new List<Category>() { (Category)PhysCat.Player, (Category)PhysCat.NPC });

                        }
                    }
                }
                else
                {
                    Entity.Animator.PerformAction(null, Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position,
                      _otherEntity.CenteredPosition), ActionType.Attack);
                    Entity.Halt();
                }
                return;
            }
            else
            {
                if (IsStuck(gameTime))
                {
                    Entity.ResumeDefaultBehaviour();
                    _otherEntity.TilePositionChanged -= OnChasedEntityPointChanged;

                    return;
                }
                if (Entity.Animator.CurrentActionType != ActionType.Walking)
                {
                    Entity.Animator.PerformAction(null, Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position,
                        _otherEntity.CenteredPosition), ActionType.Walking);

                }

                if (_otherEntity.GetType() != typeof(Player))
                {
                    Vector2Helper.MoveTowardsVector(_otherEntity.CenteredPosition, Entity.Position, ref velocity, gameTime, 15);

                }

                else if (!Entity.HasActivePath && Vector2.Distance(Entity.Position, _otherEntity.CenteredPosition) > _distanceToReschase)
                {
                    if (SimpleTimer.Run(gameTime))
                    {
                        if (Entity.NPCData != null)
                        {
                            if (Entity.NPCData.AlwaysSubmerged)
                            {

                            }
                            if (Entity.IsWater(_otherEntity.CenteredPosition))
                            {

                            }
                        }
                        if (Entity.FindPathTo(_otherEntity.CenteredPosition) && _otherEntity.GetType() == typeof(Player))
                        {
                            Entity.SetNavigatorTarget(_otherEntity.CenteredPosition);
                        }
                        else
                        {
                            Vector2Helper.MoveTowardsVector(_otherEntity.CenteredPosition, Entity.Position, ref velocity, gameTime, 15);

                        }

                    }
                }
            
                if (Entity.HasActivePath && !Entity.Animator.IsPerformingAnimation())
                {

                    Entity.FollowPath(gameTime, ref velocity);

                    if (Entity.Animator.CurrentActionType != ActionType.Walking)
                    {
                        Entity.Animator.PerformAction(null, Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position,
                            _otherEntity.CenteredPosition), ActionType.Walking);

                    }
                }
            }
          
  
        }
   
        public override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            if (fixtureB.Body.Tag == _otherEntity && fixtureA.CollisionCategories == (Category)PhysCat.NPCBigSensor)
            {
                //Todo, check to make sure entity is within a certain radius of other entity
                if (Entity.Animator.CurrentActionType != ActionType.Attack)
                {
                    //Entity.Animator.PerformAction(null, Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position,
                    //    _otherEntity.CenteredPosition), ActionType.Attack);
                    //Entity.Halt();
                    InAttackRange = true;

                }

            }
            return base.OnCollides(fixtureA, fixtureB, contact);
        }
        public override bool OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            if (fixtureA.CollisionCategories == (Category)PhysCat.NPCBigSensor && fixtureB.Body.Tag == _otherEntity)
            {

                if(_otherEntity == this.Entity)
                    Console.WriteLine("test");
                InAttackRange = false;
                //Entity.ResumeDefaultBehaviour();
                //_otherEntity.TilePositionChanged -= OnChasedEntityPointChanged;
            }

            return base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
