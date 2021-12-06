using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Day5
{
    public class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var input = AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day5.input.txt");
            Console.WriteLine($"Overlapping horizonal and vertical points: {OverlappingPointsCount(input, line => line.Direction != LineDirection.Diagonal)}");
            Console.WriteLine($"Overlapping points count: {OverlappingPointsCount(input)}");
        }

        public static int OverlappingPointsCount(IEnumerable<string> input) => OverlappingPointsCount(input, line => true);

        public static int OverlappingPointsCount(IEnumerable<string> input, Func<Line, bool> predicate) => Input.FromLines(input)
            .Where(predicate)
            .SelectMany(line => line.Points())
            .GroupBy(position => position, (key, values) => (position: key, count: values.Count()))
            .Where(positionCount => positionCount.count >= 2)
            .Count();
    }

    public class Line
    {
        public int X1 { get; }
        public int Y1 { get; }
        public int X2 { get; }
        public int Y2 { get; }

        public int MinX { get => X1 < X2 ? X1 : X2; }
        public int MaxX { get => X1 > X2 ? X1 : X2; }
        public int MinY { get => Y1 < Y2 ? Y1 : Y2; }
        public int MaxY { get => Y1 > Y2 ? Y1 : Y2; }

        public static Line FromInputLine(string line)
        {
            var regex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)", RegexOptions.Compiled);
            var match = regex.Match(line);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid line input");
            }

            var groups = match.Groups;
            return new Line(int.Parse(groups[1].Value),
                int.Parse(groups[2].Value),
                int.Parse(groups[3].Value),
                int.Parse(groups[4].Value)
            );
        }

        public IEnumerable<(int x, int y)> Points()
        {
            switch (Direction)
            {
                case LineDirection.Horizontal:
                    for (int x = MinX; x <= MaxX; ++x)
                    {
                        yield return (x, Y1);
                    }
                    break;
                case LineDirection.Vertical:
                    for (int y = MinY; y <= MaxY; ++y)
                    {
                        yield return (X1, y);
                    }
                    break;
                case LineDirection.Diagonal:
                    var (startY, endY) = X1 < X2 ? (Y1, Y2) : (Y2, Y1);
                    var yIncrement = startY < endY ? 1 : -1;
                    for (int i = 0; i <= MaxX - MinX; ++i)
                    {
                        yield return (MinX + i, startY + (i * yIncrement));
                    }
                    break;
            }
        }

        public LineDirection Direction
        {
            get
            {
                if (X1 == X2)
                {
                    return LineDirection.Vertical;
                }
                if (Y1 == Y2)
                {
                    return LineDirection.Horizontal;
                }

                return LineDirection.Diagonal;
            }
        }

        Line(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }
    }

    public enum LineDirection
    {
        Horizontal,
        Vertical,
        Diagonal,
    }

    public static class Input
    {
        public static IEnumerable<Line> FromLines(IEnumerable<string> lines) => lines.Select(Line.FromInputLine);

    }
}
