using DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globals.Classes.Time
{
    
    public class TimeKeeper
    {
        public Month Month { get; internal set; } = Month.Jan;
        public DayOfWeek DayOfWeek { get; internal set; }
        public int Days { get; internal set; } = 1;
        public int Hours { get; internal set; }
        public int Minutes { get; internal set; }
    }
}
