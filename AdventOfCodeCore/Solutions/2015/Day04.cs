using AdventOfCodeCore.Core;
using System.Text;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day04
    {
        [Solution(4, 1)]
        public int Solution1(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                for (var i = 0; i < int.MaxValue; i++)
                {
                    var combined = input.Trim() + i.ToString();
                    var inputBytes = Encoding.ASCII.GetBytes(combined);
                    var hashBytes = md5.ComputeHash(inputBytes);

                    if (hashBytes.Take(2).All(it => it == 0x00) && hashBytes[2] < 0x10)
                        return i;
                }
            }

            throw new Exception("No code found");
        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                for (var i = 0; i < int.MaxValue; i++)
                {
                    var combined = input.Trim() + i.ToString();
                    var inputBytes = Encoding.ASCII.GetBytes(combined);
                    var hashBytes = md5.ComputeHash(inputBytes);

                    if (hashBytes.Take(3).All(it => it == 0x00))
                        return i;
                }
            }

            throw new Exception("No code found");
        }
    }
}
