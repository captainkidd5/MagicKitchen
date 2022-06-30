
using DataModels.DialogueStuff;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DataModels
{
    public enum Month
    {
        None = 0,
        Jan = 1,
        Feb = 2,
        Mar = 3,
        Apr = 4,
        May = 5,
        Jun = 6,
        Jul = 7,
        Aug = 8,
        Sep = 9,
        Oct = 10,
        Nov = 11,
        Dec = 12
    }

    public enum EndBehaviour
    {
        None = 0,
        Stationary = 1,
        Wander = 2,
        Search = 3,
        Patron = 4,
        CustomScript = 5,
    }
    public class Schedule
    {
        //Jan4,2:20_AM-3:40_AM
        public string Date { get; set; }

      
        public int TileX { get; set; }
        public int TileY { get; set; }

        ////Optional, mutually exclusive with tilex/tiley
        //public string ZoneName { get; set; }

        public EndBehaviour EndBehaviour { get; set; }

        public string CustomScriptName { get; set; }
        public Dialogue Dialogue { get; set; }


        [JsonIgnore]

        public Month Month { get; set; }

        [JsonIgnore]

        public int Day { get; set; }
        [JsonIgnore]

        public TimeSpan StartTime { get; set; }

        [JsonIgnore]
        public TimeSpan EndTime { get; set; }




        public void ConvertTimeString()
        {
            Month = ParseMonth(Date);
            Day = ParseDay(Date);
            TimeSpan[] times = ParseHours(Date);
            StartTime = times[0];
            EndTime = times[1];
        }

        private Month ParseMonth(string dateString)
        {
            string month = string.Empty;
            try
            {
                month = dateString.Split(',')[0].Remove(3);
            }
            catch
            {
                throw new Exception($"Datestring {dateString} is incorrect format");
            }
            Month result =Month.None;
            if(Enum.TryParse(month, out result)){
                return result;
            }
            else{
                 throw new Exception($"Unable to parse month {month}");
            }
        }

        private static int ParseDay(string dateString)
        {
            try
            {
                return int.Parse(dateString.Split(',')[0][3].ToString());
            }
            catch
            {
                throw new Exception($"Datestring {dateString} is incorrect format");
            }
        }
        /// <summary>
        /// Index 0 is start, index 1 is end
        /// </summary>
        private static TimeSpan[] ParseHours(string dateString)
        {
            TimeSpan[] times = new TimeSpan[2];
            try
            {
                string startTime = dateString.Split(',')[1].Split('-')[0].Replace('_', ' ');
                string endTime = dateString.Split(',')[1].Split('-')[1].Replace('_', ' ');

                DateTime sTime = DateTime.Parse(startTime);
                DateTime eTime = DateTime.Parse(endTime);



                times[0] = TimeSpan.FromHours(sTime.Hour) + TimeSpan.FromMinutes(sTime.Minute);
                times[1] = TimeSpan.FromHours(eTime.Hour) + TimeSpan.FromMinutes(eTime.Minute);

                return times;

            }
            catch
            {
                throw new Exception($"Datestring {dateString} is incorrect format");
            }
        }
    }
}
