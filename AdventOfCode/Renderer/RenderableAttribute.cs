using System;

namespace AdventOfCode.Renderer
{
    [AttributeUsage(AttributeTargets.Method)]
    class RenderableAttribute : Attribute
    {
        public int Day { get; }
        public int? Version { get; }

        public RenderableAttribute(int day)
        {
            Day = day;
        }

        public RenderableAttribute(int day, int version)
        {
            Day = day;
            Version = version;
        }
    }
}
