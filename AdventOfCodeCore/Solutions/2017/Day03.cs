using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day03
    {
        [Solution(3, 1)]
        public long solution1(string input)
        {
            var target = int.Parse(input);
            var pos = Point.Origin;
            var depth = 0;
            var i = 1;

            if (i == target)
                return pos.GetTaxiCabDistanceTo(Point.Origin);

            while (true)
            {
                i++;
                depth++;
                var step = (depth * 2) - 1;
                pos = pos.MoveEast();
                if (i == target)
                    return pos.GetTaxiCabDistanceTo(Point.Origin);

                var dist = (Math.Min(i + step, target) - i);
                pos = pos.MoveNorth(-dist);
                i += dist;
                step += 1;
                if (i == target)
                    return pos.GetTaxiCabDistanceTo(Point.Origin);

                dist = (Math.Min(i + step, target) - i);
                pos = pos.MoveEast(-dist);
                i += dist;
                if (i == target)
                    return pos.GetTaxiCabDistanceTo(Point.Origin);

                dist = (Math.Min(i + step, target) - i);
                pos = pos.MoveNorth(dist);
                i += dist;
                if (i == target)
                    return pos.GetTaxiCabDistanceTo(Point.Origin);

                dist = (Math.Min(i + step, target) - i);
                pos = pos.MoveEast(dist);
                i += dist;
                if (i == target)
                    return pos.GetTaxiCabDistanceTo(Point.Origin);
            }
        }

        [Solution(3, 2)]
        public long Solution2(string input)
        {
            var target = int.Parse(input);
            var pos = Point.Origin;
            var depth = 0;
            var map = new Dictionary<Point, int>();
            map.Add(Point.Origin, 1);

            while(true)
            {
                pos = pos.MoveEast();
                var result = SumAround(pos, map);
                if (result > target)
                    return result;

                map.Add(pos, result);

                depth += 1;
                var step = (depth * 2) - 1;

                for(var i = 0; i < step; i++)
                {
                    pos = pos.MoveNorth(1);
                    result = SumAround(pos, map);
                    if (result > target)
                        return result;
                    map.Add(pos, result);
                }

                step += 1;

                for (var i = 0; i < step; i++)
                {
                    pos = pos.MoveEast(-1);
                    result = SumAround(pos, map);
                    if (result > target)
                        return result;
                    map.Add(pos, result);
                }

                for (var i = 0; i < step; i++)
                {
                    pos = pos.MoveNorth(-1);
                    result = SumAround(pos, map);
                    if (result > target)
                        return result;
                    map.Add(pos, result);
                }

                for (var i = 0; i < step; i++)
                {
                    pos = pos.MoveEast();
                    result = SumAround(pos, map);
                    if (result > target)
                        return result;
                    map.Add(pos, result);
                }
            }
        }

        private int SumAround(Point point, Dictionary<Point, int> map)
        {
            return point.GetSurrounding8()
                .Where(it => map.ContainsKey(it))
                .Select(it => map[it])
                .Sum();
        }
    }
}
