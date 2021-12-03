using Contracts.Enums;
using Contracts.Models;
using System;
using System.Collections.Generic;

namespace CongestionFeeApp.Models.Methods
{
    public class ConsoleLogger
    {
        public void Log(object message)
        {
            Console.WriteLine(message);
        }

        public void LogExpectedTimeFormat(TimeGate timeGate)
        {
            Console.WriteLine($"Enter {timeGate.ToString().ToLower()} time in `dd/MM/yyyy HH:mm` format (no ticks): ");
        }

        public void PrintAllSelections()
        {
            Log("Available commands:");
            foreach (var name in Enum.GetValues(typeof(Selection)))
            {
                Console.WriteLine($"{(int)name}: {name}");
            }
        }

        public void PrintVechicleTypes()
        {
            Log("Available vechicle types:");
            foreach (var name in Enum.GetValues(typeof(VehicleType)))
            {
                Console.WriteLine($"{(int)name}: {name}");
            }
        }

        public void PrintIndividualCharges(List<PeriodTotalCharge> charges)
        {
            foreach (var entry in charges)
            {
                Log(entry);
            }
        }

        public void PrintTotalCharge(double totalCharge)
        {
            Log($"Total charge: £{ totalCharge.ToString("0.00")}");
        }
    }
}
