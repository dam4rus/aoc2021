using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Day8
{

    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var inputLines = Input.Get(AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day8.input.txt"));
            Console.WriteLine($"Count of 1, 4, 7, 8s: {inputLines.Select(inputLine => inputLine.DigitAppearCount()).Sum()}");
            Console.WriteLine($"Sum of digits: {inputLines.Select(inputLine => inputLine.DecodeDigits()).Sum()}");
        }
    }

    public class InputLine
    {
        public List<string> SignalPatterns { get; }
        public List<string> DigitValues { get; }

        public int DigitAppearCount()
        {
            var decodedSignals = DecodeSignalsByLength();
            var decodedDigits = DecodeDigitsByLength(decodedSignals);
            return decodedDigits.Count();
        }

        public int DecodeDigits()
        {
            var wiring = SolveWiring();
            var digits = DigitsFromSegments(wiring);
            return DigitValues
                .Select((digit, i) => digits[digit] * (int)Math.Pow(10, 3 - i))
                .Sum();
        }

        internal static InputLine FromLine(string line)
        {
            var patternsDigits = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
            if (patternsDigits.Length != 2)
            {
                throw new ArgumentException($"Invalid input: {line}");
            }

            var patterns = patternsDigits[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SortString).ToList();
            var digits = patternsDigits[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SortString).ToList();
            return new InputLine(patterns, digits);
        }

        InputLine(List<string> signalPatterns, List<string> digitValues)
        {
            SignalPatterns = signalPatterns;
            DigitValues = digitValues;
        }

        Dictionary<string, int> DigitsFromSegments(Dictionary<Segment, char> segments) => Enumerable.Range(0, 10)
            .Select(i => KeyValuePair.Create(SegmentsToDigit(segments, DigitSegments[i]), i))
            .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);

        readonly Dictionary<int, HashSet<Segment>> DigitSegments = new Dictionary<int, HashSet<Segment>>()
        {
            { 0, new HashSet<Segment>() { Segment.TOP, Segment.TOP_LEFT, Segment.TOP_RIGHT, Segment.BOTTOM_LEFT, Segment.BOTTOM_RIGHT, Segment.BOTTOM } },
            { 1, new HashSet<Segment>() { Segment.TOP_RIGHT, Segment.BOTTOM_RIGHT } },
            { 2, new HashSet<Segment>() { Segment.TOP, Segment.TOP_RIGHT, Segment.CENTER, Segment.BOTTOM_LEFT, Segment.BOTTOM } },
            { 3, new HashSet<Segment>() { Segment.TOP, Segment.TOP_RIGHT, Segment.CENTER, Segment.BOTTOM_RIGHT, Segment.BOTTOM } },
            { 4, new HashSet<Segment>() { Segment.TOP_LEFT, Segment.CENTER, Segment.TOP_RIGHT, Segment.BOTTOM_RIGHT } },
            { 5, new HashSet<Segment>() { Segment.TOP, Segment.TOP_LEFT, Segment.CENTER, Segment.BOTTOM_RIGHT, Segment.BOTTOM } },
            { 6, new HashSet<Segment>() {Segment.TOP, Segment.TOP_LEFT, Segment.CENTER, Segment.BOTTOM_LEFT, Segment.BOTTOM_RIGHT, Segment.BOTTOM } },
            { 7, new HashSet<Segment>() { Segment.TOP, Segment.TOP_RIGHT, Segment.BOTTOM_RIGHT } },
            { 8, new HashSet<Segment>() { Segment.TOP, Segment.TOP_LEFT, Segment.TOP_RIGHT, Segment.CENTER, Segment.BOTTOM_LEFT, Segment.BOTTOM_RIGHT, Segment.BOTTOM } },
            { 9, new HashSet<Segment>() { Segment.TOP, Segment.TOP_LEFT, Segment.TOP_RIGHT, Segment.CENTER, Segment.BOTTOM_RIGHT, Segment.BOTTOM } },
        };

        string SegmentsToDigit(Dictionary<Segment, char> segments, HashSet<Segment> segmentsToConvert) => new string(segmentsToConvert
            .Select(segment => segments[segment])
            .OrderBy(c => c)
            .ToArray());

        Dictionary<Segment, char> SolveWiring()
        {
            var decodedSignals = DecodeSignalsByLength();
            var dict = new Dictionary<Segment, char>();
            // we can deduce the top segment by getting the difference of 7 and 1 segments
            dict.Add(Segment.TOP, decodedSignals[7].Except(decodedSignals[1]).Single());

            // we can get bottom segment by going through the digits with 6 segments and getting the difference from {x} - {1 | 4 | 7}
            // only 9 will have a single segment left
            var unionOfOneFourSeven = new string(decodedSignals[1].Union(decodedSignals[4]).Union(decodedSignals[7]).ToArray());

            var bottomChar =
                (from difference in PatternsWithSegmentCountExcept(6, unionOfOneFourSeven)
                 where difference.Length == 1
                 select difference.First())
                .Single();

            dict.Add(Segment.BOTTOM, bottomChar);
            decodedSignals.Add(9, new string(unionOfOneFourSeven.Append(bottomChar).OrderBy(c => c).ToArray()));

            // we can get the bottom left segment easily by {8} - {9}
            var bottomLeftChar = decodedSignals[8].Except(decodedSignals[9]).Single();
            dict.Add(Segment.BOTTOM_LEFT, bottomLeftChar);

            // i give up
            var filter = new string(decodedSignals[7].Append(dict[Segment.BOTTOM]).Append(dict[Segment.BOTTOM_LEFT]).OrderBy(c => c).ToArray());
            var centerChar =
                (from difference in PatternsWithSegmentCountExcept(5, filter)
                 where difference.Length == 1
                 select difference.First())
                .First();
            dict.Add(Segment.CENTER, centerChar);
            var topLeftChar = decodedSignals[8].Except(decodedSignals[7].Append(bottomChar).Append(bottomLeftChar).Append(centerChar)).Single();
            dict.Add(Segment.TOP_LEFT, topLeftChar);

            var filter2 = new List<char>()
            {
                dict[Segment.TOP],
                dict[Segment.TOP_LEFT],
                dict[Segment.CENTER],
                dict[Segment.BOTTOM_LEFT],
                dict[Segment.BOTTOM],
            };
            var bottomRightChar =
                (from difference in PatternsWithSegmentCountExcept(6, filter2)
                 where difference.Length == 1
                 select difference.First())
                .Single();
            dict.Add(Segment.BOTTOM_RIGHT, bottomRightChar);
            var topRightChar = decodedSignals[8].Except(dict.Values).Single();
            dict.Add(Segment.TOP_RIGHT, topRightChar);
            return dict;
        }

        Dictionary<int, string> DecodeSignalsByLength() => SignalPatterns.Aggregate(new Dictionary<int, string>(), (dict, pattern) =>
        {
            switch (pattern.Length)
            {
                case 2:
                    dict.Add(1, pattern);
                    break;
                case 3:
                    dict.Add(7, pattern);
                    break;
                case 4:
                    dict.Add(4, pattern);
                    break;
                case 7:
                    dict.Add(8, pattern);
                    break;
            }

            return dict;
        });

        IEnumerable<int> DecodeDigitsByLength(Dictionary<int, string> decodedSignals) =>
            from digit in DigitValues
            from keyValuePair in decodedSignals
            where keyValuePair.Value.Length == digit.Length
            select keyValuePair.Key;

        IEnumerable<string> PatternsWithSegmentCountExcept(int segmentCount, IEnumerable<char> exception) =>
            from pattern in SignalPatterns
            where pattern.Length == segmentCount
            select new string(pattern.Except(exception).ToArray());

        static string SortString(string s) => new string(s.OrderBy(c => c).ToArray());
    }

    public static class Input
    {
        public static List<InputLine> Get(IEnumerable<string> input) => input.Select(InputLine.FromLine).ToList();
    }

    enum Segment
    {
        TOP,
        TOP_LEFT,
        TOP_RIGHT,
        CENTER,
        BOTTOM_LEFT,
        BOTTOM_RIGHT,
        BOTTOM,
    }
}
