namespace Solution
{
    using System;
    using System.IO;
    using System.Linq;

    public static class Day6
    {
        public static void Solve()
        {
            var data = File.ReadAllText("Day6.data");
            var answerList = data.Split("\n\n");
            var answerCounts = answerList.Select(customsData =>
            {
                return customsData
                    .Select(c => Convert.ToInt32(c))
                    .Where(c => c > 96 && c < 123) // 97 = 'a'; 122 = 'z'
                    .Distinct()
                    .Count();
            });

            Console.WriteLine($"Sum of all groups answer counts: {answerCounts.Sum()}");
        }
    }
}
