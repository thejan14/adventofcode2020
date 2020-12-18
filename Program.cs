namespace Solution
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class Program
    {
        public static void Main()
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("REPL_ID")))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------ WARNING ------");
                Console.WriteLine("Please note that the solution for day 16 does not compile on repl as it uses C# 9.0 features and mono only supports features up to C# 7.0");
                Console.WriteLine("---------------------");
                Console.ForegroundColor = ConsoleColor.White;
            }

            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            var solutions = Assembly.GetAssembly(typeof(Solution)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Solution)));

            if (Debugger.IsAttached)
            {
                var currentSolution = solutions.Last();
                ((Solution)Activator.CreateInstance(currentSolution)).Solve(Path.Combine(dataPath, $"{currentSolution.Name}.data"));
            }
            else
            {
                foreach (var type in solutions)
                {
                    Console.WriteLine($"--- {type.Name} ---");
                    var solution = (Solution)Activator.CreateInstance(type);
                    solution.Solve(Path.Combine(dataPath, $"{type.Name}.data"));
                    Console.WriteLine();
                }
            }
        }
    }
}
