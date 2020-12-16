namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Rule
    {
        /*public Rule(string name, (int lower, int upper) validRange, (int lower, int upper) alternateValidRange)
        {
            this.Name = name;
            this.ValidRange = validRange;
            this.AlternateValidRange = alternateValidRange;
        }*/

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

            Console.WriteLine($"Ticket scanning error rate: {CalculateErrorRate(nearbyTicketData, rules)}");
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

        private static int CalculateErrorRate(string nearbyTicketData, Rule[] rules)
        {
            var ticketData = nearbyTicketData
                .Split('\n')
                .Skip(1) // first line is "nearby tickets:"
                .SelectMany(ticket =>
                    ticket.Split(',')
                    .Select(num => int.Parse(num)))
                .ToArray();

            return ticketData.Where(d => !rules.Any(r => r.AppliesTo(d))).Sum();
        }
    }
}
