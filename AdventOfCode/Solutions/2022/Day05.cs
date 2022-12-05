using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day05
    {
        [Solution(5, 1)]
        public string Solution1(string input)
        {
            var (columns, moves) = Parse(input);

            foreach (var item in moves)
            {
                for(var i = 0; i < item[0]; i++)
                {
                    var crate = columns[item[1] - 1].Pop();
                    columns[item[2] - 1].Push(crate);
                }
            }

            return new string(columns.Select(it => it.Pop()).ToArray());
        }

        [Solution(5, 2)]
        public string Solution2(string input)
        {
            var (columns, moves) = Parse(input);

            foreach (var item in moves)
            {
                var crane = new Stack<char>();
                for (var i = 0; i < item[0]; i++)
                    crane.Push(columns[item[1] - 1].Pop());
                for (var i = 0; i < item[0]; i++)
                    columns[item[2] - 1].Push(crane.Pop());
            }

            return new string(columns.Select(it => it.Pop()).ToArray());
        }

        private (Stack<char>[] columns, int[][]moves) Parse(string input)
        {
            var chunks = input.Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var columnLines = chunks[0].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var columns = Enumerable.Range(1, columnLines.Max(it => it.Length))
                .Where(it => (it - 1) % 4 == 0)
                .Select(it => columnLines.Take(columnLines.Length - 1).Select(c => c[it]).Where(c => c != ' ').ToArray())
                .Select(it => new Stack<char>(it.Reverse()))
                .ToArray();

            var moves = Parser.ToArrayOfString(chunks[1])
                .Select(it => Parser.SplitOnSpace(it).ToArray())
                .Select(it => new[] { it[1], it[3], it[5] }.Select(m => int.Parse(m)).ToArray())
                .ToArray();

            return (columns, moves);
        }
    }
}
