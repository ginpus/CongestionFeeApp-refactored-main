using Contracts.Enums;
using Contracts.Models;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IChargeService
    {
        List<PeriodTotalCharge> CalculateCharges(DateTime start, DateTime end, VehicleType type);

        List<ChargePeriod> CalculateChargePeriods(DateTime start, DateTime end);

        List<TimeSplit> SplitTimeRangeIntoChargableDays(DateTime start, DateTime end);
        double CalculateTotalCharge(List<PeriodTotalCharge> periodTotalCharges);

        void GetDefaultChargeValues();
    }
}