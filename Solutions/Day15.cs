namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day15 : Solution
    {
        public override void Solve(string dataPath)
        {
            var startingNumbers = File.ReadAllText(dataPath).Split(',').Select(s => int.Parse(s));
            var result = CalculateMemoryGame(startingNumbers.ToArray(), 2020);
            Console.WriteLine($"(1) 2020th number spoken: {result}");

            result = CalculateMemoryGame(startingNumbers.ToArray(), 30000000);
            Console.WriteLine($"(2) 30000000th number spoken: {result}");
        }

        private static int CalculateMemoryGame(int[] startingNumbers, int targetNumber)
        {
            var numberTurns = new Dictionary<int, int>();
            for (var i = 0; i < startingNumbers.Length; i++)
            {
                numberTurns[startingNumbers[i]] = i + 1;
            }

            int previousNumber;
            var lastNumber = startingNumbers.Last();
            for (var turn = startingNumbers.Length + 1; turn < targetNumber + 1; turn++)
            {
                previousNumber = lastNumber;
                if (numberTurns.TryGetValue(lastNumber, out var mostRecentTurn))
                {
                    lastNumber = (turn - 1) - mostRecentTurn;
                }
                else
                {
                    lastNumber = 0;
                }

                numberTurns[previousNumber] = turn - 1;
            }

            return lastNumber;
        }
    }
}
