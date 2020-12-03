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

            var numberOfValidPasswordsOld = 0;
            var numberOfValidPasswordsNew = 0;
            foreach (var policyAndPassword in policyAndPasswordList)
            {
                var match = PolicyAndPasswordRegex.Match(policyAndPassword);
                if (match.Success
                    && match.Groups.Count == 5
                    && int.TryParse(match.Groups[1].Value, out var leftHandPolicy)
                    && int.TryParse(match.Groups[2].Value, out var rightHandPolicy))
                {
                    var enforcedCharacer = Convert.ToChar(match.Groups[3].Value);
                    var password = match.Groups[4].Value;
                    if (PasswordValidOld(password, enforcedCharacer, leftHandPolicy, rightHandPolicy))
                    {
                        numberOfValidPasswordsOld += 1;
                    }

                    if (PasswordValidNew(password, enforcedCharacer, leftHandPolicy, rightHandPolicy))
                    {
                        numberOfValidPasswordsNew += 1;
                    }
                }
                else
                {
                    Console.WriteLine($"Input policy/password not in expected format: '{policyAndPassword}'");
                    return;
                }
            }

            Console.WriteLine($"(1) Number of valid passwords: {numberOfValidPasswordsOld}");
            Console.WriteLine($"(2) Number of valid passwords: {numberOfValidPasswordsNew}");
        }
        public static bool PasswordValidOld(string password, char enforcedCharacer, int minOccurences, int maxOccurences)
        {
            var occurences = password.Count(c => c == enforcedCharacer);
            return occurences >= minOccurences && occurences <= maxOccurences;
        }

        public static bool PasswordValidNew(string password, char enforcedCharacer, int firstPos, int secondPos)
        {
            var firstPosMatch = password[firstPos - 1] == enforcedCharacer;
            var secondPosMatch = password[secondPos - 1] == enforcedCharacer;
            return (firstPosMatch && !secondPosMatch) || (!firstPosMatch && secondPosMatch);
        }
    }
}
