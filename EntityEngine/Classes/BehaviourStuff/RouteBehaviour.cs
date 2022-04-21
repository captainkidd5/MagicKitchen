using DataModels;
using EntityEngine.Classes.CharacterStuff;
using Globals.Classes;
using Globals.Classes.Console;
using Globals.Classes.Helpers;
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
   


        public string TargetLocation => _activeSchedule.StageEndLocation;
        public RouteBehaviour(Entity entity,StatusIcon statusIcon, Navigator navigator,TileManager tileManager,
            Schedule activeSchedule, List<Schedule> schedules, float? scheduleCheckFrequency) :
            base(entity,statusIcon, navigator,tileManager, scheduleCheckFrequency)
        {
            _tileManager = tileManager;
            _activeSchedule = activeSchedule;
            _schedules = schedules;
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);

            if (SimpleTimer.Run(gameTime))
            {
                CheckForUpdatedSchedule();
            }
            if (Navigator.HasActivePath)
                Navigator.FollowPath(gameTime, Entity.Position, ref velocity);
            else
                Entity.Halt();

            if(_activeSchedule != null)
            Entity.TargetStage = _activeSchedule.StageEndLocation;
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            base.DrawDebug(spriteBatch);
         

        }

        private void CheckForUpdatedSchedule()
        {
            if (!Navigator.HasActivePath)
            {
                _activeSchedule = Scheduler.GetScheduleFromCurrentTime(Entity.Name);


                Vector2 targetpos = Scheduler.GetTargetFromSchedule(Entity.CurrentStageName, _activeSchedule, _tileManager);

                base.GetPath(targetpos, _activeSchedule.StageEndLocation);
                CommandConsole.Append($"{Entity.Name} heading to : {_activeSchedule.StageEndLocation}");
                CommandConsole.Append($"{Entity.Name} current location : {Entity.CurrentStageName}");



                if (Vector2Helper.WithinRangeOf(Entity.Position, targetpos))
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
