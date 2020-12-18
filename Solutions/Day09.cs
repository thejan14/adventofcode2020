namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day09 : Solution
    {
        public override void Solve(string dataPath)
        {
            var data = File.ReadAllLines(dataPath).Select(line => long.Parse(line)).ToArray();
            var invalidNumber = GetFirstInvalidNumber(data);
            Console.WriteLine($"(1) First invalid number: {invalidNumber}");

            var sumSet = GetContiguousSumSet(data, invalidNumber);
            Console.WriteLine($"(2) Encryption weakness sum (smallest + largest): {sumSet.Min() + sumSet.Max()}");
        }

        private static IEnumerable<long> GetContiguousSumSet(long[] data, long target)
        {
            var start = 0;
            var end = 0;
            var found = false;
            while (!found && start < data.Length)
            {
                end = start + 1;
                var sum = data[start] + data[end];
                while (sum < target && !found)
                {
                    end += 1;
                    sum += data[end];
                    if (sum == target)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    start += 1;
                }
            }

            return data.Skip(start).Take(end - start);
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
