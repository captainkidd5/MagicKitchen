using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Globals.Classes.Time
{
    
    public enum DayStatus
    {
        DayTime = 1,
        NightTime = 2
    }
    public delegate void ClockTimeChanged(TimeKeeper timeKeeper);
    public static class Clock
    {
        public static event ClockTimeChanged ClockTimeChanged;
        public static readonly float clockSpeed = 5f;
        private static readonly int MaxMinutes = 60;
        private static readonly int MaxHours = 24;

        public static DayStatus DayStatus;

        public static SimpleTimer CalendarTimer;
        public static SimpleTimer GlobalCounterTimer;


        public static TimeKeeper TimeKeeper;

        //Increases by 1 every time minutes increase, never decreases
        public static float TotalTime;

        public static bool Paused;

        public static Dictionary<float, Interval> Intervals { get; private set; }
        public static void Load()
        {
            DayStatus = DayStatus.DayTime;
            CalendarTimer = new SimpleTimer(clockSpeed);
            GlobalCounterTimer = new SimpleTimer(.25f);
            TimeKeeper = new TimeKeeper();
            Intervals = new Dictionary<float, Interval>();
            for(int i = 0; i < 10; i++)
            {
                Intervals.Add((float)i /10,new Interval((float)i * .15f));
            }
        }

        public static void Save(BinaryWriter writer)
        {
            TimeKeeper.Save(writer);
            writer.Write(TotalTime);
        }
        public static void Load(BinaryReader reader)
        {
            TimeKeeper.LoadSave(reader);
            TotalTime = reader.ReadSingle();
        }
        public static void Update(GameTime gameTime)
        {
            if (!Paused)
            {
                if (CalendarTimer.Run(gameTime))
                {
                    IncrementTime();
                }
                if (GlobalCounterTimer.Run(gameTime))
                {
                    TotalTime++;

                }

                foreach (Interval interval in Intervals.Values)
                {
                    interval.Update(gameTime);

                }


            }
        }

        public static Interval GetInterval(float intervalDuration)
        {
            return Intervals[intervalDuration];
        }

        private static void IncrementTime(int minuteToIncrement = 10)
        {
            TimeKeeper.Minutes += minuteToIncrement;
            if(TimeKeeper.Minutes >= MaxMinutes)
            {
                TimeKeeper.Minutes = 0;
                TimeKeeper.Hours++;
                if(TimeKeeper.Hours >= MaxHours)
                {
                    TimeKeeper.Hours = 0;
                    TimeKeeper.Days++;
                    TimeKeeper.DayOfWeek++;
                    if(TimeKeeper.Days > 31)
                    {
                        TimeKeeper.Days = 0;
                        TimeKeeper.Month++;
                    }
                }
            }
           
            int maxValue = Enum.GetValues(typeof(DayOfWeek)).Cast<int>().Max();
            if ((int)TimeKeeper.DayOfWeek > maxValue)
                TimeKeeper.DayOfWeek = DayOfWeek.Sunday;
            ClockTimeChanged?.Invoke(TimeKeeper);
        }
    }
}
