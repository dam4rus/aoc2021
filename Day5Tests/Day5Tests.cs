using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq;
using Day5;

namespace Day5Tests
{
    [TestClass]
    public class Day5Tests
    {
        [TestMethod]
        public void Part1Test()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var input = AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day5Tests.input.txt");
            Assert.AreEqual(5, Program.OverlappingPointsCount(input, line => line.Direction != LineDirection.Diagonal));
        }

        [TestMethod]
        public void Part2Test()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var input = AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day5Tests.input.txt");
            Assert.AreEqual(12, Program.OverlappingPointsCount(input));
        }
    }
}
