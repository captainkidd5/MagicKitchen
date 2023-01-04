using DataModels;
using DataModels.DialogueStuff;
using Globals.Classes.Time;
using Globals.XPlatformHelpers;
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
            foreach (var file in AssetLocator.GetFiles(basePath))
            {

                using (var stream = TitleContainer.OpenStream($"{AssetLocator.GetStaticFileDirectory(basePath)}{file}"))
                {
                    using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    var str = reader.ReadToEnd();
                    List<Schedule> schedules = JsonSerializer.Deserialize<List<Schedule>>(str, options);
                    Schedules.Add(Path.GetFileName(file).Split(".json")[0], schedules);


                }

                Console.WriteLine("test");
                //foreach (Schedule sch in schedules)
                //    sch.ConvertTimeString();

                //schedules.Sort(0, schedules.Count, new ScheduleTimeComparer());

            }

        }

        /// <summary>
        /// Retrieves the schedule which is closest to, but not less than the current time.
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        public static Schedule GetScheduleFromCurrentTime(string entityName)
        {

            return Schedules[entityName].FirstOrDefault(x => x.DayStatus == Clock.DayStatus || x.DayStatus == Enums.DayStatus.Any);


        }

    }
}
