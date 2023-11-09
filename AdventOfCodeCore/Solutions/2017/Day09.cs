using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day09
    {
        [Solution(9, 1)]
        public int Solution1(string input)
        {
            var root = new Group(input);

            return root.GetScore(1);
        }

        [Solution(9, 2)]
        public int Solution2(string input)
        {
            var root = new Group(input);

            return root.GetTotalGarbage();
        }

        private class Group
        {
            public Group[] SubGroups { get; }
            public string Remaining { get; }

            private int groupGarbage = 0;

            public Group(string input)
            {
                var index = 1;
                var inGarbage = false;
                var newGroups = new List<Group>();
                while (true)
                {
                    var current = input[index];
                    if (current == '!')
                        index += 2;
                    else if (inGarbage)
                    {
                        index++;
                        if (current == '>')
                            inGarbage = false;
                        else
                            groupGarbage++;
                    }
                    else if (current == '<')
                    {
                        inGarbage = true;
                        index++;
                    }
                    else if (current == '{')
                    {
                        var addedGroup = new Group(input.Substring(index));
                        input = addedGroup.Remaining;
                        index = 0;
                        newGroups.Add(addedGroup);
                    }
                    else if (current == '}')
                    {
                        SubGroups = newGroups.ToArray();
                        Remaining = input.Substring(index + 1);

                        return;
                    }
                    else
                        index++;
                }
            }

            public int GetScore(int level)
            {
                return level + SubGroups.Sum(it => it.GetScore(level + 1));
            }

            public int GetTotalGarbage()
            {
                return groupGarbage + SubGroups.Sum(it => it.GetTotalGarbage());
            }
        }
    }
}
