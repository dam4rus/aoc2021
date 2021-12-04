using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Day3
{
    public class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var inputs = AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day3.input.txt")
                .Select(line => line.Select(c => c == '1' ? 1 : 0).ToList())
                .ToList();

            Console.WriteLine($"power consumption: {Part1(inputs)}");
            Console.WriteLine($"life support rating: {Part2(inputs)}");
        }

        public static int Part1(List<List<int>> inputs)
        {
            var bitCounts = inputs
                .Cast<IEnumerable<int>>()
                .Aggregate((acc, bits) => acc.Zip(bits).Select(values => values.First + values.Second))
                .Reverse()
                .ToList();

            var mostCommonBits = bitCounts
                .Select(bitCount => bitCount >= inputs.Count - bitCount ? 1 : 0)
                .ToList();

            var leastCommonBits = mostCommonBits
                .Select(bitValue => 1 - bitValue)
                .ToList();

            var gamma = BitValuesToInt(mostCommonBits);
            var epsilon = BitValuesToInt(leastCommonBits);
            Console.WriteLine($"gamma: {gamma}, epsilon: {epsilon}");
            return gamma * epsilon;
        }

        public static int Part2(List<List<int>> inputs)
        {
            var oxigenGeneratorBits = GetRateBits(inputs, true);
            var co2ScrubberBits = GetRateBits(inputs, false);

            var oxigenGeneratorRate = BitValuesToInt(oxigenGeneratorBits.AsEnumerable().Reverse());
            var co2ScrubberRate = BitValuesToInt(co2ScrubberBits.AsEnumerable().Reverse());

            Console.WriteLine($"oxigen generator rate: {oxigenGeneratorRate}, CO2 scrubber rate: {co2ScrubberRate}");
            return oxigenGeneratorRate * co2ScrubberRate;
        }

        public static List<int> GetRateBits(List<List<int>> inputs, bool forOxigenGenerator)
        {
            var copyOfInput = new List<List<int>>(inputs);
            var valueOfMostCommonBit = forOxigenGenerator ? 1 : 0;
            for (int x = 0; x < copyOfInput.First().Count; ++x)
            {
                var onBitCount = OnBitsAt(copyOfInput, x);
                int valueToKeep = onBitCount >= copyOfInput.Count - onBitCount
                    ? valueOfMostCommonBit
                    : 1 - valueOfMostCommonBit;
                copyOfInput.RemoveAll(bits => bits.ElementAt(x) != valueToKeep);
                if (copyOfInput.Count == 1)
                {
                    return copyOfInput.First();
                }
            }

            throw new ArgumentException("Invalid arguments", nameof(inputs));
        }

        static int OnBitsAt(List<List<int>> bitsList, int index) => bitsList
                .Where(bits => bits.ElementAt(index) == 1)
                .Count();

        public static int BitValuesToInt(IEnumerable<int> bitValues) => bitValues
            .Select((value, index) => value << index)
            .Aggregate((acc, value) => acc | value);
    }
}
