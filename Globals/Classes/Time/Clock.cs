using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        public static SimpleTimer SimpleTimer;

        public static TimeKeeper TimeKeeper;

        public static bool Paused;

        public static Interval Interval { get; private set; }
        public static void Load()
        {
            DayStatus = DayStatus.DayTime;
            SimpleTimer = new SimpleTimer(clockSpeed);
            TimeKeeper = new TimeKeeper();
            Interval = new Interval(100);
        }
        public static void OnClockTimeChanged(TimeKeeper timeKeeper)
        {
            ClockTimeChanged?.Invoke(timeKeeper);
        }
        public static void Update(GameTime gameTime)
        {
            if (!Paused)
            {
                if (SimpleTimer.Run(gameTime))
                {
                    IncrementTime();
                }

                
                    Interval.Update(gameTime);
                
            }
        }

        public static void SubscribeToInterval(ITimerSubscribeable obj)
        {
            Interval.TimerTargetReached += obj.TimerFrameChanged;
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
            OnClockTimeChanged(TimeKeeper);
        }
    }
}
