using System;

namespace Contracts.Models
{
    public class PeriodTotalCharge
    {
        public string Alias { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public double TotalCharge { get; set; }

        public override string ToString()
        {
            return $"Charge for {Math.Floor(TotalDuration.TotalHours)}h {TotalDuration.Minutes}m ({Alias}): £{TotalCharge.ToString("0.00")}";
        }
    }
}