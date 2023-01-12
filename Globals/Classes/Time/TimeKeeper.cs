using DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Globals.Classes.Time
{
    
    public class TimeKeeper : ISaveable
    {
        public Month Month { get; internal set; } = Month.Jan;
        public DayOfWeek DayOfWeek { get; internal set; }
        public int Days { get; internal set; } = 1;
        public int Hours { get; internal set; }
        public int Minutes { get; internal set; }

        public void SetToDefault()
        {
            Month = Month.Jan;
            DayOfWeek = DayOfWeek.Sunday;
            Days = 1;
            Hours = 0;
            Minutes = 0;

        }
        public void Save(BinaryWriter writer)
        {
            writer.Write((int)Month);
            writer.Write((int)DayOfWeek);
            writer.Write((int)Days);
            writer.Write((int)Hours);
            writer.Write((int)Minutes);
        }
        public void LoadSave(BinaryReader reader)
        {
            Month = (Month)reader.ReadInt32();
            DayOfWeek = (DayOfWeek)reader.ReadInt32();
            Days = reader.ReadInt32();
            Hours = reader.ReadInt32();
            Minutes = reader.ReadInt32();
        }

      

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
