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
