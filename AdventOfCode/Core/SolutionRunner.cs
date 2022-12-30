using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Core
{
    class SolutionRunner : IRunner
    {
        private SolutionMapping[] solutions;

        public SolutionRunner(YearManager yearManager)
        {
            yearManager.Subscribe(SetYear);
            SetYear(yearManager.CurrentYear);
        }

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

        public void PrintStartupMessage()
        {
            Console.WriteLine("Leave blank for latest solution, or enter day and part numbers to target specific solution");
            Console.WriteLine("Input will be copied from clipboard.");
        }

        public bool CheckRequest(string[] args)
        {
            var matching = GetMatchingSolution(args);

            if (matching == null)
                return false;

            RunInstanceOfSolution(matching);

            return true;
        }

        public double RunTimingTest(string[] args)
        {
            var matching = GetMatchingSolution(args);

            if (matching == null)
                throw new Exception("Failed to find solution to test");

            var runs = 10;
            var total = 0D;
            var input = Clipboard.Read();
            // Warm up
            RunIsolatedInstance(input, matching);

            for(var i = 0; i < runs; i++)
                total += RunIsolatedInstance(input, matching).Duration;

            return total / runs;

        }

        private SolutionMapping GetMatchingSolution(string[] args)
        {
            var matchingList = new SolutionMapping[0];

            if (args.Length == 0)
            {
                if (solutions.Length == 0)
                {
                    Console.WriteLine("Unable to find any solutions for specified problem");

                    return null;
                }

                var latest = solutions.OrderByDescending(it => it.Day).ThenByDescending(it => it.Problem).First();

                matchingList = solutions.Where(it => it.Day == latest.Day && it.Problem == latest.Problem).ToArray();
            }
            else if (args.Length == 2 && int.TryParse(args[0], out var day) && int.TryParse(args[1], out var problem))
            {
                matchingList = solutions.Where(it => it.Day == day && it.Problem == problem).ToArray();
            }
            else
                return null;

            if(matchingList.Length == 0)
            {
                Console.WriteLine("Unable to find any solutions for specified problem");

                return null;
            }

            if(matchingList.Length > 1)
            {
                Console.WriteLine("Enter which version you want to run (blank works for default)");
                Console.WriteLine($"({string.Join("/", solutions.Select(it => it.Name ?? "default"))})");
                var input = Console.ReadLine().ToLower();

                if ((string.IsNullOrEmpty(input) || input == "default") && solutions.Any(it => it.Name == null))
                    return solutions.First(it => it.Name == null);
                else
                {
                    var matching = solutions.First(it => it.Name?.ToLower()?.Contains(input) == true);

                    if (matching != null)
                        return matching;
                    else
                    {
                        Console.WriteLine("Unable to find matching solution");

                        return null;
                    }
                }
            }

            return matchingList[0];
        }

        private void RunInstanceOfSolution(SolutionMapping solutionMapping)
        {
            var input = Clipboard.Read();

            try
            {
                Console.WriteLine($"Running solution for day {solutionMapping.Day} part {solutionMapping.Problem}...");
                var output = RunIsolatedInstance(input, solutionMapping);
                Console.WriteLine($"Solution for day {solutionMapping.Day}, problem {solutionMapping.Problem}:");
                Console.WriteLine(output.Result);
                Console.WriteLine($"Elapsed time: {output.Duration} seconds");
                Clipboard.Write(output.Result?.ToString() ?? string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Generate answer: " + e.InnerException?.Message ?? e.Message);
            }
        }

        private (string Result, double Duration) RunIsolatedInstance(string input, SolutionMapping solutionMapping)
        {
            var instance = Activator.CreateInstance(solutionMapping.ClassType);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = solutionMapping.Method.Invoke(instance, new object[] { input });
            stopwatch.Stop();

            return (result.ToString(), stopwatch.Elapsed.TotalSeconds);
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
