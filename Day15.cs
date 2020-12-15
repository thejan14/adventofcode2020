﻿namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Day15
    {
        public static void Solve()
        {
            var startingNumbers = File.ReadAllText("Day15.data").Split(',').Select(s => int.Parse(s));
            var result = CalculateMemoryGame(startingNumbers.ToArray(), 2020);
            Console.WriteLine($"2020th number spoken: {result}");
        }

        private static int CalculateMemoryGame(int[] startingNumbers, int targetNumber)
        {
            var numberTurns = new Dictionary<int, int>();
            for (var i = 0; i < startingNumbers.Length; i++)
            {
                numberTurns[startingNumbers[i]] = i + 1;
            }

            var previousNumber = 0;
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
