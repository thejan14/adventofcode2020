namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day19 : Solution
    {
        public override void Solve(string dataPath)
        {
            var data = File.ReadAllText(dataPath).Split("\n\n");
            var rulesData = data[0];
            var messages = data[1].Split('\n');
            var ruleRegex = ParseRulesData(rulesData, false);
            Console.WriteLine($"(1) Number of valid messages: {messages.Count(m => ruleRegex.IsMatch(m))}");

            var updatedRuleRegex = ParseRulesData(rulesData, true);
            var numberOfMatches = messages.Count(m =>
            {
                var match = updatedRuleRegex.Match(m);
                Console.WriteLine(match.Groups["x"].Captures.Count);
                return match.Success;
            });

            Console.WriteLine(numberOfMatches);
        }

        private static Regex ParseRulesData(string rulesData, bool updatedRules)
        {
            var rulesDict = new Dictionary<int, string>();
            foreach (var rule in rulesData.Split('\n').Select(s => s.Split(':')))
            {
                rulesDict.Add(int.Parse(rule[0]), rule[1].Trim());
            }

            return new Regex($@"^{ParseRule(rulesDict, 0, updatedRules)}$");
        }

        private static string ParseRule(Dictionary<int, string> rulesDict, int ruleNumber, bool updatedRules)
        {
            if (updatedRules && ruleNumber == 8)
            {
                return $"({ParseRule(rulesDict, 42, updatedRules)})+";
            }

            if (updatedRules && ruleNumber == 11)
            {
                // use balancing groups to match 42 and 31 equally often (lhs and rhs respectively)
                // see https://docs.microsoft.com/en-us/dotnet/standard/base-types/grouping-constructs-in-regular-expressions?redirectedfrom=MSDN#balancing_group_definition
                var rule42 = ParseRule(rulesDict, 42, updatedRules);
                var rule31 = ParseRule(rulesDict, 31, updatedRules);
                return $"(((?'Open'{rule42})+(?'Close-Open'{rule31})+)+(?(Open)(?!)))";
            }

            var rule = rulesDict.GetValueOrDefault(ruleNumber);
            if (rule.StartsWith('"'))
            {
                return rule.Substring(1, 1);
            }
            else
            {
                var subRule = string.Join("", rule
                    .Split(' ')
                    .Select(s => int.TryParse(s, out var num) ? ParseRule(rulesDict, num, updatedRules) : s));
                return $"({subRule})";
            }
        }
    }
}
