namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Rule
    {
        public string Name { get; init; }

        public (int lower, int upper) ValidRange { get; init; }

        public (int lower, int upper) AlternateValidRange { get; init; }

        public bool AppliesTo(int value)
        {
            return value >= ValidRange.lower && value <= ValidRange.upper
                || value >= AlternateValidRange.lower && value <= AlternateValidRange.upper;
        }
    }

    public static class Day16
    {
        private static readonly Regex ruleRegex = new Regex(@"(.+): (\d+)-(\d+) or (\d+)-(\d+)");

        public static void Solve()
        {
            var data = File.ReadAllText("Day16.data").Split("\n\n");
            var rules = ParseRules(data[0]);
            var nearbyTicketData = data[2];

            Console.WriteLine($"(1) Ticket scanning error rate: {CalculateErrorRate(nearbyTicketData, rules)}");

            var orderedRules = DetermineRulesOrder(nearbyTicketData, rules);
            var departureSum = GetDepartureProduct(data[1], orderedRules);
            Console.WriteLine($"(2) Sum of all departure values: {departureSum}");
        }

        private static long GetDepartureProduct(string yourTicketData, Rule[] orderedRules)
        {
            var product = 1L;
            var ticketValues = yourTicketData
                .Split('\n')
                .Skip(1) // first line is "your ticket:"
                .SelectMany(s => s.Split(','))
                .Select(num => int.Parse(num))
                .ToArray();

            for (var i = 0; i < orderedRules.Length; i++)
            {
                if (orderedRules[i].Name.StartsWith("departure"))
                {
                    product *= ticketValues[i];
                }
            }

            return product;
        }

        private static Rule[] DetermineRulesOrder(string nearbyTicketData, Rule[] rules)
        {
            var ticketData = nearbyTicketData
                .Split('\n')
                .Skip(1) // first line is "nearby tickets:"
                .Select(ticket =>
                    ticket.Split(',').Select(num => int.Parse(num)));

            // for each ticket (line) for each value (comma separated)
            // get the list of rules that apply
            var ticketValidationData = ticketData
                .Select(ticket =>
                    ticket
                    .Select(value =>
                        rules.Select(r => r.AppliesTo(value)).ToArray())
                    .ToArray())
                .Where(appliedRules => !appliedRules.Any(list => list.All(result => result == false))); // discard tickets with invalid fields

            var rulePositionOptions = GetRulePositionOptions(ticketValidationData, rules);

            var orderedRules = new Rule[rules.Length];
            var exclusivePositionRule = rulePositionOptions.FirstOrDefault(kv => kv.Value.Count(res => res == true) == 1);
            while (exclusivePositionRule.Key != null)
            {
                var ruleIndex = Array.FindIndex(rules, r => r.Name == exclusivePositionRule.Key);
                var posIndex = Array.IndexOf(exclusivePositionRule.Value, true);
                orderedRules[posIndex] = rules[ruleIndex];

                // as the position for this rule was found exclude the position for all rules
                foreach (var options in rulePositionOptions.Values)
                {
                    options[posIndex] = false;
                }

                exclusivePositionRule = rulePositionOptions.FirstOrDefault(kv => kv.Value.Count(res => res == true) == 1);
            }

            return orderedRules;
        }

        // for each rule get a boolean array indicating whether there was
        // a violation of the rule at this position or not
        private static Dictionary<string, bool[]> GetRulePositionOptions(IEnumerable<bool[][]> ticketValidationData, Rule[] rules)
        {
            var rulePositionOptions = new Dictionary<string, bool[]>();
            foreach (var rule in rules)
            {
                var options = new bool[rules.Length];
                Array.Fill(options, true);
                rulePositionOptions[rule.Name] = options;
            }

            // for each ticket
            foreach (var validationData in ticketValidationData)
            {
                // for each value
                for (var pos = 0; pos < validationData.Length; pos++)
                {
                    // for each rule validation result
                    for (var ruleIndex = 0; ruleIndex < validationData[pos].Length; ruleIndex++)
                    {
                        if (validationData[pos][ruleIndex] == false)
                        {
                            rulePositionOptions[rules[ruleIndex].Name][pos] = false;
                        }
                    }
                }
            }

            return rulePositionOptions;
        }

        private static int CalculateErrorRate(string nearbyTicketData, Rule[] rules)
        {
            var ticketData = nearbyTicketData
                .Split('\n')
                .Skip(1) // first line is "nearby tickets:"
                .SelectMany(ticket =>
                    ticket.Split(',')
                    .Select(num => int.Parse(num)));

            return ticketData.Where(d => !rules.Any(r => r.AppliesTo(d))).Sum();
        }

        private static Rule[] ParseRules(string ruleData)
        {
            return ruleData.Split('\n').Select(rule =>
            {
                var match = ruleRegex.Match(rule);
                return new Rule
                {
                    Name = match.Groups[1].Value,
                    ValidRange = (int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)),
                    AlternateValidRange = (int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value)),
                };
            }).ToArray();
        }
    }
}
