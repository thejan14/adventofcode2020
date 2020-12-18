namespace Solution
{
    using System;
    using System.IO;
    using System.Linq;

    public class Day03 : Solution
    {
        private static int patternSize = 31;

        public override void Solve(string dataPath)
        {
            var patternList = File.ReadAllLines(dataPath);
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

        private static int GetTreeEncounters(string[] patternList, int traveseX, int traverseY)
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

        private static bool CheckForTree(string pattern, int posX)
        {
            return pattern[posX % patternSize] == '#';
        }
    }
}
