using System;
using System.Linq;

namespace AdventOfCode.Common
{
    static class Parser
    {
        public static T[] ToArrayOf<T>(string input, Func<string, T> converter)
        {
            return input
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(it => converter.Invoke(it.Trim()))
                .ToArray();
        }

        public static int[] ToArrayOfInt(string input)
        {
            return ToArrayOf(input, it => Convert.ToInt32(it));
        }

        public static string[] ToArrayOfString(string input)
        {
            return ToArrayOf(input, it => it);
        }

        public static string[] ToArrayOfGroups(string input)
        {
            return input
                .Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(it => it.Trim())
                .ToArray();
        }

        public static string[] SplitOnSpace(string input)
        {
            return input
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }

        public static string[] SplitOn(string input, params char[] splits)
        {
            return input
                .Split(splits, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }

        public static string[] SplitOn(string input, params string[] splits)
        {
            return input
                .Split(splits, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }
    }
}
