using DataModels;
using EntityEngine.Classes.CharacterStuff;
using EntityEngine.Classes.NPCStuff;
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
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using TextEngine.Classes;
using TiledEngine.Classes;

namespace EntityEngine.Classes.BehaviourStuff
{
    internal class RouteBehaviour : Behaviour
    {
        private Schedule _activeSchedule;
   


        public RouteBehaviour(BehaviourManager behaviourManager, NPC entity,StatusIcon statusIcon,TileManager tileManager,
            Schedule activeSchedule, float? scheduleCheckFrequency) :
            base(behaviourManager,entity, statusIcon,tileManager, scheduleCheckFrequency)
        {
            _activeSchedule = activeSchedule;
        }

        public override void Update(GameTime gameTime, ref Vector2 velocity)
        {
            base.Update(gameTime, ref velocity);

            if (SimpleTimer.Run(gameTime))
            {
                CheckForUpdatedSchedule();
            }
            if (Entity.HasActivePath)
                Entity.FollowPath(gameTime,  ref velocity);
            else
                Entity.Halt();

        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            base.DrawDebug(spriteBatch);
         

        }

        private void CheckForUpdatedSchedule()
        {
            if (!Entity.HasActivePath)
            {
                _activeSchedule = Scheduler.GetScheduleFromCurrentTime(Entity.Name);


                Vector2 targetpos = Scheduler.GetTargetFromSchedule( _activeSchedule);

                base.GetPath(targetpos);

                if (Vector2Helper.WithinRangeOf(Entity.Position, targetpos))
                {
                    return;
                }

            }

        }

        public bool HasReachedEndOfScheduledRoute()
        {
            return Vector2Helper.WithinRangeOf(Entity.Position, Scheduler.GetTargetFromSchedule(_activeSchedule), 16);
        }



        public override bool OnCollides(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.OnCollides(fixtureA, fixtureB, contact);
        }

    }
}
