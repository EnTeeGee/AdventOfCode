using System;

namespace AdventOfCode.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    class SolutionAttribute : Attribute
    {
        public int Day { get; }
        public int Problem { get; }
        public string Name { get; }

        public SolutionAttribute(int day, int problem)
        {
            Day = day;
            Problem = problem;
        }

        public SolutionAttribute(int day, int problem, string name)
        {
            Day = day;
            Problem = problem;
            Name = name;
        }
    }
}
