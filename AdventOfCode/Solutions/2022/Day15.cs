using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day15
    {
        [Solution(15, 1)]
        public int Solution1(string input)
        {
            var sensors = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOn(it, ' ', ',', '=', ':'))
                .Select(it => new {
                    sensor = new Point(int.Parse(it[3]), int.Parse(it[5])),
                    beacon = new Point(int.Parse(it[11]), int.Parse(it[13])) })
                .ToArray();

            var coveredRange = new HashSet<int>();
            var targetedY = 2000000;
            //var targetedY = 10;


            foreach (var item in sensors)
            {
                var overlappingDist = item.sensor.GetTaxiCabDistanceTo(item.beacon) - Math.Abs(targetedY - item.sensor.Y);
                if (((overlappingDist * 2) + 1) <= 0)
                    continue;

                var left = item.sensor.X - overlappingDist;
                var points = Enumerable.Range((int)left, ((int)overlappingDist * 2) + 1);
                foreach (var point in points)
                    coveredRange.Add(point);
            }

            return coveredRange.Count - sensors.Select(it => it.beacon).Distinct().Count(it => it.Y == targetedY);
        }

        [Solution(15, 2)]
        public long Solution2(string input)
        {
            // It stands to reason that the one uncovered point will be on the edge of another region, so could try getting all points around each region
            // until I find one not in any other region

            var sensors = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOn(it, ' ', ',', '=', ':'))
                .Select(it => new {
                    sensor = new Point(int.Parse(it[3]), int.Parse(it[5])),
                    beacon = new Point(int.Parse(it[11]), int.Parse(it[13])) })
                .Select(it => new { it.sensor, it.beacon, size = it.sensor.GetTaxiCabDistanceTo(it.beacon) })
                .OrderBy(it => it.size)
                .ToArray();

            var limit = 4000000;
            //var limit = 20;

            for (var i = 0; i < sensors.Length; i++)
            {
                var item = sensors[i];
                var targets = Enumerable.Range(0, (int)item.size + 1)
                    .SelectMany(it => new[] {
                        new Point(item.sensor.X + it, item.sensor.Y - item.size - 1 + it),
                        new Point(item.sensor.X - item.size + it, item.sensor.Y + 1 + it),
                        new Point(item.sensor.X - item.size - 1 + it, item.sensor.Y - it),
                        new Point(item.sensor.X + 1 + it, item.sensor.Y + item.size - it) })
                    .Where(it => it.WithinBounds(0, limit, 0, limit))
                    .ToArray();

                for (var j = 0; j < sensors.Length; j++)
                {
                    if (j == i)
                        continue;

                    if (!targets.Any())
                        break;

                    var toCheck = sensors[j];
                    targets = targets.Where(it => toCheck.sensor.GetTaxiCabDistanceTo(it) > toCheck.size).ToArray();
                }

                if (targets.Any())
                {
                    if (targets.Length > 1)
                        throw new Exception("Found multiple valid points");

                    return (targets[0].X * limit) + targets[0].Y;
                }
            }

            throw new Exception("Unable to find uncovered point");
        }
    }
}
