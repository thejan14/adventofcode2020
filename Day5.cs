namespace Solution
{
    using System;
    using System.IO;
    using System.Linq;

    public static class Day5
    {
        public static void Solve()
        {
            var seatStrings = File.ReadAllLines("Day5.data");
            var orderedSeatIDs = seatStrings.Select(s => GetSeatID(s)).OrderBy(id => id).ToArray();
            Console.WriteLine($"(1) Highest seat ID: {orderedSeatIDs.Max()}");

            var missingSeatID = GetMissingSeatID(orderedSeatIDs);
            Console.WriteLine($"(2) Missing seat ID: {missingSeatID}");
        }

        // expect seat IDs in ascending order
        private static int GetMissingSeatID(int[] orderedSeatIDs)
        {
            for (var i = 0; i < orderedSeatIDs.Length - 1; i++)
            {
                if (orderedSeatIDs[i] + 1 != orderedSeatIDs[i + 1])
                {
                    return orderedSeatIDs[i] + 1;
                }
            }

            return 0;
        }

        private static int GetSeatID(string seatString)
        {
            var row = GetRowNumber(seatString);
            var column = GetColumnNumber(seatString);
            return row * 8 + column;
        }

        private static byte GetRowNumber(string seatString)
        {
            // treat row as the last 7 digits of a byte where F=0 and B=1
            byte row = 0b_0000_0000;
            // use flag to set bits in row in via bitwise OR
            byte flag = 0b_0100_0000;
            foreach (var c in seatString.Take(7))
            {
                if (c == 'B')
                {
                    row |= flag;
                }

                flag /= 2;
            }

            return row;
        }

        private static byte GetColumnNumber(string seatString)
        {
            // treat column as the last 3 digits of a byte where L=0 and R=1
            byte column = 0b_0000_0000;
            // use flag to set bits in column in via bitwise OR
            byte flag = 0b_0000_0100;
            foreach (var c in seatString.Skip(7))
            {
                if (c == 'R')
                {
                    column |= flag;
                }

                flag /= 2;
            }

            return column;
        }
    }
}
