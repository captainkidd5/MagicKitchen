using DataModels;
using EntityEngine.Classes.NPCStuff;
using Globals.Classes;
using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Classes.Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;
using TiledEngine.Classes;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;

namespace EntityEngine.Classes.BehaviourStuff
{
    internal class RouteBehaviour : Behaviour
    {
        private TileManager _tileManager;
        private Schedule _activeSchedule;
        private List<Schedule> _schedules;
        private SimpleTimer _simpleTimer;

        private float _scheduleCheckFrequency;
        public RouteBehaviour(Entity entity,StatusIcon statusIcon, Navigator navigator,TileManager tileManager, Schedule activeSchedule, List<Schedule> schedules, float? scheduleCheckFrequency) : base(entity,statusIcon, navigator)
        {
            _tileManager = tileManager;
            _activeSchedule = activeSchedule;
            _schedules = schedules;
            _scheduleCheckFrequency = scheduleCheckFrequency ?? 5f;
            _simpleTimer = new SimpleTimer(_scheduleCheckFrequency, true);
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);

            if (_simpleTimer.Run(gameTime))
            {
                CheckForUpdatedSchedule();
            }
            if (Navigator.HasActivePath)
                Navigator.FollowPath(gameTime, Entity.Position, ref velocity);
            else
                Entity.Halt();
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            base.DrawDebug(spriteBatch);
            if(Entity.IsInStage)
                Navigator.DrawDebug(spriteBatch, Color.Green);
            else
                Navigator.DrawDebug(spriteBatch, Color.Red);

        }

        private void CheckForUpdatedSchedule()
        {
            if (!Navigator.HasActivePath)
            {
                _activeSchedule = Scheduler.GetScheduleFromCurrentTime(_schedules);


                Vector2 targetpos = Scheduler.GetTargetFromSchedule(Entity.CurrentStageName, _activeSchedule, _tileManager);

                base.GetPath(targetpos, _activeSchedule.StageEndLocation);


                if (Navigator.WithinRangeOf(Entity.Position, targetpos))
                {
                    return;
                }

            }

        }

        public override void OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            base.OnCollides(fixtureA, fixtureB, contact);
        }

    }
}
