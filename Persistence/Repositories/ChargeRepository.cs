using Contracts.Enums;
using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Repositories
{
    public class ChargeRepository : IChargeRepository
    {
        private readonly List<TimeSpan> _periodThresholds = new List<TimeSpan>();
        private readonly List<ChargePeriodMain> _periods = new List<ChargePeriodMain>();

        public ChargePeriodMain NewPeriod { get; set; }

        public string[] SavedPeriods { get; set; }

        public ChargeRepository()
        {
            var amPeriod = new ChargePeriodMain
            {
                Id = Guid.NewGuid(),
                Alias = "AM rate",
                Start = new TimeSpan(7, 0, 0),
                End = new TimeSpan(12, 0, 0),
                FeeList = new Dictionary<VehicleType, double>()
            };

            amPeriod.FeeList.Add(VehicleType.Car, 2.00);
            amPeriod.FeeList.Add(VehicleType.Motorbike, 1.00);

            _periods.Add(amPeriod);

            var pmPeriod = new ChargePeriodMain
            {
                Id = Guid.NewGuid(),
                Alias = "PM rate",
                Start = new TimeSpan(12, 0, 0),
                End = new TimeSpan(19, 0, 0),
                FeeList = new Dictionary<VehicleType, double>()
            };

            pmPeriod.FeeList.Add(VehicleType.Car, 2.50);
            pmPeriod.FeeList.Add(VehicleType.Motorbike, 1.00);

            _periods.Add(pmPeriod);

            var allPeriodThresholds = new List<TimeSpan> { };

            foreach (var range in _periods)
            {
                allPeriodThresholds.Add(range.Start);
                allPeriodThresholds.Add(range.End);
            }

            var periodThresholds = allPeriodThresholds
                .GroupBy(p => p)
                .Select(g => g.First())
                .ToList();

            _periodThresholds = periodThresholds;
        }

        public double GetRates(TimeSpan startOfPeriod, VehicleType type)
        {
            double rate;

            _periods
                 .FirstOrDefault(d => d.Start == startOfPeriod)
                 .FeeList.TryGetValue(type, out rate);

            return rate;
        }

        public List<TimeSpan> GetPeriodThresholds()
        {
            return _periodThresholds;
        }

        public List<ChargePeriodMain> GetChargeRanges()
        {
            return _periods;
        }
    }
}