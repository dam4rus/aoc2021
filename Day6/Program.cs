using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var input = AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day6.input.txt").First();
            var lanternfishLifetimes = Input.Get(input);
            Console.WriteLine($"Lanternfishes after 80 days: {Part1(lanternfishLifetimes)}");
            Console.WriteLine($"Lanternfished after 256 days: {Part2(lanternfishLifetimes)}");
        }

        static long Part1(Dictionary<long, long> lanternfishLifetimes) => LanternfishLifecycle.CountAfterDaysPasses(lanternfishLifetimes, 80);
        static long Part2(Dictionary<long, long> lanternfishLifetimes) => LanternfishLifecycle.CountAfterDaysPasses(lanternfishLifetimes, 256);
    }

    public static class LanternfishLifecycle
    {
        public static long CountAfterDaysPasses(Dictionary<long, long> lanternfishLifetimes, int days) => Enumerable.Range(0, days)
            .Aggregate(lanternfishLifetimes, (lifetimes, _) => AfterDayPasses(lifetimes))
            .Select(pair => pair.Value)
            .Sum();

        static Dictionary<long, long> AfterDayPasses(Dictionary<long, long> lanterfishLifetimes) => lanterfishLifetimes
            .Select(pair => KeyValuePair.Create(pair.Key - 1, pair.Value))
            .GroupBy(pair => pair.Key == -1 ? 6 : pair.Key, (key, values) => KeyValuePair.Create(key, values.Sum(pair => pair.Value)))
            .Append(KeyValuePair.Create(8L, lanterfishLifetimes.FirstOrDefault(pair => pair.Key == 0).Value))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public static class Input
    {
        public static Dictionary<long, long> Get(string input) => input.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .GroupBy(lifetime => lifetime, (lifetime, values) => (Lifetime: lifetime, Count: values.LongCount()))
            .ToDictionary(pair => pair.Lifetime, pair => pair.Count);
    }
}
