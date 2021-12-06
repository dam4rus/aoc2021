using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var input = AocUtils.AocUtils.ReadLinesFromAssemblyResource(assembly, "Day4.input.txt");
            Console.WriteLine($"final score is: {Part1(input)}");
            Console.WriteLine($"last winner board final score is: {Part2(input)}");
        }

        static int Part1(IEnumerable<string> inputLines) => Input.FromLines(inputLines)
            .EnumerateWinnerScores()
            .First();

        static int Part2(IEnumerable<string> inputLines) => Input.FromLines(inputLines)
            .EnumerateWinnerScores()
            .Last();
    }

    public class BingoNumber
    {
        public int Number { get; }
        public bool IsDrawn { get; set; }

        public BingoNumber(int number)
        {
            Number = number;
            IsDrawn = false;
        }
    }

    public class Board
    {
        public static Board FromInput(IEnumerable<string> board) => new Board(board
                .Select(row => row.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(number => new BingoNumber(int.Parse(number)))
                    .ToList())
                .ToList());

        public void MarkNumber(int number)
        {
            var bingoNumber = numbers
                .SelectMany(row => row)
                .FirstOrDefault(bingomNumber => bingomNumber.Number == number);

            if (bingoNumber != null)
            {
                bingoNumber.IsDrawn = true;
            }
        }

        public bool IsWinner()
        {
            if (numbers.Any(row => row.All(number => number.IsDrawn)))
            {
                return true;
            }

            return Enumerable.Range(0, numbers.First().Count())
                .Any(x => numbers.Select(row => row.ElementAt(x)).All(number => number.IsDrawn));
        }

        public int SumOfUnmarkedNumbers() => numbers.SelectMany(row => row)
                .Where(number => !number.IsDrawn)
                .Select(number => number.Number)
                .Sum();

        List<List<BingoNumber>> numbers;

        Board(List<List<BingoNumber>> numbers)
        {
            this.numbers = numbers;
        }
    }

    public class Input
    {
        public static Input FromLines(IEnumerable<string> input) => new Input
        {
            numbersToDraw = input.First().Split(',').Select(int.Parse).ToList(),
            boards = GetBoardsFromInput(input),
        };

        public IEnumerable<int> EnumerateWinnerScores()
        {
            var winningBoards = new List<Board>();
            foreach (var number in numbersToDraw)
            {
                foreach (var board in boards)
                {
                    board.MarkNumber(number);
                    if (board.IsWinner() && !winningBoards.Contains(board))
                    {
                        yield return number * board.SumOfUnmarkedNumbers();
                        winningBoards.Add(board);
                    }
                }
            }
        }

        static List<Board> GetBoardsFromInput(IEnumerable<string> input) => EnumerateBoards(input.Skip(2))
                .Select(Board.FromInput)
                .ToList();

        static IEnumerable<IEnumerable<string>> EnumerateBoards(IEnumerable<string> input)
        {
            var boardParameters = new List<string>();
            foreach (var line in input)
            {
                if (line.Length == 0)
                {
                    yield return boardParameters;
                    boardParameters.Clear();
                    continue;
                }

                boardParameters.Add(line);
            }

            yield return boardParameters;
        }

        List<int> numbersToDraw;
        List<Board> boards;
    }
}
