using AdventOfCodeCore.Common;

namespace AdventOfCodeCore.Core
{
    internal class FilterRate : IRunner
    {
        public bool CheckRequest(string[] args)
        {
            if (args.Length == 0 || args[0].ToLower() != "filtered")
                return false;

            Compute();

            return true;
        }

        private void Compute()
        {
            var input = Clipboard.Read();
            if (input == null)
                return;

            var lines = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it).Take(3).ToArray()).Reverse().Where(it => it[1] != "0").ToArray();
            var results = lines.Zip(lines.Skip(1), (a, b) => ComputeItem(a[1], b[1])).ToArray();

            var average = results.Sum() / results.Length;

            var startValue = int.Parse(lines[0][1]);
            var test = startValue * Math.Pow(average, results.Length);
            var test2 = startValue * Math.Pow(average, 24);

            var message = $"So far about {(1 - average).ToString("P0")} of people are filtered each day.{Environment.NewLine}" +
                $"If it continues at this rate, approximately {(int)test2} people will manage to get all 50 stars and save Christmas.";

            Console.WriteLine(message);
            Clipboard.Write(message);
        }
        
        private double ComputeItem(string beforeStr, string afterStr)
        {
            var before = int.Parse(beforeStr);
            var after = int.Parse(afterStr);
            return 1 - ((before - after) / (double)before);
        }

        public void PrintStartupMessage() { }
    }
}
