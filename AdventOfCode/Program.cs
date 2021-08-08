using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
