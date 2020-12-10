namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Day9
    {
        public static void Solve()
        {
            var data = File.ReadAllLines("Day9.data").Select(line => long.Parse(line)).ToArray();
            Console.WriteLine($"First invalid number: {GetFirstInvalidNumber(data)}");
        }

        private static long GetFirstInvalidNumber(long[] data)
        {
            var firstInvalidNumber = 0L;
            for (var i = 25; i < data.Length; i++)
            {
                if (!CheckXMAS(new ArraySegment<long>(data, i - 25, 25), data[i]))
                {
                    return data[i];
                }
            }

            return firstInvalidNumber;
        }

        private static bool CheckXMAS(IEnumerable<long> preamble, long target)
        {
            return preamble.Any(n => preamble.Contains(target - n));
        }
    }
}
