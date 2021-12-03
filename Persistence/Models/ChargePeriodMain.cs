using Contracts.Enums;
using System;
using System.Collections.Generic;

namespace Persistence.Models
{
    public class ChargePeriodMain
    {
        public Guid Id { get; set; }
        public string Alias { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public Dictionary<VehicleType, double> FeeList { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}