using Contracts.Enums;
using Domain.Services;
using System;

namespace CongestionFeeApp.Models.Methods
{
    public class DemoHelper
    {
        private readonly IChargeService _chargeService;
        private ConsoleLogger _logger;

        public DemoHelper(IChargeService chargeService, ConsoleLogger logger)
        {
            _chargeService = chargeService;
            _logger = logger;
        }

        public void DisplayDemoResults(DateTime startTime, DateTime endTime, VehicleType vehicleType)
        {
            var charges = _chargeService.CalculateCharges(startTime, endTime, vehicleType);
            foreach (var entry in charges)
            {
                _logger.Log(entry);
            }
            var totalCharge = _chargeService.CalculateTotalCharge(charges);
            _logger.Log($"Total charge: £{ totalCharge.ToString("0.00")}");
        }

        public void PrintAllDemos()
        {
            _logger.Log("\nAvailable demos:");
            _logger.Log("1: Car: 24/04/2008 11:32 - 24/04/2008 14:42");
            _logger.Log("2: Motorbike: 24/04/2008 17:00 - 24/04/2008 22:11"); ;
            _logger.Log("3: Van: 25/04/2008 10:23 - 28/04/2008 09:02");
        }
    }
}
