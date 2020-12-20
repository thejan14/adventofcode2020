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
            var ruleRegex = ParseRulesData(rulesData);
            Console.WriteLine($"Number of valid messages: {messages.Count(m => ruleRegex.IsMatch(m))}");
        }

        private static Regex ParseRulesData(string rulesData)
        {
            var rulesDict = new Dictionary<int, string>();
            foreach (var rule in rulesData.Split('\n').Select(s => s.Split(':')))
            {
                rulesDict.Add(int.Parse(rule[0]), rule[1].Trim());
            }

            return new Regex($@"^{ParseRule(rulesDict, 0)}$");
        }

        private static string ParseRule(Dictionary<int, string> rulesDict, int ruleNumber)
        {
            var rule = rulesDict.GetValueOrDefault(ruleNumber);
            if (rule.StartsWith('"'))
            {
                return rule.Substring(1, 1);
            }
            else
            {
                var subRule = string.Join("", rule
                    .Split(' ')
                    .Select(s => int.TryParse(s, out var num) ? ParseRule(rulesDict, num) : s));
                return $"({subRule})";
            }
        }
    }
}
