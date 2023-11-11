namespace AdventOfCodeCore.Solutions._2017.Common
{
    internal class KnotHash
    {
        byte[] values;

        public KnotHash(string input)
        {
            values = Enumerable.Range(0, 256).Select(it => (byte)it).ToArray();
            var lengths = input.Select(it => (byte)it).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray();
            var currentPos = 0;
            var skipSize = 0;

            for (var i = 0; i < 64; i++)
            {
                foreach(var length in lengths)
                {
                    var section = values
                        .Skip(currentPos)
                        .Take(length)
                        .Concat(values.Take(currentPos - values.Length + length))
                        .Reverse()
                        .ToArray();
                    for (var j = 0; j < section.Length; j++)
                        values[(currentPos + j) % values.Length] = section[j];

                    currentPos = (currentPos + skipSize + length) % values.Length;
                    skipSize++;
                }
            }
        }

        public string AsHexidecimal()
        {
            return string.Join("", Enumerable.Range(0, 16).Select(it => values.Skip(it * 16).Take(16).Aggregate((acc, x) => (byte)(acc ^ x)).ToString("x2")));
        }

        public string AsBinary()
        {
            return string.Join(
                "",
                Enumerable.Range(0, 16).Select(it => Convert.ToString(values.Skip(it * 16).Take(16).Aggregate((acc, x) => (byte)(acc ^ x)), 2).PadLeft(8, '0')));
        }
    }
}
