using AdventOfCodeCore.Core;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCodeCore.Solutions._2016
{
    class Day05
    {
        [Solution(5, 1)]
        public string Solution1(string input)
        {
            var index = 0;
            var output = new List<char>();

            using(var md5 = MD5.Create())
            {
                while (output.Count < 8)
                {
                    var testString = input + index.ToString();
                    var testResult = md5.ComputeHash(Encoding.ASCII.GetBytes(testString));
                    var testHex = string.Concat(testResult.Take(3).Select(it => it.ToString("X2")).ToArray());
                    if (testHex.StartsWith("00000"))
                        output.Add(testHex[5]);

                    ++index;
                }
            }

            return new string(output.ToArray()).ToLower();
        }

        [Solution(5, 2)]
        public string Solution2(string input)
        {
            var index = 0;
            var output = new char?[8];

            using (var md5 = MD5.Create())
            {
                while (true)
                {
                    var testString = input + index.ToString();
                    var testResult = md5.ComputeHash(Encoding.ASCII.GetBytes(testString));
                    var testHex = string.Concat(testResult.Take(4).Select(it => it.ToString("X2")).ToArray());
                    if (testHex.StartsWith("00000") && char.IsDigit(testHex[5]))
                    {
                        var targetIndex = int.Parse(testHex[5].ToString());
                        if(targetIndex < output.Length && output[targetIndex] == null)
                        {
                            output[targetIndex] = testHex[6];

                            if (output.All(it => it != null))
                                break;
                        }
                            
                    }

                    ++index;
                }
            }

            return new string(output.Select(it => it!.Value).ToArray()).ToLower();
        }
    }
}
