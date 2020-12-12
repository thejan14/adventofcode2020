namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    public static class Day10
    {
        public static void Solve()
        {
            var joltageRatings = File.ReadAllLines("Day10.data")
                .Select(line => int.Parse(line))
                .OrderBy(rate => rate);

            var joltageRatingsMultiple = GetMultipleOfJoltDifferences(joltageRatings.ToArray());
            Console.WriteLine($"(1) Multiple of one jolt differences and three jolt differences: {joltageRatingsMultiple}");
            Console.WriteLine($"(2) Number of distinct adapter arrangements: {GetNumberOfDistinctArrangements(joltageRatings.ToList())}");
        }

        private static int GetMultipleOfJoltDifferences(int[] joltageRatings)
        {
            var ratingDiffs = new List<int>();
            ratingDiffs.Add(joltageRatings.First()); // diff from charging outlet to first adapter
            ratingDiffs.Add(3); // diff from last adapter to device
            for (var i = 1; i < joltageRatings.Length; i++)
            {
                ratingDiffs.Add(joltageRatings[i] - joltageRatings[i - 1]);
            }


            var oneJoltDiffs = ratingDiffs.Count(r => r == 1);
            var threeJoltDiffs = ratingDiffs.Count(r => r == 3);
            return oneJoltDiffs * threeJoltDiffs;
        }

        private static ulong GetNumberOfDistinctArrangements(List<int> joltageRatings)
        {
            // add 0 as start
            joltageRatings.Insert(0, 0);

            // graph stores possible adapter options for each adapter (index based)
            var graph = new List<int>[joltageRatings.Count];
            for (var i = 0; i < joltageRatings.Count; i++)
            {
                var rate = joltageRatings[i];
                graph[i] = joltageRatings
                    .Select((r, i) => i)
                    .Where(i => joltageRatings[i] > rate && joltageRatings[i] < rate + 4)
                    .ToList();
            }

            // store result for each node, calculate results from destination backwards to minimize recursion
            var nodeResults = new ulong[graph.Length];
            for (var i = graph.Length - 1; i >= 0; i--)
            {
                var result = 0UL;
                GetNumberOfArrangements(graph.ToArray(), i, graph.Length - 1, ref nodeResults, ref result);
                nodeResults[i] = result;
            }

            // stores result for all arrangements starting from 0 (which is at index 0)
            return nodeResults[0];
        }


        // Increment result each time destination was reached in a depth-first search
        private static void GetNumberOfArrangements(List<int>[] graph, int currentNode, int destination, ref ulong[] nodeResults, ref ulong result)
        {
            var adjacentNodes = graph[currentNode];
            foreach (var node in adjacentNodes)
            {
                if (node != destination)
                {
                    if (nodeResults[node] != 0)
                    {
                        result += nodeResults[node];
                    }
                    else
                    {
                        GetNumberOfArrangements(graph, node, destination, ref nodeResults, ref result);
                    }
                }
                else
                {
                    result += 1;
                }
            }
        }
    }
}
