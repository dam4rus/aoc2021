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
            var inputs =
                from line in AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "day1.input.txt")
                select int.Parse(line);

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
            var increaseCount = SumsOfTriplets(inputs)
                .Zip(SumsOfTriplets(inputs.Skip(1)))
                .Where(tripletSums => tripletSums.Second > tripletSums.First)
                .Count();

            Console.WriteLine("Increase count = {0}", increaseCount);
        }

        static IEnumerable<int> SumsOfTriplets(IEnumerable<int> input)
        {
            var slidingWindow = input.Zip(input.Skip(1))
                .Zip(input.Skip(2), (firstPair, third) => (first: firstPair.First, second: firstPair.Second, third));
            return from triplet in slidingWindow
                   select triplet.first + triplet.second + triplet.third;
        }
    }
}
