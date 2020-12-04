namespace Solution
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Day4
    {
        private static readonly string[] mandatoryFields = { "byr:", "iyr:", "eyr:", "hgt:", "hcl:", "ecl:", "pid:", };

        public static void Solve()
        {
            var passportList = File.ReadAllText("Day4.data").Split("\n\n");
            var numberOfValidPassports = passportList.Count(p => CheckPassportForMandatoryFields(p));
            Console.WriteLine($"Number of valid passports: {numberOfValidPassports}");
        }

        private static bool CheckPassportForMandatoryFields(string passportData)
        {
            return mandatoryFields.All(field => passportData.Contains(field));
        }
    }
}
