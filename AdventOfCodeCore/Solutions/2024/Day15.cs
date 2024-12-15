using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day15
    {
        [Solution(15, 1)]
        public long Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var lines = Parser.ToArrayOfString(chunks[0]);
            var walls = new HashSet<Point>();
            var boxes = new HashSet<Point>();
            var pos = Point.Origin;

            for(var y = 0; y < lines.Length; y++)
            {
                for(var x = 0; x < lines[y].Length; x++)
                {
                    var symbol = lines[y][x];
                    if(symbol == '#')
                        walls.Add(new Point(x, y));
                    else if (symbol == 'O')
                        boxes.Add(new Point(x, y));
                    else if (symbol == '@')
                        pos = new Point(x, y);
                }
            }

            foreach(var item in chunks[1].Where(it => "^v<>".Contains(it)))
            {
                var dir = (Orientation)"^>v<".IndexOf(item);
                var target = pos.MoveOrient(dir);

                if(TryMove(target, dir, walls, boxes))
                    pos = target;
            }

            return boxes.Sum(it => (it.Y * 100) + it.X);
        }

        [Solution(15, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var lines = Parser.ToArrayOfString(chunks[0]);
            var walls = new HashSet<Point>();
            var boxes = new HashSet<Point>();
            var pairings = new Dictionary<Point, Point>();
            var pos = Point.Origin;

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    var symbol = lines[y][x];
                    var left = new Point(x * 2, y);
                    var right = new Point((x * 2) + 1, y);
                    if (symbol == '#')
                    {
                        walls.Add(left);
                        walls.Add(right);
                    }
                    else if (symbol == 'O')
                    {
                        boxes.Add(left);
                        boxes.Add(right);
                        pairings.Add(left, right);
                        pairings.Add(right, left);
                    }
                        
                    else if (symbol == '@')
                        pos = left;
                }
            }

            foreach (var item in chunks[1].Where(it => "^v<>".Contains(it)))
            {
                var dir = (Orientation)"^>v<".IndexOf(item);
                var target = pos.MoveOrient(dir);

                if(CanMove(target, dir, walls, boxes, pairings))
                {
                    TryMoveV2(target, dir, walls, boxes, pairings);
                    pos = target;
                }
            }

            return pairings.Select(it => it.Key.X < it.Value.X ? it.Key : it.Value).Distinct().Sum(it => (it.Y * 100) + it.X);
        }

        private bool TryMove(Point target, Orientation dir, HashSet<Point> walls, HashSet<Point> boxes)
        {
            if(walls.Contains(target))
                return false;

            if (boxes.Contains(target))
            {
                if (!TryMove(target.MoveOrient(dir), dir, walls, boxes))
                    return false;

                boxes.Remove(target);
                boxes.Add(target.MoveOrient(dir));

                return true;
            }

            return true;
        }

        private bool CanMove(Point target, Orientation dir, HashSet<Point> walls, HashSet<Point> boxes, Dictionary<Point, Point> pairings)
        {
            if (walls.Contains(target))
                return false;

            if (boxes.Contains(target))
            {
                var boxSquares = new[] { target, pairings[target] }.Where(it => it.MoveOrient(dir) != pairings[it]).ToArray();

                return boxSquares.All(it => CanMove(it.MoveOrient(dir), dir, walls, boxes, pairings));
            }

            return true;
        }

        private void TryMoveV2(Point target, Orientation dir, HashSet<Point> walls, HashSet<Point> boxes, Dictionary<Point, Point> pairings)
        {
            if (walls.Contains(target))
                throw new Exception("Trying to move into wall");

            if (boxes.Contains(target))
            {
                var boxSquares = new[] { target, pairings[target] }.Where(it => it.MoveOrient(dir) != pairings[it]).ToArray();
                foreach(var item in boxSquares)
                    TryMoveV2(item.MoveOrient(dir), dir, walls, boxes, pairings);

                var pair = pairings[target];
                boxes.Remove(target);
                boxes.Remove(pair);
                pairings.Remove(target);
                pairings.Remove(pair);
                target = target.MoveOrient(dir);
                pair = pair.MoveOrient(dir);
                boxes.Add(target);
                boxes.Add(pair);
                pairings.Add(target, pair);
                pairings.Add(pair, target);
            }
        }
    }
}
