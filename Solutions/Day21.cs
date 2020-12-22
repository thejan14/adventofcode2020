namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day21 : Solution
    {
        private static readonly Regex ingredientsRegex = new Regex(@"((?:\w+ {0,1})+) \(contains ((?:\w+(?:, ){0,1})+)\)");

        public override void Solve(string dataPath)
        {
            var ingredientsList = File.ReadAllLines(dataPath);
            Console.WriteLine($"(1) Number of inert ingredients occurences: {GetNumberOfInertIngredients(ingredientsList, out var dangerousIngredients)}");
            Console.WriteLine($"(2) Canonical dangerous ingredient list: {string.Join(',', dangerousIngredients)}");
        }

        private static int GetNumberOfInertIngredients(string[] ingredientsList, out List<string> dangerousIngredients)
        {
            var parsedList = ingredientsList.Select(s =>
            {
                var match = ingredientsRegex.Match(s);
                return (ingredients: match.Groups[1].Value.Split(' ').ToHashSet(), allergenes: match.Groups[2].Value.Split(", "));
            });

            var ingredients = parsedList.SelectMany(e => e.ingredients);
            var allergenesDict = new Dictionary<string, HashSet<string>>();
            parsedList.SelectMany(e => e.allergenes).Distinct().ToList().ForEach(a => allergenesDict.Add(a, ingredients.ToHashSet()));

            foreach (var entry in parsedList)
            {
                foreach (var allergene in entry.allergenes)
                {
                    allergenesDict[allergene].IntersectWith(entry.ingredients);
                }
            }

            var allergeneToIngredientMap = new List<(string allergene, string ingredient)>();
            var nextUnambigousEntry = allergenesDict.FirstOrDefault(kv => kv.Value.Count == 1);
            while (nextUnambigousEntry.Value != null)
            {
                var ingredientMatch = nextUnambigousEntry.Value.First();
                allergeneToIngredientMap.Add((nextUnambigousEntry.Key, ingredientMatch));
                foreach (var entry in allergenesDict)
                {
                    entry.Value.Remove(ingredientMatch);
                }

                nextUnambigousEntry = allergenesDict.FirstOrDefault(kv => kv.Value.Count == 1);
            }

            dangerousIngredients = allergeneToIngredientMap.OrderBy(map => map.allergene).Select(map => map.ingredient).ToList();

            var allergeneIngredients = allergenesDict.Values.Aggregate(new HashSet<string>(), (acc, ingredients) => acc.Union(ingredients).ToHashSet()).ToList();
            return parsedList.SelectMany(e => e.ingredients).Count(i => !allergeneIngredients.Contains(i));
        }
    }
}
