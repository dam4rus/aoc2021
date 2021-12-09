using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Day7;

namespace Day7Tests
{
    [TestClass]
    public class Tests
    {
        static readonly string input = "16,1,2,0,4,2,7,1,2,14";

        [TestMethod]
        public void Part1()
        {
            var positions = Input.Get(input);
            Assert.AreEqual(37, CrabSubmarines.FindLeastFuelConsumptionContantRate(positions));
        }

        [TestMethod]
        public void Part2()
        {
            var positions = Input.Get(input);
            Assert.AreEqual(168, CrabSubmarines.FindLeastFuelConsumptionVariableRate(positions));
        }
    }
}
