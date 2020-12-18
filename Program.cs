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
                Console.WriteLine("---------------------\n");
                Console.ForegroundColor = ConsoleColor.White;
            }

            var dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            var solutions = Assembly.GetAssembly(typeof(Solution)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Solution)));
            
            var stopwatch = new Stopwatch();
            if (Debugger.IsAttached)
            {
                var currentSolution = solutions.Last();
                PrintSolutionHeader(currentSolution.Name);
                BenchmarkSolution(stopwatch, (Solution)Activator.CreateInstance(currentSolution), Path.Combine(dataFolder, $"{currentSolution.Name}.data"));
                Console.WriteLine();
            }
            else
            {
                foreach (var type in solutions)
                {
                    PrintSolutionHeader(type.Name);
                    BenchmarkSolution(stopwatch, (Solution)Activator.CreateInstance(type), Path.Combine(dataFolder, $"{type.Name}.data"));
                    Console.WriteLine();
                }
            }
        }

        private static void PrintSolutionHeader(string name)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"--- {name} ---");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void BenchmarkSolution(Stopwatch stopwatch, Solution solution, string dataPath)
        {
            stopwatch.Reset();
            stopwatch.Start();
            solution.Solve(dataPath);
            stopwatch.Stop();
            Console.ForegroundColor = stopwatch.ElapsedMilliseconds > 100
                ? stopwatch.ElapsedMilliseconds > 1000 ? ConsoleColor.Red : ConsoleColor.Yellow
                : ConsoleColor.Green;
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
