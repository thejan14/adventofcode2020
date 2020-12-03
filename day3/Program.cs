namespace Solution
{
    using System;
    using System.IO;

    public class Program
    {
        private static int patternSize = 31;

        public static void Main()
        {
            var patternList = File.ReadAllLines("input.txt");
            
            var treeEncounters = 0;
            var posX = 0;
            for (var posY = 0; posY < patternList.Length; posY += 1)
            {
                if (CheckForTree(patternList[posY], posX))
                {
                    treeEncounters += 1;
                }
                
                posX += 3;
            }

            Console.WriteLine($"Number of tree encounters: {treeEncounters}");
        }

        public static bool CheckForTree(string pattern, int posX)
        {
            return pattern[posX % patternSize] == '#';
        }
    }
}
