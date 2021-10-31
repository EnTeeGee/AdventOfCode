using System;
using System.Reflection;

namespace AdventOfCode.Core
{
    class SolutionMapping
    {
        public int Day { get; set; }

        public int Problem { get; set; }

        public string Name { get; set; }

        public Type ClassType { get; set; }

        public MethodInfo Method { get; set; }
    }
}
