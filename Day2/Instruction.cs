using System;

namespace Day2
{
    public struct Instruction
    {
        public Direction Direction { get; }
        public int Value { get; }

        public static Instruction FromString(string instruction)
        {
            var parameters = instruction.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var direction = parameters[0];
            var offset = parameters[1];
            return new Instruction(
                direction switch
                {
                    "forward" => Direction.Forward,
                    "up" => Direction.Up,
                    "down" => Direction.Down,
                    _ => throw new ArgumentException($"invalid direction: {direction}"),
                },
                int.Parse(offset)
            );
        }

        Instruction(Direction direction, int offset)
        {
            Direction = direction;
            Value = offset;
        }
    }
}
