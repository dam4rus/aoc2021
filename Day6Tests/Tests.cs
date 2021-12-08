using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day6;
using System.Linq;

namespace Day6Tests
{
    [TestClass]
    public class Tests
    {
        static readonly string input = "3,4,3,1,2";

        [TestMethod]
        public void Part1()
        {
            Assert.AreEqual(5934, LanternfishLifecycle.CountAfterDaysPasses(Input.Get(input), 80));
        }

        [TestMethod]
        public void Part2()
        {
            Assert.AreEqual(26984457539, LanternfishLifecycle.CountAfterDaysPasses(Input.Get(input), 256));
        }
    }
}
