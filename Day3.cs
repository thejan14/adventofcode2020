namespace Solution
{
    using System;
    using System.IO;
    using System.Linq;

    public static class Day3
    {
        private static int patternSize = 31;

        public static void Solve()
        {
            var patternList = File.ReadAllLines("Day3.data");
            var slopes = new (int traverseX, int traverseY)[] {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2),
            };

            var slopeTreeEncounters = slopes.Select(s => GetTreeEncounters(patternList, s.traverseX, s.traverseY)).ToArray();

            Console.WriteLine($"(1) Number of tree encounters: {slopeTreeEncounters[1]}");

            var productOfSlopeEncounters = 1L;
            foreach (var treeEncounters in slopeTreeEncounters)
            {
                productOfSlopeEncounters *= treeEncounters;
            }

            Console.WriteLine($"(2) Prodcut of all slopes tree encounters: {productOfSlopeEncounters}");

        }

        public static int GetTreeEncounters(string[] patternList, int traveseX, int traverseY)
        {
            var treeEncounters = 0;
            var posX = 0;
            for (var posY = 0; posY < patternList.Length; posY += traverseY)
            {
                if (CheckForTree(patternList[posY], posX))
                {
                    treeEncounters += 1;
                }

                posX += traveseX;
            }

            return treeEncounters;
        }

        public static bool CheckForTree(string pattern, int posX)
        {
            return pattern[posX % patternSize] == '#';
        }
    }
}
