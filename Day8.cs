namespace Solution
{
    using System;
    using System.IO;
    using System.Linq;

    public enum Operation
    {
        nop,
        jmp,
        acc
    }

    public static class Day8
    {
        public static void Solve()
        {
            var programmInstructions = ParseProgramm(File.ReadAllLines("Day8.data"));
            TryGetAccumulator(programmInstructions, out var accumulator);
            Console.WriteLine($"(1) State of the accumulator before repeat: {accumulator}");
            Console.WriteLine($"(2) State of the accumulator after fixed programm: {GetFixedAccumulator(programmInstructions)}");
        }

        private static (Operation operation, int argument, int nextLine)[] ParseProgramm(string[] programmLines)
        {
            var programmInstructions = new (Operation operation, int argument, int nextLine)[programmLines.Length];
            for (var line = 0; line < programmLines.Length; line++)
            {
                var instruction = programmLines[line];
                var operation = Enum.Parse<Operation>(instruction.Substring(0, 3));
                var argument = int.Parse(instruction.Substring(4));
                var nextLine = operation switch
                {
                    Operation.jmp => line + argument,
                    _ => line + 1
                };

                programmInstructions[line] = (operation, argument, nextLine);
            }

            return programmInstructions;
        }

        private static bool TryGetAccumulator((Operation operation, int argument, int nextLine)[] programmInstructions, out int accumulator)
        {
            var linesHit = new bool[programmInstructions.Length];

            accumulator = 0;
            var line = 0;
            while (line < programmInstructions.Length)
            {
                if (linesHit[line])
                {
                    return false;
                }
                else
                {
                    linesHit[line] = true;
                }

                var instruction = programmInstructions[line];
                if (instruction.operation == Operation.acc)
                {
                    accumulator += instruction.argument;
                }

                line = instruction.nextLine;
            }

            return true;
        }

        private static int GetFixedAccumulator((Operation operation, int argument, int nextLine)[] programmInstructions)
        {
            var linesToFlip = Enumerable.Range(0, programmInstructions.Length - 1)
                .Where(i => programmInstructions[i].operation == Operation.jmp || programmInstructions[i].operation == Operation.nop);

            foreach (var line in linesToFlip)
            {
                ExchangeInstruction(programmInstructions, line);
                if (TryGetAccumulator(programmInstructions, out var accumulator))
                {
                    return accumulator;
                }

                // change back instruction
                ExchangeInstruction(programmInstructions, line);
            }

            return 0;
        }

        private static void ExchangeInstruction((Operation operation, int argument, int nextLine)[] programmInstructions, int line)
        {
            switch (programmInstructions[line].operation)
            {
                case Operation.jmp:
                    programmInstructions[line].operation = Operation.nop;
                    programmInstructions[line].nextLine = line + 1;
                    break;
                case Operation.nop:
                    programmInstructions[line].operation = Operation.jmp;
                    programmInstructions[line].nextLine = line + programmInstructions[line].argument;
                    break;
                default:
                    break;
            }
        }
    }
}
