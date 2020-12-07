namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Day7
    {
        private static readonly Regex ruleBagColorRegex = new Regex(@"(\w+ \w+) bags contain");
        private static readonly Regex bagContentRuleRegex = new Regex(@"(\d+) (\w+ \w+) bags?[,.]");

        // Dictionary<bagColor, Dictionary<bagColor, numberOfAllowed>>
        private static readonly Dictionary<string, Dictionary<string, int>> bagContentRules
            = new Dictionary<string, Dictionary<string, int>>();

        public static void Solve()
        {
            var rulesList = File.ReadAllLines("Day7.data");
            PopulateRules(rulesList);

            var allowedBags = GetAllowedBagsFor("shiny gold");
            Console.WriteLine($"Number of bag colors that can contain at least one 'shiny golden' bag: {allowedBags.Length}");
        }

        private static void PopulateRules(string[] rulesList)
        {
            foreach (var rule in rulesList)
            {
                var contentRulesDict = new Dictionary<string, int>();
                var bagColor = ruleBagColorRegex.Match(rule).Groups[1].Value;
                foreach (Match contentRuleMatch in bagContentRuleRegex.Matches(rule))
                {
                    var contentBagColor = contentRuleMatch.Groups[2].Value;
                    var allowedAmount = int.Parse(contentRuleMatch.Groups[1].Value);
                    contentRulesDict.Add(contentBagColor, allowedAmount);
                }

                bagContentRules.Add(bagColor, contentRulesDict);
            }
        }

        private static string[] GetAllowedBagsFor(string bagColor)
        {
            return GetAllowedBagsFor(bagColor, new HashSet<string>());
        }

        private static string[] GetAllowedBagsFor(string bagColor, HashSet<string> allowedBags)
        {
            var newAllowedBags = bagContentRules
                .Where((kv) => kv.Value.ContainsKey(bagColor))
                .Select(kv => kv.Key)
                .ToHashSet()
                .Except(allowedBags)
                .ToArray();

            allowedBags.UnionWith(newAllowedBags);
            foreach (var allowedBagColor in newAllowedBags)
            {
                allowedBags.UnionWith(GetAllowedBagsFor(allowedBagColor, allowedBags));
            }

            return allowedBags.ToArray();
        }
    }
}
