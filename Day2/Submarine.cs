using System;

namespace Day2
{
    public struct Submarine
    {
        public int X { get; }
        public int Depth { get; }
        public int? Aim { get; }

        public static Submarine WithAim() => new Submarine(0, 0, 0);

        public Submarine ExecuteInstruction(Instruction instruction)
        {
            return Aim is int aim
                ? instruction.Direction switch
                {
                    Direction.Forward => new Submarine(X + instruction.Value, Depth + (instruction.Value * aim), aim),
                    Direction.Up => new Submarine(X, Depth, aim - instruction.Value),
                    Direction.Down => new Submarine(X, Depth, aim + instruction.Value),
                    _ => throw new ArgumentOutOfRangeException(nameof(instruction.Direction)),
                }
                : instruction.Direction switch
                {
                    Direction.Forward => new Submarine(X + instruction.Value, Depth),
                    Direction.Up => new Submarine(X, Depth - instruction.Value),
                    Direction.Down => new Submarine(X, Depth + instruction.Value),
                    _ => throw new ArgumentOutOfRangeException(nameof(instruction.Direction))
                };
        }

        Submarine(int x, int depth)
        {
            X = x;
            Depth = depth;
            Aim = null;
        }

        Submarine(int x, int depth, int aim)
        {
            X = x;
            Depth = depth;
            Aim = aim;
        }
    }
}
