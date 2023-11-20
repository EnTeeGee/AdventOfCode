using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day10
    {
        [Solution(10, 1)]
        public int Solution1(string input)
        {
            var current = input.Trim();
            for (var i = 0; i < 40; i++)
            {
                var output = "";
                var index = 0;
                var currentChar = current[0];
                var charCount = 1;
                while (index < current.Length)
                {
                    index++;
                    if (index < current.Length && current[index] == currentChar)
                    {
                        charCount++;
                    }
                    else
                    {
                        output += $"{charCount}{currentChar}";
                        if (index < current.Length)
                            currentChar = current[index];
                        charCount = 1;
                    }
                }

                current = output;
            }

            return current.Length;
        }

        [Solution(10, 2)]
        public int Solution2(string input)
        {
            var currentQueue = new Queue<byte>();
            foreach (var item in input.Trim())
                currentQueue.Enqueue(byte.Parse(item.ToString()));
            for (var i = 0; i < 50; i++)
            {
                var output = new Queue<byte>();
                var currentByte = currentQueue.Dequeue();
                var byteCount = (byte)1;
                while (currentQueue.Any())
                {
                    var newByte = currentQueue.Dequeue();
                    if (newByte == currentByte)
                    {
                        byteCount++;
                    }
                    else
                    {
                        output.Enqueue(byteCount);
                        output.Enqueue(currentByte);
                        byteCount = 1;
                    }

                    currentByte = newByte;
                }

                output.Enqueue(byteCount);
                output.Enqueue(currentByte);

                currentQueue = output;
            }

            return currentQueue.Count;
        }
    }
}
