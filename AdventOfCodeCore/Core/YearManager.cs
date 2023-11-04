using System.Reflection;

namespace AdventOfCodeCore.Core
{
    internal class YearManager : IRunner
    {
        private readonly int[] availableYears;
        private readonly List<Action<int>> actions;

        public int CurrentYear { get; private set; }

        public YearManager()
        {
            availableYears = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(it => it.FullName?.Contains("AdventOfCodeCore.Solutions.") == true)
                .Select(it => Convert.ToInt32(it.FullName?.Split('.')[2][1..]))
                .Distinct()
                .ToArray();

            CurrentYear = availableYears.Max();

            actions = new List<Action<int>>();
        }

        public void PrintStartupMessage()
        {
            Console.WriteLine($"Using solutions from year {CurrentYear}");
        }

        public bool CheckRequest(string[] args)
        {
            if (args.Length == 0)
                return false;

            var head = args[0].ToLower();
            if (head != "year" && head != "y")
                return false;

            args = args.Skip(1).ToArray();

            if (args.Length == 0)
            {
                Console.WriteLine($"Current year is {CurrentYear}.");

                return true;
            }

            if (args.Length >= 1 && int.TryParse(args[0], out var newYear))
            {
                if (!availableYears.Contains(newYear))
                    Console.WriteLine($"No solutions found for year {newYear}");
                else
                {
                    CurrentYear = newYear;
                    foreach (var item in actions)
                        item.Invoke(CurrentYear);

                    Console.WriteLine($"Year set to {newYear}");
                }
                return true;
            }

            return false;
        }

        public void Subscribe(Action<int> action)
        {
            actions.Add(action);
        }
    }
}
