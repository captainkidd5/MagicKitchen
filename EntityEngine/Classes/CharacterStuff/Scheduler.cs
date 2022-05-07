using DataModels;
using Globals.Classes;
using Globals.Classes.Helpers;
using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledEngine.Classes;

namespace EntityEngine.Classes.CharacterStuff
{
    internal static class Scheduler
    {
       
        /// <summary>
        /// Retrieves the schedule which is closest to, but not less than the current time.
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        public static Schedule GetScheduleFromCurrentTime(string entityName)
        {
            List<Schedule> schedules = EntityFactory.GetSchedules(entityName);
            TimeSpan clockHours = TimeSpan.FromHours(Clock.TimeKeeper.Hours);
            TimeSpan clockMinutes = TimeSpan.FromMinutes(Clock.TimeKeeper.Minutes);
            clockHours = clockHours.Add(clockMinutes);
            //TODO: Filter more

            int closestIndex = schedules.BinarySearch(new Schedule() { StartTime = clockHours }, new ScheduleTimeComparer());
            closestIndex = closestIndex >= 0 ? closestIndex : 0;
            return schedules[closestIndex];
        }

        /// <summary>
        /// Note: this DOES NOT account for the stage being different than the current stage.
        /// </summary>
        /// <param name="currentEntityStage"></param>
        /// <param name="schedule"></param>
        /// <param name="tileManager"></param>
        /// <returns></returns>
        public static Vector2 GetTargetFromSchedule(Schedule schedule)
        {
            return Vector2Helper.GetWorldPositionFromTileIndex(schedule.TileX, schedule.TileY);

        }

        

        public class ScheduleTimeComparer : IComparer<Schedule>
        {
            public int Compare(Schedule x, Schedule y)
            {
                return x.StartTime.CompareTo(y.StartTime);
            }
        }
    }
}
