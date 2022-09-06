using DataModels;
using DataModels.ScriptedEventStuff;
using EntityEngine.Classes.BehaviourStuff.DamageResponses;
using EntityEngine.Classes.BehaviourStuff.PatronStuff;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
using Globals;
using Globals.Classes;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TextEngine.Classes;
using TiledEngine.Classes;


namespace EntityEngine.Classes.BehaviourStuff
{
    public class BehaviourManager : IDebuggable
    {
        private NPC _entity;
        private StatusIcon _statusIcon;
        private Navigator _navigator;
        private TileManager _tileManager;
        private SimpleTimer _simpleTimer;
        private Schedule _activeSchedule;

        protected Behaviour CurrentBehaviour { get; set; }
        public BehaviourManager(NPC entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager)
        {
            _entity = entity;
            _statusIcon = statusIcon;
            _navigator = navigator;
            _tileManager = tileManager;
        }

        public void Load()
        {
            _simpleTimer = new SimpleTimer(2f);
            CheckForUpdatedSchedule();


        }

        public void ChaseAndAttack(NPC otherEntity)
        {
            if (otherEntity == _entity)
                return;
            _navigator.Unload();
            CurrentBehaviour = new ChaseAndAttackBehaviour(this, _entity, otherEntity, _statusIcon, _tileManager, 2f);
        }
        public void Flee(Entity otherEntity)
        {
            CurrentBehaviour = new FleeBehaviour(this, _entity, otherEntity, _statusIcon, _tileManager, 2f);
        }
        public void ChangeBehaviour(EndBehaviour newbehaviour)
        {
            Behaviour behaviour = null;
            switch (newbehaviour)
            {
                case EndBehaviour.None:
                    return;
                case EndBehaviour.Stationary:
                    return;
                case EndBehaviour.Wander:
                    behaviour = new WanderBehaviour(this,_entity, _statusIcon, _tileManager, new Point(5, 5), 2f);
                    break;
                case EndBehaviour.Search:
                    behaviour = new SearchBehaviour(this, _entity, _statusIcon, _tileManager, new Point(5, 5), 2f);
                    break;
                case EndBehaviour.Patron:
                    behaviour = new PatronBehaviourManager(this, _entity, _statusIcon, _tileManager, 2f);
                    break;
                case EndBehaviour.CustomScript:
                    behaviour = new ScriptBehaviour(this, _entity, _statusIcon, _tileManager, 2f);
                    (behaviour as ScriptBehaviour).InjectSubscript(EntityFactory.GetSubscript(_activeSchedule.CustomScriptName));
                    break;


                default:
                    return;
            }

            CurrentBehaviour = behaviour;
        }
        public void Update(GameTime gameTime, ref Vector2 velocity)
        {
            _entity.Animator.OverridePause = false;
            if (_simpleTimer.Run(gameTime))
            {
                CheckForUpdatedSchedule();
            }

            CurrentBehaviour.Update(gameTime, ref velocity);
            CheckIfHasReachedEndOfRouteBehaviourAndAssignNewBehaviour();
        }

        private void CheckIfHasReachedEndOfRouteBehaviourAndAssignNewBehaviour()
        {
            if (CurrentBehaviour.GetType() == typeof(RouteBehaviour) &&
                           (CurrentBehaviour as RouteBehaviour).HasReachedEndOfScheduledRoute())
            {
               ChangeBehaviour(_activeSchedule.EndBehaviour);
            }
        }

        public void InjectScript(SubScript subscript)
        {
            CurrentBehaviour = new ScriptBehaviour(this, _entity, _statusIcon, _tileManager, 2f);
            (CurrentBehaviour as ScriptBehaviour).InjectSubscript(subscript);
        }
        private void CheckForUpdatedSchedule()
        {
            Schedule newSchedule = Scheduler.GetScheduleFromCurrentTime(_entity.ScheduleName);
            if(_activeSchedule != newSchedule)
            {
                _activeSchedule = newSchedule;

                    ChangeBehaviour(_activeSchedule.EndBehaviour);

                
            }

        }
        public void SwitchStage(TileManager tileManager)
        {
            _tileManager = tileManager;
            CurrentBehaviour.SwitchStage(tileManager);

        }
        public void DrawDebug(SpriteBatch spriteBatch)
        {
            CurrentBehaviour.DrawDebug(spriteBatch);
        }


        public virtual bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return CurrentBehaviour.OnCollides(fixtureA, fixtureB, contact);
        }

        public virtual bool OnSeparates(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return CurrentBehaviour.OnSeparates(fixtureA, fixtureB, contact);
        }
    }
}

