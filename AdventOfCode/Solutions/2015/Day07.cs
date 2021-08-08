using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2015
{
    class Day07
    {
        [Solution(7, 1)]
        public int Solution1(string input)
        {
            var gates = Parser.ToArrayOf(input, it => new Gate(it));
            var unmatchedGates = gates.Where(it => it.Value == null).ToList();
            var gateDict = gates.ToDictionary(it => it.Id, it => it);

            foreach(var item in gates)
                item.ParentGates = item.ParentGates.Concat(item.ParentGateIds.Select(it => gateDict[it])).ToList();

            while (unmatchedGates.Any())
            {
                var toRemove = new List<Gate>();

                foreach(var item in unmatchedGates)
                {
                    var result = item.TryRunning();
                    if(result)
                        toRemove.Add(item);
                }

                unmatchedGates.RemoveAll(it => toRemove.Contains(it));
            }

            return gateDict["a"].Value.Value;
        }
        
        [Solution(7, 2)]
        public int Solution2(string input)
        {
            var gates = Parser.ToArrayOf(input, it => new Gate(it));
            var gateDict = gates.ToDictionary(it => it.Id, it => it);
            gateDict["b"].Value = 3176;
            var unmatchedGates = gates.Where(it => it.Value == null).ToList();

            foreach (var item in gates)
                item.ParentGates = item.ParentGates.Concat(item.ParentGateIds.Select(it => gateDict[it])).ToList();

            while (unmatchedGates.Any())
            {
                var toRemove = new List<Gate>();

                foreach (var item in unmatchedGates)
                {
                    var result = item.TryRunning();
                    if (result)
                        toRemove.Add(item);
                }

                unmatchedGates.RemoveAll(it => toRemove.Contains(it));
            }

            //var firstRun = gateDict["a"].Value.Value;

            //foreach(var item in gates)
            //{
            //    if (item.GateType != GateType.Input)
            //        item.Value = null;
            //}

            //gateDict["b"].Value = firstRun;
            //unmatchedGates = gates.Where(it => it.Value == null).ToList();

            //while (unmatchedGates.Any())
            //{
            //    var toRemove = new List<Gate>();

            //    foreach (var item in unmatchedGates)
            //    {
            //        var result = item.TryRunning();
            //        if (result)
            //            toRemove.Add(item);
            //    }

            //    unmatchedGates.RemoveAll(it => toRemove.Contains(it));
            //}

            return gateDict["a"].Value.Value;
        }

        private class Gate
        {
            public Gate(string input)
            {
                var tokens = Parser.SplitOnSpace(input);
                Id = tokens.Last();
                ParentGates = new List<Gate>();
                OriginalText = input;

                if(tokens[1] == "AND")
                {
                    GateType = GateType.And;
                    ParentGateIds = new[] { tokens[0], tokens[2] };
                }
                else if(tokens[1] == "OR")
                {
                    GateType = GateType.Or;
                    ParentGateIds = new[] { tokens[0], tokens[2] };
                }
                else if (tokens[0] == "NOT")
                {
                    GateType = GateType.Not;
                    ParentGateIds = new[] { tokens[1] };
                }
                else if (tokens[1] == "LSHIFT")
                {
                    GateType = GateType.LShift;
                    ParentGateIds = new[] { tokens[0] };
                    AltValue = ushort.Parse(tokens[2]);
                }
                else if (tokens[1] == "RSHIFT")
                {
                    GateType = GateType.RShift;
                    ParentGateIds = new[] { tokens[0] };
                    AltValue = ushort.Parse(tokens[2]);
                }
                else
                {
                    GateType = GateType.Input;
                    ParentGateIds = new string[0];
                    if (ushort.TryParse(tokens[0], out _))
                        Value = ushort.Parse(tokens[0]);
                    else
                        ParentGateIds = new[] { tokens[0] };
                }

                CleanUpIntIds();
            }

            public Gate(ushort value)
            {
                GateType = GateType.Input;
                Value = value;
            }

            public string Id { get; }

            public ushort? Value { get; set; }

            public string[] ParentGateIds { get; private set; }

            public List<Gate> ParentGates { get; set; }

            public ushort AltValue { get; }

            public GateType GateType { get; }

            private string OriginalText { get; }

            public bool TryRunning()
            {
                if (ParentGates.Any(it => it.Value == null))
                    return false;

                if (Value != null)
                    return true;

                if (GateType == GateType.And)
                    Value = (ushort)(ParentGates[0].Value & ParentGates[1].Value);
                else if (GateType == GateType.Or)
                    Value = (ushort)(ParentGates[0].Value | ParentGates[1].Value);
                else if (GateType == GateType.Not)
                    Value = (ushort)~ParentGates[0].Value;
                else if (GateType == GateType.LShift)
                    Value = (ushort)(ParentGates[0].Value << AltValue);
                else if (GateType == GateType.RShift)
                    Value = (ushort)(ParentGates[0].Value >> AltValue);
                else if (GateType == GateType.Input)
                    Value = ParentGates[0].Value.Value;
                else
                    throw new Exception("Unexpected gate type");

                return true;
            }

            private void CleanUpIntIds()
            {
                var matching = ParentGateIds.Where(it => ushort.TryParse(it, out _));

                foreach(var item in matching)
                {
                    ParentGateIds = ParentGateIds.Where(it => it != item).ToArray();
                    ParentGates.Add(new Gate(ushort.Parse(item)));
                }
            }
        }

        private enum GateType { Input, And, Or, Not, LShift, RShift }
    }
}
