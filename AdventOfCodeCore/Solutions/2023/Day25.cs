using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day25
    {
        [Solution(25, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var nodes = new Dictionary<string, Node>();

            foreach(var item in lines)
            {
                var chunks = Parser.SplitOn(item, ' ', ':');
                if (!nodes.ContainsKey(chunks[0]))
                    nodes.Add(chunks[0], new Node(chunks[0]));

                foreach (var join in chunks.Skip(1))
                {
                    if(!nodes.ContainsKey(join))
                        nodes.Add(join, new Node(join));
                    nodes[chunks[0]].Joins.Add(join);
                    nodes[join].Joins.Add(chunks[0]);
                }
            }

            var result = SearchFromSplitGroup(nodes);

            return result.Length * (nodes.Count - result.Length);
        }

        private class Node
        {
            public string Key { get; }
            public List<string> Joins { get; }

            public Node(string key)
            {
                Key = key;
                Joins = new List<string>();
            }
        }

        private string[] SearchFromSplitGroup(Dictionary<string, Node> nodes)
        {
            var nodeList = nodes.Values.ToArray();
            var source = nodeList.Take(nodeList.Length / 2).ToArray();
            var sourceHash = source.Select(it => it.Key).ToHashSet();
            var dest = nodeList.Skip(source.Length).ToArray();
            var destHash = dest.Select(it => it.Key).ToHashSet();
            var nothingPrevious = false;

            var overlap = source.SelectMany(it => it.Joins).Distinct().Where(it => destHash.Contains(it)).ToArray();

            while(overlap.Length > 3)
            {
                var option = source.FirstOrDefault(it => it.Joins.Count(it2 => sourceHash.Contains(it2)) <= it.Joins.Count(it2 => destHash.Contains(it2)));
                if(option == null)
                {
                    if (nothingPrevious)
                        throw new Exception("Unable to move anything either way");

                    var temp1 = source;
                    source = dest;
                    dest = temp1;
                    var tempHash1 = sourceHash;
                    sourceHash = destHash;
                    destHash = tempHash1;
                    nothingPrevious = true;
                    continue;
                }

                nothingPrevious = false;

                source = source.Where(it => it != option).ToArray();
                dest = dest.Append(option).ToArray();
                sourceHash.Remove(option.Key);
                destHash.Add(option.Key);

                var temp2 = source;
                source = dest;
                dest = temp2;
                var tempHash2 = sourceHash;
                sourceHash = destHash;
                destHash = tempHash2;

                overlap = source.SelectMany(it => it.Joins).Distinct().Where(it => destHash.Contains(it)).ToArray();
            }

            return sourceHash.ToArray();
        }
    }
}
