namespace ExpensesReport
{
    using System;
    using System.IO;
    using System.Linq;

    public class Program
    {
        public static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var expenses = input.Select(i => int.Parse(i)).ToArray();

            _ = TryGetTwoAddends(expenses, 2020, out var leftHandAddend, out var rightHandAddend);
            Console.WriteLine($"(1) {leftHandAddend} * {rightHandAddend} = {leftHandAddend * rightHandAddend}");

            _ = TryGetThreeAddends(expenses, 2020, out var firstAddend, out var secondAddend, out var thirdAddend);
            Console.WriteLine($"(2) {firstAddend} * {secondAddend} * {thirdAddend} = {firstAddend * secondAddend * thirdAddend}");
        }

        public static bool TryGetThreeAddends(int[] numbers, int targetSum, out int firstAddend, out int secondAddend, out int thirdAddend)
        {
            firstAddend = 0;
            secondAddend = 0;
            thirdAddend = 0;

            var i = 0;
            while (i < numbers.Length)
            {
                var leftoverSum = targetSum - numbers[i];
                if (TryGetTwoAddends(numbers.Where(n => n < leftoverSum).ToArray(), leftoverSum, out secondAddend, out thirdAddend))
                {
                    firstAddend = numbers[i];
                    return true;
                }

                i += 1;
            }

            return false;
        }

        public static bool TryGetTwoAddends(int[] numbers, int targetSum, out int leftHandAddend, out int rightHandAddend)
        {
            leftHandAddend = 0;
            rightHandAddend = 0;

            var i = 0;
            while (i < numbers.Length)
            {
                var difference = targetSum - numbers[i];
                var foundResult = numbers.Skip(i).FirstOrDefault(e => e == difference);
                if (foundResult != 0)
                {
                    leftHandAddend = numbers[i];
                    rightHandAddend = foundResult;
                    return true;
                }

                i += 1;
            }

            return false;
        }
    }
}
