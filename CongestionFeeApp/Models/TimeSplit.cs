using System;
using System.Collections.Generic;

namespace CongestionFeeApp.Models
{
    public class TimeSplit
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DayOfWeek WeekDay { get; set; }

        public List<TimeSpan> Thresholds { get; set; }

        public override string ToString()
        {
            return $"{StartTime}, {EndTime}, {WeekDay}";
        }
    }
}