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

        public YearManager(SolutionRunner solutionRunner)
        {
            availableYears = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(it => it.FullName.StartsWith("AdventOfCode.Solutions."))
                .Select(it => Convert.ToInt32(it.FullName.Split('.')[2].Substring(1)))
                .Distinct()
                .ToArray();

            currentYear = availableYears.Max();
            this.solutionRunner = solutionRunner;
            solutionRunner.SetYear(currentYear);
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

                    Console.WriteLine($"Year set to {newYear}");
                }
                return true;
            }

            return false;
        }
    }
}
