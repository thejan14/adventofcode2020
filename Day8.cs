namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Day8
    {
        public static void Solve()
        {
            var programmInstructions = File.ReadAllLines("Day8.data");

            Console.WriteLine($"State of the accumulator before repeat: {GetAccumulatorBeforeLoop(programmInstructions)}");
        }

        private static int GetAccumulatorBeforeLoop(string[] programmInstructions)
        {
            var linesHit = new HashSet<int>();

            var accumulator = 0;
            var line = 0;
            while (line < programmInstructions.Length)
            {
                if (linesHit.Contains(line))
                {
                    return accumulator;
                }
                else
                {
                    linesHit.Add(line);
                }

                var instruction = programmInstructions[line];
                var operation = instruction.Substring(0, 3);
                var argument = instruction.Substring(4);
                switch (operation)
                {
                    case "acc":
                        accumulator += int.Parse(argument);
                        line += 1;
                        break;
                    case "jmp":
                        line += int.Parse(argument);
                        break;
                    default:
                        line += 1;
                        break;
                }
            }

            return accumulator;
        }
    }
}
