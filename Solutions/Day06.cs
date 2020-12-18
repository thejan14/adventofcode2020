namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day06 : Solution
    {
        public override void Solve(string dataPath)
        {
            var data = File.ReadAllText(dataPath);
            var answerList = data.Split("\n\n");

            var anyAnswerCounts = GetAnyAnswerCounts(answerList);
            Console.WriteLine($"(1) Sum of all groups answer counts (anyone answered): {anyAnswerCounts.Sum()}");

            var allAnswerCounts = GetAllAnswerCounts(answerList);
            Console.WriteLine($"(2) Sum of all groups answer counts (all answered): {allAnswerCounts.Sum()}");
        }

        private static IEnumerable<int> GetAnyAnswerCounts(string[] answerList)
        {
            return answerList.Select(customsData =>
            {
                return customsData
                    .Select(c => Convert.ToInt32(c))
                    .Where(c => c > 96 && c < 123) // 97 = 'a'; 122 = 'z'
                    .Distinct()
                    .Count();
            });
        }

        private static IEnumerable<int> GetAllAnswerCounts(string[] answerList)
        {
            return answerList.Select(customsData =>
            {
                return customsData
                    .Split("\n")
                    .Aggregate((a, b) => new string(a.Intersect(b).ToArray()))
                    .Count();
            });
        }
    }
}
