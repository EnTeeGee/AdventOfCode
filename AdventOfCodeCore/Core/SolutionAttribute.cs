namespace AdventOfCodeCore.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class SolutionAttribute : Attribute
    {
        public int Day { get; }
        public int Problem { get; }
        public string Name { get; }

        public SolutionAttribute(int day, int problem)
        {
            Day = day;
            Problem = problem;
            Name = string.Empty;
        }

        public SolutionAttribute(int day, int problem, string name)
        {
            Day = day;
            Problem = problem;
            Name = name;
        }
    }
}
