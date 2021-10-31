using AdventOfCode.Renderer;
using System;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Core
{
    class YearManager : IRunner
    {
        private int[] availableYears;
        private int currentYear;
        private SolutionRunner solutionRunner;
        private RendererRunner rendererRunner;

        public YearManager(SolutionRunner solutionRunner, RendererRunner rendererRunner)
        {
            availableYears = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(it => it.FullName.StartsWith("AdventOfCode.Solutions."))
                .Select(it => Convert.ToInt32(it.FullName.Split('.')[2].Substring(1)))
                .Distinct()
                .ToArray();

            currentYear = availableYears.Max();
            this.solutionRunner = solutionRunner;
            this.rendererRunner = rendererRunner;
            solutionRunner.SetYear(currentYear);
            rendererRunner.SetYear(currentYear);
        }

        public void PrintStartupMessage()
        {
            Console.WriteLine($"Using solutions from year {currentYear}");
        }

        public bool CheckRequest(string[] args)
        {
            if(args.Length == 1 && args[0].ToLower() == "year")
            {
                Console.WriteLine($"Current year is {currentYear}.");

                return true;
            }

            if (args.Length >= 2 && args[0].ToLower() == "year" && int.TryParse(args[1], out var newYear))
            {
                if (!availableYears.Contains(newYear))
                    Console.WriteLine($"No solutions found for year {newYear}");
                else
                {
                    currentYear = newYear;
                    solutionRunner.SetYear(newYear);
                    rendererRunner.SetYear(newYear);

                    Console.WriteLine($"Year set to {newYear}");
                }
                return true;
            }

            return false;
        }
    }
}
