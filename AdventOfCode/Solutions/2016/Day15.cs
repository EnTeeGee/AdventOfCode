using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2016
{
    class Day15
    {
        [Solution(15, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it));
            var size = int.Parse(lines[0][3]);
            var fallAt = (size - int.Parse(lines[0][11].TrimEnd('.')) - 1) % size;
            // for 5
            // position 4, fall at 0
            // position 3, fall at 1
            // position 2, fall at 2


            foreach (var item in lines.Skip(1))
            {
                var index = int.Parse(item[1].TrimStart('#'));
                var itemSize = int.Parse(item[3]);
                var itemFallAt = itemSize - int.Parse(item[11].TrimEnd('.'));

                var newFallAt = fallAt;
                while (true)
                {
                    // for second index, needs to be at itemSize - index
                    // check if itemSize - index == target
                    // if they are equal, valid
                    // otherwise increase newFallAt by size
                    var target = newFallAt % itemSize;
                    if((itemSize - fallAt - index + itemSize) % itemSize == target)
                        break;

                    newFallAt += size;
                }

                fallAt = newFallAt;
                size *= itemSize;

            }

            return fallAt;
        }
    }
}
