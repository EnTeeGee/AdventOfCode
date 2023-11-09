using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day07
    {
        [Solution(7, 1)]
        public string Solution1(string input)
        {
            var towers = Parser.ToArrayOf(input, it => new Tower(it));
            var towersDict = towers.ToDictionary(it => it.Name, it => it);

            foreach(var item in towers)
            {
                item.Children = item.ChildNames.Select(it => towersDict[it]).ToArray();
                foreach (var child in item.Children)
                    child.Parent = item;
            }

            var node = towers.First();
            while (node.Parent != null)
                node = node.Parent;

            return node.Name;
        }

        [Solution(7, 2)]
        public int Solution2(string input)
        {
            var towers = Parser.ToArrayOf(input, it => new Tower(it));
            var towersDict = towers.ToDictionary(it => it.Name, it => it);

            foreach (var item in towers)
            {
                item.Children = item.ChildNames.Select(it => towersDict[it]).ToArray();
                foreach (var child in item.Children)
                    child.Parent = item;
            }

            var node = towers.First();
            while (node.Parent != null)
                node = node.Parent;

            while (true)
            {
                var test = node.Children.Select(it => it.GetWeight()).ToArray();
                var test2 = node.Children.Select(it => it.IsUnbalanced()).ToArray();
                var unbalancedChild = node.Children.FirstOrDefault(it => it.IsUnbalanced());
                if(unbalancedChild != null)
                {
                    node = unbalancedChild;
                    continue;
                }

                unbalancedChild = node.Children.GroupBy(it => it.GetWeight()).First(it => it.Count() == 1).First();

                var targetTotalWeight = node.Children.First(it => it != unbalancedChild).GetWeight();

                return unbalancedChild.Weight + (targetTotalWeight - unbalancedChild.GetWeight());
            }
        }

        private class Tower
        {
            public string Name { get; }
            public int Weight { get; }
            public string[] ChildNames { get; }
            public Tower[] Children { get; set; }
            public Tower? Parent { get; set; }

            public Tower(string input)
            {
                var sections = Parser.SplitOn(input, " (", ")", " -> ", ", ");
                Name = sections[0];
                Weight = int.Parse(sections[1]);
                if (sections.Length > 2)
                    ChildNames = sections[2..];
                else
                    ChildNames = Array.Empty<string>();
                Children = Array.Empty<Tower>();
            }

            public int GetWeight()
            {
                return Weight + Children.Sum(it => it.GetWeight());
            }

            public bool IsUnbalanced()
            {
                if (!Children.Any())
                    return false;

                return Children.Select(it => it.GetWeight()).Distinct().Count() > 1;
            }
        }
    }
}
