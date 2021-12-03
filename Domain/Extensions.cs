using Contracts.Models;
using Persistence.Models;

namespace Domain
{
    public static class Extensions
    {
        public static ChargePeriod AsDto(this ChargePeriodMain model)
        {
            return new ChargePeriod
            {
                Id = model.Id,
                Alias = model.Alias,
                Start = model.Start,
                End = model.End,
                FeeList = model.FeeList,
                TotalDuration = model.TotalDuration
            };
        }
    }
}