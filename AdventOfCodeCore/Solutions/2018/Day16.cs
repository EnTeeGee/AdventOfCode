using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    class Day16
    {
        [Solution(16, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input).AsEnumerable();
            var tests = new List<OpCodeTest>();

            while (lines.First().StartsWith("Before"))
            {
                tests.Add(new OpCodeTest(lines.Take(3).ToArray()));
                lines = lines.Skip(3);
            }

            var opCodes = GetAll();

            var result = tests.Where(it => opCodes.Where(oc => AssertTest(it, oc)).Count() >= 3).Count();

            return result;
        }

        [Solution(16, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input).AsEnumerable();
            var tests = new List<OpCodeTest>();

            while (lines.First().StartsWith("Before"))
            {
                tests.Add(new OpCodeTest(lines.Take(3).ToArray()));
                lines = lines.Skip(3);
            }

            var program = lines.Select(it => it.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(d => int.Parse(d)).ToArray());
            var opCodesToTest = GetAll();
            var opCodeMapping = new Dictionary<int, IOpCode>();

            Console.WriteLine("Completed parsing");

            while (tests.Any())
            {
                //var onlyPossible = tests.Where(it => opCodesToTest.Where(oc => AssertTest(it, oc)).Count() == 1);
                var onlyPossible = tests
                    .SelectMany(it => opCodesToTest.Select(oc => new { Code = oc, Test = it }))
                    .Where(it => AssertTest(it.Test, it.Code))
                    .GroupBy(it => it.Test.Number)
                    .Where(it => it.Select(info => info.Code).Distinct().Count() == 1);

                if (!onlyPossible.Any())
                    return 0;

                foreach(var item in onlyPossible)
                {
                    var info = item.First();

                    opCodeMapping.Add(info.Test.Number, info.Code);
                    tests.RemoveAll(it => it.Number == info.Test.Number);
                    opCodesToTest = opCodesToTest.Where(it => it != info.Code).ToArray();
                }
            }

            Console.WriteLine("Completed assiging opCodes to numbers");

            var registers = new int[] { 0, 0, 0, 0 };
            foreach(var item in program)
            {
                var opCode = opCodeMapping[item[0]];
                registers = opCode.Perform(registers, item[1], item[2], item[3]);
            }

            return registers[0];
        }

        private IOpCode[] GetAll()
        {
            return new IOpCode[]
            {
                new AddR(),
                new AddI(),
                new MulR(),
                new MulI(),
                new BanR(),
                new BanI(),
                new BorR(),
                new BorI(),
                new SetR(),
                new SetI(),
                new GtIR(),
                new GtRI(),
                new GtRR(),
                new EqIR(),
                new EqRI(),
                new EqRR()
            };
        }

        private bool AssertTest(OpCodeTest test, IOpCode opCode)
        {
            var result = opCode.Perform(test.Before, test.A, test.B, test.C);

            return result.Zip(test.After, (a, b) => new { a, b }).All(it => it.a == it.b);
        }

        private class OpCodeTest
        {
            public int[] Before { get; }
            public int[] After { get; }
            public int Number { get; }
            public int A { get; }
            public int B { get; }
            public int C { get; }

            public OpCodeTest(string[] inputs)
            {
                Before = ToIntArray(inputs[0]);
                After = ToIntArray(inputs[2]);
                var command = inputs[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(it => int.Parse(it)).ToArray();
                Number = command[0];
                A = command[1];
                B = command[2];
                C = command[3];
            }

            private int[] ToIntArray(string input)
            {
                return input.Where(it => char.IsDigit(it)).Select(it => int.Parse(it.ToString())).ToArray();
            }
        }

        private interface IOpCode
        {
            int[] Perform(int[] registers, int a, int b, int c);
        }

        private class AddR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] + registers[b];

                return output;
            }
        }

        private class AddI : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] + b;

                return output;
                
            }
        }

        private class MulR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] * registers[b];

                return output;
            }
        }

        private class MulI : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] * b;

                return output;
            }
        }

        private class BanR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] & registers[b];

                return output;
            }
        }

        private class BanI : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] & b;

                return output;
            }
        }

        private class BorR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] | registers[b];

                return output;
            }
        }

        private class BorI : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] | b;

                return output;
            }
        }

        private class SetR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a];

                return output;
            }
        }

        private class SetI : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = a;

                return output;
            }
        }

        private class GtIR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = a > registers[b] ? 1 : 0;

                return output;
            }
        }

        private class GtRI : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] > b ? 1 : 0;

                return output;
            }
        }

        private class GtRR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] > registers[b] ? 1 : 0;

                return output;
            }
        }

        private class EqIR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = a == registers[b] ? 1 : 0;

                return output;
            }
        }

        private class EqRI : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] == b ? 1 : 0;

                return output;
            }
        }

        private class EqRR : IOpCode
        {
            public int[] Perform(int[] registers, int a, int b, int c)
            {
                var output = registers.ToArray();
                output[c] = registers[a] == registers[b] ? 1 : 0;

                return output;
            }
        }
    }
}
