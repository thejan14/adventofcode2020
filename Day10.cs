namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Day10
    {
        public static void Solve()
        {
            var joltageRatings = File.ReadAllLines("Day10.data")
                .Select(line => int.Parse(line))
                .OrderBy(rate => rate)
                .ToArray();

            var ratingDiffs = new List<int>();
            ratingDiffs.Add(joltageRatings.First()); // diff from charging outlet to first adapter
            ratingDiffs.Add(3); // diff from last adapter to device
            for (var i = 1; i < joltageRatings.Length; i++)
            {
                ratingDiffs.Add(joltageRatings[i] - joltageRatings[i - 1]);
            }


            var oneJoltDiffs = ratingDiffs.Count(r => r == 1);
            var threeJoltDiffs = ratingDiffs.Count(r => r == 3);
            Console.WriteLine($"{oneJoltDiffs} * {threeJoltDiffs} = {oneJoltDiffs * threeJoltDiffs}");
        }
    }
}
