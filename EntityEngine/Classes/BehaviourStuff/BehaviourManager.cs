using DataModels;
using DataModels.ScriptedEventStuff;
using EntityEngine.Classes.BehaviourStuff.Agression;
using EntityEngine.Classes.BehaviourStuff.PatronStuff;
using EntityEngine.Classes.CharacterStuff;
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
using TiledEngine.Classes;


namespace EntityEngine.Classes.BehaviourStuff
{
    public class BehaviourManager : IDebuggable
    {
        private Entity _entity;
        private StatusIcon _statusIcon;
        private Navigator _navigator;
        private TileManager _tileManager;
        private SimpleTimer _simpleTimer;
        private Schedule _activeSchedule;

        protected Behaviour CurrentBehaviour { get; set; }
        public BehaviourManager(Entity entity, StatusIcon statusIcon, Navigator navigator, TileManager tileManager)
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

        public void ChaseAndAttack(Entity otherEntity)
        {
            CurrentBehaviour = new ChaseAndAttackBehaviour(_entity, otherEntity, _statusIcon, _navigator, _tileManager, 2f);
        }

        public Behaviour ChangeBehaviour(EndBehaviour newbehaviour)
        {
            switch (newbehaviour)
            {
                case EndBehaviour.None:
                    return null;
                case EndBehaviour.Stationary:
                    return null;
                case EndBehaviour.Wander:
                    return new WanderBehaviour(_entity, _statusIcon, _navigator, _tileManager, new Point(5, 5), 2f);

                case EndBehaviour.Search:
                    return new SearchBehaviour(_entity, _statusIcon, _navigator, _tileManager, new Point(5, 5), 2f);

                case EndBehaviour.Patron:
                    return new PatronBehaviourManager(_entity, _statusIcon, _navigator, _tileManager, 2f);

                case EndBehaviour.CustomScript:
                    ScriptBehaviour behaviour = new ScriptBehaviour(_entity, _statusIcon, _navigator, _tileManager, 2f);
                    behaviour.InjectSubscript(EntityFactory.GetSubscript(_activeSchedule.CustomScriptName));
                    return behaviour;


                default:
                    return null;
            }
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
                CurrentBehaviour = ChangeBehaviour(_activeSchedule.EndBehaviour);
            }
        }

        public void InjectScript(SubScript subscript)
        {
            CurrentBehaviour = new ScriptBehaviour(_entity, _statusIcon, _navigator, _tileManager, 2f);
            (CurrentBehaviour as ScriptBehaviour).InjectSubscript(subscript);
        }
        private void CheckForUpdatedSchedule()
        {
            Schedule newSchedule = Scheduler.GetScheduleFromCurrentTime(_entity.ScheduleName);
            if(_activeSchedule != newSchedule)
            {
                _activeSchedule = newSchedule;

                    CurrentBehaviour = ChangeBehaviour(_activeSchedule.EndBehaviour);

                
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
    }
}

