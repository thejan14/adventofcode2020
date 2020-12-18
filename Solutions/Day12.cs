namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day12 : Solution
    {
        private enum Direction
        {
            N = 0,
            E = 1,
            S = 2,
            W = 3,
        }

        public override void Solve(string dataPath)
        {
            var data = File.ReadAllLines(dataPath);
            var instructions = data.Select(d => (d[0], int.Parse(d.Substring(1))));
            Console.WriteLine($"(1) Manhattan distance (guessed instruction interpretation): {CalculateDistance(instructions, Direction.E)}");
            Console.WriteLine($"(2) Manhattan distance (after reading the manual): {CalculateActualDistance(instructions)}");
        }

        private static int CalculateActualDistance(IEnumerable<(char action, int value)> instructions)
        {
            var horizontal = 0;
            var vertical = 0;

            // relative to the ship
            var waypointX = 10;
            var waypointY = 1;

            foreach (var instruction in instructions)
            {
                switch (instruction.action)
                {
                    case 'N': waypointY += instruction.value; break;
                    case 'E': waypointX += instruction.value; break;
                    case 'S': waypointY -= instruction.value; break;
                    case 'W': waypointX -= instruction.value; break;
                    case 'L': RotateWaypoint(ref waypointX, ref waypointY, instruction.value, false); break;
                    case 'R': RotateWaypoint(ref waypointX, ref waypointY, instruction.value, true); break;
                    case 'F':
                        vertical += waypointX * instruction.value;
                        horizontal += waypointY * instruction.value;
                        break;
                }
            }

            return Math.Abs(horizontal) + Math.Abs(vertical);
        }

        private static void RotateWaypoint(ref int x, ref int y, int degree, bool clockWise)
        {
            int turns = (degree / 90) % 360;
            for (var i = 0; i < turns; i++)
            {
                var tmp = x;
                x = clockWise ? y : y * -1;
                y = clockWise ? tmp * -1 : tmp;
            }
        }

        private static int CalculateDistance(IEnumerable<(char action, int value)> instructions, Direction startDirection)
        {
            var direction = startDirection;
            var coordinates = new Dictionary<Direction, int>()
            {
                { Direction.N, 0 },
                { Direction.E, 0 },
                { Direction.S, 0 },
                { Direction.W, 0 },
            };

            foreach (var instruction in instructions)
            {
                switch (instruction.action)
                {
                    case 'N': coordinates[Direction.N] += instruction.value; break;
                    case 'E': coordinates[Direction.E] += instruction.value; break;
                    case 'S': coordinates[Direction.S] += instruction.value; break;
                    case 'W': coordinates[Direction.W] += instruction.value; break;
                    case 'L': direction = ChangeDirectionLeft(direction, instruction.value); break;
                    case 'R': direction = ChangeDirectionRight(direction, instruction.value); break;
                    case 'F': coordinates[direction] += instruction.value; break;
                }
            }

            return Math.Abs(coordinates[Direction.N] - coordinates[Direction.S]) + Math.Abs(coordinates[Direction.E] - coordinates[Direction.W]);
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
