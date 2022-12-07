using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day07
    {
        [Solution(7, 1)]
        public int Solution1(string input)
        {
            var root = ParseStructure(input);

            return root.GetDirSizes().Where(it => it <= 100000).Sum();
        }

        [Solution(7, 2)]
        public int Solution2(string input)
        {
            var root = ParseStructure(input);
            var toRemove = root.GetTotalFileSize() - 40000000;

            return root.GetDirSizes().OrderBy(it => it).First(it => it >= toRemove);
        }

        private Dir ParseStructure(string input)
        {
            var lines = Parser.ToArrayOfString(input)
                .Skip(1)
                .Select(it => Parser.SplitOnSpace(it))
                .ToArray();

            var root = new Dir("/", null);
            var currentDir = root;
            for(var i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];
                if (currentLine[0] == "$" && currentLine[1] == "cd")
                {
                    if (currentLine[2] == "..")
                        currentDir = currentDir.Parent;
                    else if (currentLine[2] == "/")
                        currentDir = root;
                    else if (currentDir.Dirs.Any(it => it.Name == currentLine[2]))
                        currentDir = currentDir.Dirs.First(it => it.Name == currentLine[2]);
                    else
                    {
                        var newDir = new Dir(currentLine[2], currentDir);
                        currentDir.Dirs.Add(newDir);
                        currentDir = newDir;
                    }
                }
                else if (currentLine[0] == "$" && currentLine[1] == "ls") { }
                else if (currentLine[0] == "dir" && !currentDir.Dirs.Any(it => it.Name == currentLine[1]))
                    currentDir.Dirs.Add(new Dir(currentLine[1], currentDir));
                else if (!currentDir.Files.Any(it => it.name == currentLine[1]))
                    currentDir.Files.Add((currentLine[1], int.Parse(currentLine[0])));
                else
                    throw new Exception("Unexpected command");
            }

            return root;
        }

        private class Dir
        {
            public string Name { get; }
            public List<(string name, int size)> Files { get; }
            public List<Dir> Dirs { get; }
            public Dir Parent { get; }

            public Dir(string name, Dir parent)
            {
                Name = name;
                Files = new List<(string, int)>();
                Dirs = new List<Dir>();
                Parent = parent;
            }

            public int[] GetDirSizes()
            {
                var subDirs = Dirs.SelectMany(it => it.GetDirSizes()).ToArray();

                return subDirs.Concat(new[] { GetTotalFileSize() }).ToArray();
            }

            public int GetTotalFileSize()
            {
                return Dirs.Sum(it => it.GetTotalFileSize()) + Files.Sum(it => it.size);
            }
        }
    }
}
