using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Day3;

namespace Day3Tests
{
    [TestClass]
    public class DayTests
    {
        static readonly List<string> testInput = new List<string>
        {
            "00100",
            "11110",
            "10110",
            "10111",
            "10101",
            "01111",
            "00111",
            "11100",
            "10000",
            "11001",
            "00010",
            "01010",
        };

        [TestMethod]
        public void Part1Test()
        {
            var inputs = testInput
                .Select(line => line.Select(c => c == '1' ? 1 : 0).ToList())
                .ToList();

            Assert.AreEqual(198, Program.Part1(inputs));
        }

        [TestMethod]
        public void Part2Test()
        {
            var inputs = testInput
                .Select(line => line.Select(c => c == '1' ? 1 : 0).ToList())
                .ToList();

            var oxigenRateBits = Program.GetRateBits(inputs, true);
            var co2RateBits = Program.GetRateBits(inputs, false);
            CollectionAssert.AreEqual(new List<int> { 1, 0, 1, 1, 1 }, oxigenRateBits);
            CollectionAssert.AreEqual(new List<int> { 0, 1, 0, 1, 0 }, co2RateBits);

            var oxigenGeneratorRate = Program.BitValuesToInt(oxigenRateBits.AsEnumerable().Reverse());
            var co2ScrubberRate = Program.BitValuesToInt(co2RateBits.AsEnumerable().Reverse());
            Assert.AreEqual(230, oxigenGeneratorRate * co2ScrubberRate);
        }
    }
}
