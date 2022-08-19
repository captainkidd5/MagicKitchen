﻿using DataModels;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
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
        public ChaseAndAttackBehaviour(BehaviourManager behaviourManager,NPC entity, NPC otherEntity, StatusIcon statusIcon, TileManager tileManager, float? timerFrequency) :
            base(behaviourManager, entity, statusIcon, tileManager, timerFrequency)
        {
            _otherEntity = otherEntity;
            SimpleTimer.SetNewTargetTime(.25f);
            _otherEntity.TilePositionChanged += OnChasedEntityPointChanged;
        }

        private void OnChasedEntityPointChanged(Point otherPoint)
        {
            Entity.ResetNavigator();
            if (Entity.FindPathTo(_otherEntity.CenteredPosition))
            {
                Entity.SetNavigatorTarget(_otherEntity.CenteredPosition);
            }
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            Entity.Speed = Entity.BaseSpeed * 2.5f;

            base.Update(gameTime, ref velocity);
            if (Entity.DamageBody.Body.FixtureList[0].CollidesWith > tainicom.Aether.Physics2D.Dynamics.Category.None)
            {
                Entity.DeactivateDamageBody();
            }
            if (!Entity.HasActivePath && Vector2.Distance(Entity.Position, _otherEntity.CenteredPosition) > _distanceToReschase)
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
                    if (Entity.FindPathTo(_otherEntity.CenteredPosition))
                    {
                        Entity.SetNavigatorTarget(_otherEntity.CenteredPosition);
                    }
                    else
                    {
                        Vector2Helper.MoveTowardsVector(_otherEntity.CenteredPosition, Entity.Position, ref velocity, gameTime, 15);

                    }

                }
            }
            if (IsStuck(gameTime))
            {
                Entity.ResumeDefaultBehaviour();
                _otherEntity.TilePositionChanged -= OnChasedEntityPointChanged;

                return;
            }
            if (Entity.HasActivePath && !Entity.Animator.IsPerformingAnimation())
            {
                Entity.FollowPath(gameTime, ref velocity);
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
            }
            else
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
                            Entity.ActivateDamageBody(null);
                        }
                    }
                }



            }
        }

        public override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            if (fixtureB.Body.Tag == _otherEntity)
            {
                //Todo, check to make sure entity is within a certain radius of other entity
                if (Entity.Animator.CurrentActionType != ActionType.Attack)
                {
                    Entity.Animator.PerformAction(null, Vector2Helper.GetDirectionOfEntityInRelationToEntity(Entity.Position,
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
            return base.OnCollides(fixtureA, fixtureB, contact);
        }
        public override bool OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureA.CollisionCategories.HasFlag((Category)PhysCat.ArraySensor))
            {
                if (fixtureB.Tag == _otherEntity)
                {
                    Entity.ResumeDefaultBehaviour();
                    _otherEntity.TilePositionChanged -= OnChasedEntityPointChanged;
                }
            }
            return base.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}
