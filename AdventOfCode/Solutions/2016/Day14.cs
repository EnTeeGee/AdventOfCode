using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions._2016
{
    class Day14
    {
        private Dictionary<string, string> seenMappings;

        public Day14()
        {
            seenMappings = new Dictionary<string, string>();
        }

        [Solution(14, 1)]
        public int Solution1(string input)
        {
            return Run(input, GetHash);
        }

        [Solution(14, 2)]
        public int Solution2(string input)
        {
            return Run(input, GetStretchedHash);
        }

        private int Run(string input, Func<MD5, int, string, string> hashFunc)
        {
            var quintIndexes = new Dictionary<char, List<int>>();
            for (var i = 0; i < 16; i++)
            {
                var symbol = i.ToString("x")[0];
                quintIndexes.Add(symbol, new List<int>());
            }

            using (MD5 md5 = MD5.Create())
            {
                for (var i = 0; i < 1000; i++)
                {
                    var items = QuintItems(hashFunc(md5, i, input));
                    foreach (var item in items)
                        quintIndexes[item].Add(i);
                }

                var index = 0;
                var totalMatches = 0;

                while (true)
                {
                    var quintItems = QuintItems(hashFunc(md5, index + 1000, input));
                    foreach (var item in quintItems)
                        quintIndexes[item].Add(index + 1000);

                    var tripleItem = TripleItems(hashFunc(md5, index, input));
                    if (tripleItem != null)
                    {
                        if (quintIndexes[tripleItem.Value].Any(it => it > index && it < index + 1000))
                            totalMatches += 1;
                    }

                    if (totalMatches == 64)
                        return index;

                    index += 1;
                }
            }
        }

        private string GetHash(MD5 md5, int index, string key)
        {
            var combined = key + index.ToString();
            var inputBytes = Encoding.ASCII.GetBytes(combined);
            var hashBytes = md5.ComputeHash(inputBytes);
            return string.Join(string.Empty, hashBytes.Select(it => it.ToString("x2")));
        }

        private string GetStretchedHash(MD5 md5, int index, string key)
        {
            var combined = key + index.ToString();
            if (seenMappings.ContainsKey(combined))
                return seenMappings[combined];

            var inputBytes = Encoding.ASCII.GetBytes(combined);
            var hashStr = string.Empty;
            for(var i = 0; i < 2017; i++)
            {
                inputBytes = md5.ComputeHash(inputBytes);
                hashStr = string.Join(string.Empty, inputBytes.Select(it => it.ToString("x2")));
                inputBytes = Encoding.ASCII.GetBytes(hashStr);
            }
            seenMappings[combined] = hashStr;
            return hashStr;
        }

        private char? TripleItems(string input)
        {
            return input.Zip(input.Skip(1), (a, b) => a == b ? a : (char?)null)
                .Zip(input.Skip(2), (a, b) => a == b ? a : null)
                .Where(it => it != null)
                .FirstOrDefault();
        }

        private char[] QuintItems(string input)
        {
            return input.Zip(input.Skip(1), (a, b) => a == b ? a : '_')
                .Zip(input.Skip(2), (a, b) => a == b ? a : '_')
                .Zip(input.Skip(3), (a, b) => a == b ? a : '_')
                .Zip(input.Skip(4), (a, b) => a == b ? a : '_')
                .Where(it => it != '_')
                .Distinct()
                .ToArray();
        }
    }
}
