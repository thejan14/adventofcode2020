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
            var maxSeatID = seatStrings.Select(s => GetSeatID(s)).Max();
            Console.WriteLine($"Highest seat ID: {maxSeatID}");
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
            // use flag to set bits in row in via bitwise OR
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
