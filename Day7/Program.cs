using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var input = Input.Get(AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day7.input.txt").First());
            Console.WriteLine($"Least fuel consumption with constant rate: {CrabSubmarines.FindLeastFuelConsumptionContantRate(input)}");
            Console.WriteLine($"Least fuel consumption with variable rate: {CrabSubmarines.FindLeastFuelConsumptionVariableRate(input)}");
        }
    }

    public static class CrabSubmarines
    {
        public static int FindLeastFuelConsumptionContantRate(SortedDictionary<int, int> input) => RangeOfInput(input)
            .Select(i => FuelConsumptionConstantRate(i, input))
            .Min();

        public static int FindLeastFuelConsumptionVariableRate(SortedDictionary<int, int> input) => RangeOfInput(input)
            .Select(i => FuelConsumptionVariableRate(i, input))
            .Min();

        static IEnumerable<int> RangeOfInput(SortedDictionary<int, int> input) =>
            Enumerable.Range(input.Min(pair => pair.Key), input.Max(pair => pair.Key));

        static int Distance(int i, int j) => Math.Max(i, j) - Math.Min(i, j);

        static int FuelConsumptionConstantRate(int i, SortedDictionary<int, int> input) =>
            input.Select(pair => Distance(i, pair.Key) * pair.Value).Sum();

        static int FuelConsumptionVariableRate(int i, SortedDictionary<int, int> input) =>
            input.Select(pair =>
            {
                var distance = Distance(i, pair.Key);
                var fuelConsumption = distance * (distance + 1) / 2;
                return fuelConsumption * pair.Value;
            })
            .Sum();
    }

    public static class Input
    {
        public static SortedDictionary<int, int> Get(string input) => new SortedDictionary<int, int>(input.Split(',')
            .Select(int.Parse)
            .GroupBy(num => num, (num, values) => KeyValuePair.Create(num, values.Count()))
            .ToDictionary(pair => pair.Key, pair => pair.Value));
    }
}
