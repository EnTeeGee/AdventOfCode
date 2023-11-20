using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day15
    {
        [Solution(15, 1)]
        public long Solution1(string input)
        {
            var ingredients = Parser.ToArrayOf(input, it => new Ingredient(it));
            var each = 100 / ingredients.Length;
            var current = new Measure(ingredients.ToDictionary(it => it, it => each));
            var currentScore = current.ComputeScore();

            if (currentScore == 0)
            {
                // need to find a section with a score to start from
                current = FindMeasureWithScore(current);
                currentScore = current.ComputeScore();
            }

            while (true)
            {
                var surrounding = current.GetSurrounding().Select(it => new { item = it, score = it.ComputeScore() }).OrderByDescending(it => it.score).ToArray();

                if (!surrounding.Any())
                    return currentScore;

                if (surrounding.First().score <= currentScore)
                    return currentScore;

                current = surrounding.First().item;
                currentScore = surrounding.First().score;
            }

        }

        [Solution(15, 2)]
        public int Solution2(string input)
        {
            var ingredients = Parser.ToArrayOf(input, it => new Ingredient(it));
            var each = 100 / ingredients.Length;
            var current = new Measure(ingredients.ToDictionary(it => it, it => each));
            var currentScore = current.ComputeScore();

            if (currentScore == 0)
            {
                // need to find a section with a score to start from
                current = FindMeasureWithScore(current);
                currentScore = current.ComputeScore();
            }

            while (true)
            {
                var surrounding = current.GetSurrounding().Select(it => new { item = it, score = it.ComputeScore() }).OrderByDescending(it => it.score).ToArray();

                if (!surrounding.Any())
                    break;

                if (surrounding.First().score <= currentScore)
                    break;

                current = surrounding.First().item;
                currentScore = surrounding.First().score;
            }

            // found best score, search surrounding until a suitable calorie count is found
            var topScore = 0;
            var triedMeasures = new HashSet<Measure>();
            triedMeasures.Add(current);

            var toTry = new Queue<Measure>();
            toTry.Enqueue(current);

            while (toTry.Any())
            {
                current = toTry.Dequeue();
                if (current.Has500Calories())
                {
                    var score = current.ComputeScore();
                    if (score > topScore)
                        topScore = score;
                }

                var surrounding = current.GetSurrounding().Where(it => !triedMeasures.Contains(it)).Select(it => new { item = it, score = it.ComputeScore() }).Where(it => it.score >= topScore).ToArray();
                foreach (var item in surrounding)
                    triedMeasures.Add(item.item);

                foreach (var item in surrounding)
                    toTry.Enqueue(item.item);
            }

            return topScore;
        }

        private Measure FindMeasureWithScore(Measure start)
        {
            var triedMeasures = new HashSet<Measure>();
            triedMeasures.Add(start);

            var toTry = new Queue<Measure>();
            toTry.Enqueue(start);

            while (true)
            {
                var current = toTry.Dequeue();
                var surrounding = current.GetSurrounding().Where(it => !triedMeasures.Contains(it)).Select(it => new { item = it, score = it.ComputeScore() }).ToArray();
                foreach (var item in surrounding)
                    triedMeasures.Add(item.item);

                if (surrounding.Any(it => it.score > 0))
                    return surrounding.First(it => it.score > 0).item;

                foreach (var item in surrounding)
                    toTry.Enqueue(item.item);
            }
        }

        private class Ingredient
        {
            public string Name { get; }

            public int Capacity { get; }

            public int Durability { get; }

            public int Flavour { get; }

            public int Texture { get; }

            public int Calories { get; }

            public Ingredient(string input)
            {
                var tokens = Parser.SplitOn(input, ' ', ',', ':');

                Name = tokens[0];
                Capacity = int.Parse(tokens[2]);
                Durability = int.Parse(tokens[4]);
                Flavour = int.Parse(tokens[6]);
                Texture = int.Parse(tokens[8]);
                Calories = int.Parse(tokens[10]);
            }
        }

        private class Measure
        {
            public Dictionary<Ingredient, int> Measures { get; }

            public Measure(Dictionary<Ingredient, int> input)
            {
                Measures = input;
            }

            public Measure[] GetSurrounding()
            {
                var output = new List<Measure>();

                foreach (var item in Measures)
                {
                    var remaining = Measures.Where(it => it.Key != item.Key).ToArray();
                    foreach (var toReduce in remaining)
                    {
                        if (toReduce.Value < 1)
                            continue;

                        var remaining2 = remaining.Where(it => it.Key != toReduce.Key).ToList();
                        var increasedPair = new KeyValuePair<Ingredient, int>(item.Key, item.Value + 1);
                        var decreasedPair = new KeyValuePair<Ingredient, int>(toReduce.Key, toReduce.Value - 1);
                        var dict = new Dictionary<Ingredient, int>(remaining2.ToDictionary(it => it.Key, it => it.Value));
                        dict.Add(increasedPair.Key, increasedPair.Value);
                        dict.Add(decreasedPair.Key, decreasedPair.Value);

                        output.Add(new Measure(dict));
                    }
                }

                return output.ToArray();
            }

            public int ComputeScore()
            {
                var product = 1;

                product *= Math.Max(0, Measures.Select(it => it.Value * it.Key.Capacity).Sum());
                product *= Math.Max(0, Measures.Select(it => it.Value * it.Key.Durability).Sum());
                product *= Math.Max(0, Measures.Select(it => it.Value * it.Key.Flavour).Sum());
                product *= Math.Max(0, Measures.Select(it => it.Value * it.Key.Texture).Sum());

                return product;
            }

            public bool Has500Calories()
            {
                return Measures.Select(it => it.Key.Calories * it.Value).Sum() == 500;
            }

            public override bool Equals(object? obj)
            {
                var castObj = obj as Measure;

                if (castObj == null)
                    return false;

                foreach (var item in Measures)
                {
                    if (!castObj.Measures.ContainsKey(item.Key))
                        return false;

                    if (castObj.Measures[item.Key] != item.Value)
                        return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                var output = 0;

                foreach (var item in Measures)
                    output ^= item.Value;

                return output;
            }
        }
    }
}
