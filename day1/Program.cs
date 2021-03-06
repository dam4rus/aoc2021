using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var inputs = AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "day1.input.txt")
                .Select(int.Parse);

            Part1(inputs);
            Part2(inputs);
        }

        static void Part1(IEnumerable<int> inputs)
        {
            var increaseCount = inputs.Zip(inputs.Skip(1))
                .Where(pair => pair.Second > pair.First)
                .Count();

            Console.WriteLine("Increase count = {0}", increaseCount);
        }

        static void Part2(IEnumerable<int> inputs)
        {
            var sumOfTriplets = SumsOfTriplets(inputs);
            var increaseCount = sumOfTriplets
                .Zip(sumOfTriplets.Skip(1))
                .Where(tripletSums => tripletSums.Second > tripletSums.First)
                .Count();

            Console.WriteLine("Increase count = {0}", increaseCount);
        }

        static List<int> SumsOfTriplets(IEnumerable<int> input)
        {
            return input.Zip(input.Skip(1))
                .Zip(input.Skip(2), (firstPair, third) => (first: firstPair.First, second: firstPair.Second, third))
                .Select(triplet => triplet.first + triplet.second + triplet.third)
                .ToList();
        }
    }
}
