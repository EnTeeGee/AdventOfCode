using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Core
{
    class SolutionRunner : IRunner
    {
        private SolutionMapping[] solutions;

        public void SetYear(int year)
        {
            var targets = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(it => it.FullName.Contains(year.ToString()))
                .SelectMany(it => it.GetMethods()
                    .Where(m => m.GetCustomAttribute<SolutionAttribute>() != null)
                    .Select(m => new { Type = it, Method = m, Info = m.GetCustomAttribute<SolutionAttribute>() }));

            solutions = targets.Select(it => new SolutionMapping
            {
                Day = it.Info.Day,
                Problem = it.Info.Problem,
                Name = it.Info.Name,
                ClassType = it.Type,
                Method = it.Method,
            }).ToArray();
        }

        public bool CheckRequest(string[] args)
        {
            if (args.Length == 0)
            {
                if (solutions.Length == 0)
                {
                    RunSolutions(new SolutionMapping[0]);

                    return true;
                }
                    
                var latest = solutions.OrderByDescending(it => it.Day).ThenByDescending(it => it.Problem).First();

                RunSolutions(solutions.Where(it => it.Day == latest.Day && it.Problem == latest.Problem).ToArray());
            }
            else if (args.Length == 2 && int.TryParse(args[0], out var day) && int.TryParse(args[1], out var problem))
            {
                RunSolutions(solutions.Where(it => it.Day == day && it.Problem == problem).ToArray());
            }
            else
                return false;

            return true;
        }

        private void RunSolutions(SolutionMapping[] solutions)
        {
            if(solutions.Length == 0)
            {
                Console.WriteLine("Unable to find any solutions for specified problem");

                return;
            }
            else if (solutions.Length == 1)
            {
                RunInstanceOfSolution(solutions[0]);
            }
            else
            {
                Console.WriteLine("Enter which version you want to run (blank works for default)");
                Console.WriteLine($"({string.Join("/", solutions.Select(it => it.Name ?? "default"))})");
                var input = Console.ReadLine().ToLower();

                if ((string.IsNullOrEmpty(input) || input == "default") && solutions.Any(it => it.Name == null))
                    RunInstanceOfSolution(solutions.First(it => it.Name == null));
                else
                {
                    var matching = solutions.First(it => it.Name.ToLower().Contains(input));

                    if (matching != null)
                        RunInstanceOfSolution(matching);
                    else
                        Console.WriteLine("Unable to find matching solution");
                }
            }
        }

        private void RunInstanceOfSolution(SolutionMapping solutionMapping)
        {
            var input = Clipboard.Read();
            var instance = Activator.CreateInstance(solutionMapping.ClassType);

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = solutionMapping.Method.Invoke(instance, new object[] { input });
                stopwatch.Stop();
                Console.WriteLine($"Solution for day {solutionMapping.Day}, problem {solutionMapping.Problem}:");
                Console.WriteLine(result);
                Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalSeconds} seconds");
                Clipboard.Write(result?.ToString() ?? string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Generate answer: " + e.Message);
            }
        }

        private class SolutionMapping
        {
            public int Day { get; set; }

            public int Problem { get; set; }

            public string Name { get; set; }

            public Type ClassType { get; set; }

            public MethodInfo Method { get; set; }
        }
    }
}
