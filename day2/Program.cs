namespace Solution
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Program
    {
        private static readonly Regex PolicyAndPasswordRegex = new Regex(@"(\d+)-(\d+) ([\w]): (.*)");

        public static void Main()
        {
            var policyAndPasswordList = File.ReadAllLines("input.txt");

            var numberOfValidPasswords = 0;
            foreach (var policyAndPassword in policyAndPasswordList)
            {
                var match = PolicyAndPasswordRegex.Match(policyAndPassword);
                if (match.Success
                    && match.Groups.Count == 5
                    && int.TryParse(match.Groups[1].Value, out var minOccurences)
                    && int.TryParse(match.Groups[2].Value, out var maxOccurences))
                {
                    var enforcedCharacer = Convert.ToChar(match.Groups[3].Value);
                    var password = match.Groups[4].Value;
                    if (PasswordValid(password, enforcedCharacer, minOccurences, maxOccurences))
                    {
                        numberOfValidPasswords += 1;
                    }
                }
                else
                {
                    Console.WriteLine($"Input policy/password not in expected format: '{policyAndPassword}'");
                    return;
                }
            }

            Console.WriteLine($"Number of invalid passwords: {numberOfValidPasswords}");
        }

        public static bool PasswordValid(string password, char enforcedCharacer, int minOccurences, int maxOccurences)
        {
            var occurences = password.Count(c => c == enforcedCharacer);
            return occurences >= minOccurences && occurences <= maxOccurences;
        }
    }
}
