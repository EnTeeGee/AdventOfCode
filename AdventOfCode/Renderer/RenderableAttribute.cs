using System;

namespace AdventOfCode.Renderer
{
    [AttributeUsage(AttributeTargets.Method)]
    class RenderableAttribute : Attribute
    {
        public int Day { get; }
        public int Problem { get; }

        public RenderableAttribute(int day, int problem)
        {
            Day = day;
            Problem = problem;
        }
    }
}
