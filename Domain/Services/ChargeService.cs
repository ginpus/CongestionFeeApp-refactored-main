using Contracts.Enums;
using Contracts.Models;
using Domain.Models;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services
{
    public class ChargeService : IChargeService
    {
        public StartEndValidator Validator { get; set; } = new StartEndValidator();

        private readonly IChargeRepository _chargeRepository;

        public ChargeService(IChargeRepository chargeRepository)
        {
            _chargeRepository = chargeRepository;
        }

        public List<PeriodTotalCharge> CalculateCharges(DateTime start, DateTime end, VehicleType vehicleType)
        {
            Validator.StartEndValidation(start, end);
            var totalDurations = CalculateChargePeriods(start, end);
            return SumFeeForEachPeriod(totalDurations, vehicleType);
        }

        public List<ChargePeriod> CalculateChargePeriods(DateTime start, DateTime end)
        {
            var chargeablePeriodThresholds = _chargeRepository.GetPeriodThresholds();
            var chargeablePeriods = _chargeRepository.GetChargeRanges();
            var daysToCharge = SplitTimeRangeIntoChargableDays(start, end);
            SpliChargableDaysIntoChargePeriods(daysToCharge, chargeablePeriodThresholds);
            var groupedPeriodsToCharge = SumAllChargePeriods(chargeablePeriodThresholds, daysToCharge, chargeablePeriods);

            return groupedPeriodsToCharge;
        }

        public List<TimeSplit> SplitTimeRangeIntoChargableDays(DateTime start, DateTime end)
        {
            List<TimeSplit> chargeDays = Enumerable.Range(0, (end.Date - start.Date).Days + 1)
                  .Select(d => new TimeSplit
                  {
                      StartTime = Max(start.Date.AddDays(d), start),
                      EndTime = Min(start.Date.AddDays(d + 1).AddMilliseconds(-1), end),
                      WeekDay = Max(start.Date.AddDays(d), start).DayOfWeek,
                      Thresholds = new List<TimeSpan> { }
                  })
                  .Where(d => IsChargeableDay((int)d.WeekDay))
                  .ToList();
            return chargeDays;
        }

        public List<TimeSplit> SpliChargableDaysIntoChargePeriods(List<TimeSplit> chargeDays, List<TimeSpan> periodThresholds)
        {
            for (var i = 0; i < chargeDays.Count; i++)
            {
                for (var j = 0; j < periodThresholds.Count; j++)
                {
                    chargeDays[i].Thresholds.Add(periodThresholds[j]);
                }
                if (i == 0)
                {
                    chargeDays[i].Thresholds
                        .RemoveAll(d => d <= chargeDays[i].StartTime.TimeOfDay);
                    chargeDays[i].Thresholds.Add(chargeDays[i].StartTime.TimeOfDay);
                }
                if (i == chargeDays.Count - 1)
                {
                    chargeDays[i].Thresholds
                        .RemoveAll(d => d >= chargeDays[i].EndTime.TimeOfDay);
                    chargeDays[i].Thresholds.Add(chargeDays[i].EndTime.TimeOfDay);
                }
                chargeDays[i].Thresholds = chargeDays[i].Thresholds
                    .OrderBy(t => t.TotalMinutes).ToList();
            }
            return chargeDays;
        }

        public List<ChargePeriod> SumAllChargePeriods(
            List<TimeSpan> chargeablePeriodThresholds,
            List<TimeSplit> daysToCharge,
            List<ChargePeriodMain> chargeablePeriods
            )
        {
            var periodDuration = new List<List<TimeSpan>>();
            var totalDuration = new List<TimeSpan>();

            for (var k = 0; k < chargeablePeriodThresholds.Count - 1; k++)
            {
                periodDuration.Add(new List<TimeSpan>());
                for (var i = 0; i < daysToCharge.Count; i++)
                {
                    for (var l = 0; l < daysToCharge[i].Thresholds.Count - 1; l++)
                    {
                        if (daysToCharge[i].Thresholds[l].TotalMinutes >= chargeablePeriodThresholds[k].TotalMinutes &&
                           daysToCharge[i].Thresholds[l].TotalMinutes < chargeablePeriodThresholds[k + 1].TotalMinutes)
                        {
                            var chargePeriodDuration = daysToCharge[i].Thresholds[l + 1] - daysToCharge[i].Thresholds[l];
                            periodDuration[k].Add(chargePeriodDuration);
                        }
                    }
                }
                totalDuration.Add(new TimeSpan(periodDuration[k].Sum(r => r.Ticks)));
                chargeablePeriods[k].TotalDuration = new TimeSpan(periodDuration[k].Sum(r => r.Ticks));
            }

            return chargeablePeriods
                .Select(c => c.AsDto())
                .ToList();
        }

        public List<PeriodTotalCharge> SumFeeForEachPeriod(List<ChargePeriod> totalDurations, VehicleType vehicleType)
        {
            var results = new List<PeriodTotalCharge>();
            foreach (var chargePeriod in totalDurations)
            {
                var rate = _chargeRepository.GetRates(chargePeriod.Start, vehicleType);
                var duration = chargePeriod.TotalDuration.TotalMinutes;
                var sum = Math.Floor((rate * duration / 60) * 10) / 10;
                results.Add(new PeriodTotalCharge
                {
                    Alias = chargePeriod.Alias,
                    TotalDuration = chargePeriod.TotalDuration,
                    TotalCharge = sum
                });
            }
            return results;
        }

        public void GetDefaultChargeValues()
        {
            var chargeThresholds = _chargeRepository.GetPeriodThresholds();

            foreach (var chargeThreshold in chargeThresholds)
            {
                Console.WriteLine(chargeThreshold);
            }
        }

        private static DateTime Min(DateTime a, DateTime b)
        {
            if (a > b)
                return b;
            return a;
        }

        private static DateTime Max(DateTime a, DateTime b)
        {
            if (a < b)
                return b;
            return a;
        }

        private static bool IsChargeableDay(int dayOfWeek)
        {
            return Enum.IsDefined(typeof(ChargeableDay), dayOfWeek);
        }

        public double CalculateTotalCharge(List<PeriodTotalCharge> periodTotalCharges)
        {
            double totalCharge = 0;
            foreach (var entry in periodTotalCharges)
            {
                totalCharge += entry.TotalCharge;
            };
            return totalCharge;
        }
    }
}