namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Day14
    {
        private static Regex memAssignementRegex = new Regex(@"mem\[(\d+)\] = (\d+)");

        public static void Solve()
        {
            var data = File.ReadAllLines("Day14.data");
            var memorySum = DecodeProgramm(data);
            Console.WriteLine($"Sum of all written memory after the programm completes: {memorySum}");
        }

        private static long DecodeProgramm(string[] data)
        {
            var mask = string.Empty;
            var memory = new Dictionary<int, long>();
            for (var line = 0; line < data.Length; line++)
            {
                var match = memAssignementRegex.Match(data[line]);
                if (match.Success && int.TryParse(match.Groups[1].Value, out var pos) && int.TryParse(match.Groups[2].Value, out var value))
                {
                    memory[pos] = ApplyMask(ref mask, value);
                }
                else
                {
                    mask = data[line].Substring(7);
                }
            }

            return memory.Values.Sum();
        }

        private static long ApplyMask(ref string mask, long value)
        {
            var binaryValue = Convert.ToString(value, 2);
            var newValue = mask.ToCharArray();
            for (var i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'X')
                {
                    // binary value may not be 36 bit resulting in less than 36 characters
                    // therefore start counting so that binaryValue fills up the lest significant bits
                    var valueIndex = i - (mask.Length - binaryValue.Length);
                    newValue[i] = valueIndex >= 0 ? binaryValue[valueIndex] : '0';
                }
            }

            return Convert.ToInt64(new string(newValue), 2);
        }
    }
}
