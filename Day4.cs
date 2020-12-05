namespace Solution
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public delegate bool FieldValidator(string passportData);

    public static class Day4
    {
        private static readonly string[] mandatoryFields = { "byr:", "iyr:", "eyr:", "hgt:", "hcl:", "ecl:", "pid:", };

        // (?:[ \n]|(?:)$) => End of text or whitespace/newline in non-capture groups
        private static readonly Regex byrRegex = new Regex(@"byr:(\d{4})(?:[ \n]|(?:)$)");
        private static readonly Regex iyrRegex = new Regex(@"iyr:(\d{4})(?:[ \n]|(?:)$)");
        private static readonly Regex eyrRegex = new Regex(@"eyr:(\d{4})(?:[ \n]|(?:)$)");
        private static readonly Regex hgtRegex = new Regex(@"hgt:(\d{2,3})(cm|in)(?:[ \n]|(?:)$)");
        private static readonly Regex hclRegex = new Regex(@"hcl:#([0-9a-f]{6})(?:[ \n]|(?:)$)");
        private static readonly Regex eclRegex = new Regex(@"ecl:(amb|blu|brn|gry|grn|hzl|oth)(?:[ \n]|(?:)$)");
        private static readonly Regex pidRegex = new Regex(@"pid:(\d{9})(?:[ \n]|(?:)$)");

        private static readonly FieldValidator[] fieldValidators = {
            (string passportData) => CheckMatchRange(byrRegex.Match(passportData), 1920, 2002),
            (string passportData) => CheckMatchRange(iyrRegex.Match(passportData), 2010, 2020),
            (string passportData) => CheckMatchRange(eyrRegex.Match(passportData), 2020, 2030),
            (string passportData) => CheckHeightRange(hgtRegex.Match(passportData)),
            (string passportData) => hclRegex.IsMatch(passportData),
            (string passportData) => eclRegex.IsMatch(passportData),
            (string passportData) => pidRegex.IsMatch(passportData),
        };

        public static void Solve()
        {
            var passportList = File.ReadAllText("Day4.data").Split("\n\n");
            var numberOfValidPassports1 = passportList.Count(p => CheckPassportForMandatoryFields(p));
            Console.WriteLine($"(1) Number of valid passports (mandatory fields): {numberOfValidPassports1}");

            var numberOfValidPassports2 = passportList.Count(p => CheckPassportWithValidation(p));
            Console.WriteLine($"(2) Number of valid passports (field validation): {numberOfValidPassports2}");
        }

        private static bool CheckPassportForMandatoryFields(string passportData)
        {
            return mandatoryFields.All(field => passportData.Contains(field));
        }

        private static bool CheckPassportWithValidation(string passportData)
        {
            return fieldValidators.All(validator => validator(passportData));
        }

        private static bool CheckHeightRange(Match hgtMatch)
        {
            if (hgtMatch.Success && hgtMatch.Groups.Count == 3)
            {
                return hgtMatch.Groups[2].Value switch
                {
                    "cm" => CheckMatchRange(hgtMatch, 150, 193),
                    "in" => CheckMatchRange(hgtMatch, 59, 176),
                    _ => false
                };
            }

            return false;
        }

        // expect match to have numeric value in group 1
        private static bool CheckMatchRange(Match match, int min, int max)
        {
            if (match.Success && match.Groups.Count > 1 && int.TryParse(match.Groups[1].Value, out var number))
            {
                return number >= min && number <= max;
            }

            return false;
        }
    }
}
