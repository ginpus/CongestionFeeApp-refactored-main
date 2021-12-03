using CongestionFeeApp.Models.Methods;
using Contracts.Enums;
using Contracts.Models;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CongestionFeeApp
{
    public class CongestionFeeApp
    {
        public ConsoleLogger Logger { get; set; } = new ConsoleLogger();
        public ConsoleReader Reader { get; set; } = new ConsoleReader();
        public InputValidator Validator { get; set; } = new InputValidator();



        private readonly IChargeService _chargeService;

        public CongestionFeeApp(IChargeService chargeService)
        {
            _chargeService = chargeService;
        }

        public Task Start()
        {
            while (true)
            {
                List<PeriodTotalCharge> charges;
                double totalCharge;
                Logger.PrintAllSelections();
                var chosenCommand = Reader.ParseCommandSelection();
                switch (chosenCommand)
                {
                    case Selection.Calculate_Congestion_Fee:
                        Logger.PrintVechicleTypes();
                        var vehicleType = Reader.ParseVehicleType();
                        Logger.LogExpectedTimeFormat(TimeGate.Start);
                        var startTime = Reader.ParseTime();
                        DateTime endTime;
                        do
                        {
                            Logger.LogExpectedTimeFormat(TimeGate.End);
                            endTime = Reader.ParseTime();
                        }
                        while (!Validator.IsEndTimeValid(startTime, endTime));
                        charges = _chargeService.CalculateCharges(startTime, endTime, vehicleType);
                        Logger.PrintIndividualCharges(charges);
                        totalCharge = _chargeService.CalculateTotalCharge(charges);
                        Logger.PrintTotalCharge(totalCharge);
                        Logger.Log("-----------------------------");
                        break;

                    case Selection.Demo_Congestion_Fee_Calculation:
                        bool demoMode = true;
                        while (demoMode)
                        {
                            var demo = new DemoHelper(_chargeService, Logger);
                            demo.PrintAllDemos();
                            Logger.Log("9: Exit Demo");
                            var chosenDemo = Console.ReadLine();
                            switch (chosenDemo)
                            {
                                case "1":
                                    demo.DisplayDemoResults(
                                        new DateTime(2008, 4, 24, 11, 32, 0),
                                        new DateTime(2008, 4, 24, 14, 42, 0),
                                        VehicleType.Car);
                                    break;

                                case "2":
                                    demo.DisplayDemoResults(
                                        new DateTime(2008, 4, 24, 17, 0, 0),
                                        new DateTime(2008, 4, 24, 22, 11, 0),
                                        VehicleType.Motorbike);
                                    break;

                                case "3":
                                    demo.DisplayDemoResults(
                                        new DateTime(2008, 4, 25, 10, 23, 0),
                                        new DateTime(2008, 4, 28, 9, 2, 0),
                                        VehicleType.Car);
                                    break;

                                case "9":
                                    demoMode = false;
                                    break;
                                default:
                                    Logger.Log("No such selection. Try again");
                                    break;
                            }
                        }
                        Logger.Log("--------------Exiting demo mode---------------");
                        break;

                    case Selection.Exit:
                        Logger.Log("The program ended");
                        return Task.CompletedTask;

                    default:
                        Logger.Log("No such selection. Try again");
                        break;
                }
            }
        }
    }
}