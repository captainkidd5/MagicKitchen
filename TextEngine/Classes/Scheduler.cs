using DataModels;
using Globals.Classes.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextEngine.Classes
{
    public static class Scheduler
    {
        internal static Dictionary<string, List<Schedule>> Schedules;
        public static void Load(ContentManager content)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string basePath = content.RootDirectory + "/Entities/Schedules";



            Schedules = new Dictionary<string, List<Schedule>>();
            foreach (var file in Directory.GetFiles(basePath))
            {
                string jsonString = File.ReadAllText(file);
                List<Schedule> schedules = JsonSerializer.Deserialize<List<Schedule>>(jsonString, options);

                //foreach (Schedule sch in schedules)
                //    sch.ConvertTimeString();

                //schedules.Sort(0, schedules.Count, new ScheduleTimeComparer());
                Schedules.Add(Path.GetFileName(file).Split(".json")[0], schedules);

            }

        }

        /// <summary>
        /// Retrieves the schedule which is closest to, but not less than the current time.
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        public static Schedule GetScheduleFromCurrentTime(string entityName)
        {

            return Schedules[entityName.ToLower()].FirstOrDefault(x => x.DayStatus == Clock.DayStatus || x.DayStatus == Enums.DayStatus.Any);


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
            //TODO Reimplement with zones
            return Vector2.Zero;
            // return Vector2Helper.GetWorldPositionFromTileIndex(schedule.TileX, schedule.TileY);

        }
    }
}
