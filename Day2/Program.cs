using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var inputs = AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day2.input.txt");
            Part1(inputs);
            Part2(inputs);
        }

        static void Part1(IEnumerable<string> inputs)
        {
            var submarine = inputs.Aggregate(new Submarine(),
                (submarine, line) => submarine.ExecuteInstruction(Instruction.FromString(line)));

            Console.WriteLine($"Result of part 1: {submarine.X * submarine.Depth}");
        }

        static void Part2(IEnumerable<string> inputs)
        {
            var submarine = inputs.Aggregate(Submarine.WithAim(),
                (submarine, line) => submarine.ExecuteInstruction(Instruction.FromString(line)));

            Console.WriteLine($"Result of part 2: {submarine.X * submarine.Depth}");
        }
    }
}
