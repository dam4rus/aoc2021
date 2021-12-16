using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Linq;
using Day8;

namespace Day8Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Part1()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var inputLines = Input.Get(AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day8Tests.input.txt"));
            Assert.AreEqual(26, inputLines.Select(inputLine => inputLine.DigitAppearCount()).Sum());
        }

        [TestMethod]
        public void Part2()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var inputLines = Input.Get(AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day8Tests.input.txt"));
            Assert.AreEqual(61229, inputLines.Select(inputLine => inputLine.DecodeDigits()).Sum());
        }
    }
}
