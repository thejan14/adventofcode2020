namespace ExpensesReport
{
    using System;
    using System.IO;
    using System.Linq;

    public class Program
    {
        private const int targetSum = 2020;

        static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var expenses = input.Select(i => int.Parse(i)).ToArray();

            var i = 0;
            var found = false;
            while (!found && i < expenses.Length)
            {
                var difference = targetSum - expenses[i];
                var foundResult = expenses.Skip(i).FirstOrDefault(e => e == difference);
                if (foundResult != 0)
                {
                    Console.WriteLine($"{expenses[i]} * {foundResult} = {expenses[i] * foundResult}");
                    found = true;
                }

                i += 1;
            }
        }
    }
}
