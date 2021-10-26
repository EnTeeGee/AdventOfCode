using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    static class Factor
    {
        public static int[] GetFactors(int value)
        {
            if (value == 1)
                return new[] { 1 };

            var output = new List<int>();
            var target = (int)Math.Sqrt(value);

            for(var i = 1; i <= target; i++)
            {
                if (value % i == 0)
                {
                    output.Add(i);
                    var inverse = value / i;
                    if(inverse != target)
                        output.Add(inverse);
                }
                    
            }

            return output.ToArray();
        }
    }
}
