using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var solutionRunner = new SolutionRunner();
            var yearManager = new YearManager(solutionRunner);

            var runners = new IRunner[] { solutionRunner, yearManager };

            while (true)
            {
                var values = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var result = runners.FirstOrDefault(it => it.CheckRequest(values));
            }
        }
    }
}
