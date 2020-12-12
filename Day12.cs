namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Day12
    {
        private enum Direction
        {
            N = 0,
            E = 1,
            S = 2,
            W = 3,
        }

        public static void Solve()
        {
            var data = File.ReadAllLines("Day12.data");
            var instructions = data.Select(d => (d[0], int.Parse(d.Substring(1))));
            Console.WriteLine($"Manhattan distance: {CalculateDistance(instructions, Direction.E)}");
        }

        private static int CalculateDistance(IEnumerable<(char action, int value)> instructions, Direction startDirection)
        {
            var direction = startDirection;
            var horizontal = 0;
            var vertical = 0;

            foreach (var instruction in instructions)
            {
                switch (instruction.action)
                {
                    case 'N': vertical += instruction.value; break;
                    case 'E': horizontal += instruction.value; break;
                    case 'S': vertical -= instruction.value; break;
                    case 'W': horizontal -= instruction.value; break;
                    case 'L': direction = ChangeDirectionLeft(direction, instruction.value); break;
                    case 'R': direction = ChangeDirectionRight(direction, instruction.value); break;
                    case 'F':
                        switch (direction)
                        {
                            case Direction.N: vertical += instruction.value; break;
                            case Direction.E: horizontal += instruction.value; break;
                            case Direction.S: vertical -= instruction.value; break;
                            case Direction.W: horizontal -= instruction.value; break;
                        }
                        break;
                }
            }

            return Math.Abs(horizontal) + Math.Abs(vertical);
        }

        private static Direction ChangeDirectionLeft(Direction currentDirection, int degree)
        {
            var turns = degree / 90;
            return (Direction)((int)(currentDirection + (4 - turns)) % 4);
        }

        private static Direction ChangeDirectionRight(Direction currentDirection, int degree)
        {
            var turns = degree / 90;
            return (Direction)((int)(currentDirection + turns) % 4);
        }
    }
}
