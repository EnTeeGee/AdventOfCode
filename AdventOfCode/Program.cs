﻿using AdventOfCode.Core;
using AdventOfCode.Renderer;
using System;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var yearManager = new YearManager();
            var solutionRunner = new SolutionRunner(yearManager);
            var rendererRunner = new RendererRunner(yearManager);

            var runners = new IRunner[] { solutionRunner, rendererRunner, yearManager };

            foreach (var item in runners)
                item.PrintStartupMessage();

            while (true)
            {
                var values = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var result = runners.FirstOrDefault(it => it.CheckRequest(values));
                if (result == null)
                    Console.WriteLine("unexpected command");
            }
        }
    }
}
