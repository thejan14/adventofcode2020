namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day21 : Solution
    {
        private static readonly Regex ingrediendsRegex = new Regex(@"((?:\w+ {0,1})+) \(contains ((?:\w+(?:, ){0,1})+)\)");

        public override void Solve(string dataPath)
        {
            var ingrediendsList = File.ReadAllLines(dataPath);
            Console.WriteLine($"Number of inert ingredience occurences: {GetNumberOfInertIngrediends(ingrediendsList)}");
        }

        private static int GetNumberOfInertIngrediends(string[] ingrediendsList)
        {
            var parsedList = ingrediendsList.Select(s =>
            {
                var match = ingrediendsRegex.Match(s);
                return (ingrediends: match.Groups[1].Value.Split(' ').ToHashSet(), allergenes: match.Groups[2].Value.Split(", "));
            });

            var ingrediends = parsedList.SelectMany(e => e.ingrediends);
            var allergenesDict = new Dictionary<string, HashSet<string>>();
            parsedList.SelectMany(e => e.allergenes).Distinct().ToList().ForEach(a => allergenesDict.Add(a, ingrediends.ToHashSet()));

            foreach (var entry in parsedList)
            {
                foreach (var allergene in entry.allergenes)
                {
                    allergenesDict[allergene].IntersectWith(entry.ingrediends);
                }
            }

            var allergeneIngrediends = allergenesDict.Values.Aggregate(new HashSet<string>(), (acc, ingrediends) => acc.Union(ingrediends).ToHashSet()).ToList();
            return parsedList.SelectMany(e => e.ingrediends).Count(i => !allergeneIngrediends.Contains(i));
        }
    }
}
