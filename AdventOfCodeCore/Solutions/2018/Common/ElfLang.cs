namespace AdventOfCode2018.Common
{
    class ElfLang
    {
        public static Dictionary<string, Func<IOpCode>> Mapping = new Dictionary<string, Func<IOpCode>>
        {
            {"addr", () => new AddR() },
            {"addi", () => new AddI() },
            {"mulr", () => new MulR() },
            {"muli", () => new MulI() },
            {"banr", () => new BanR() },
            {"bani", () => new BanI() },
            {"borr", () => new BorR() },
            {"bori", () => new BorI() },
            {"setr", () => new SetR() },
            {"seti", () => new SetI() },
            {"gtir", () => new GtIR() },
            {"gtri", () => new GtRI() },
            {"gtrr", () => new GtRR() },
            {"eqir", () => new EqIR() },
            {"eqri", () => new EqRI() },
            {"eqrr", () => new EqRR() },
        };
    }

    interface IOpCode
    {
        long[] Perform(long[] registers, int a, int b, int c);
    }

    class AddR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] + registers[b];

            return output;
        }
    }

    class AddI : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] + b;

            return output;

        }
    }

    class MulR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] * registers[b];

            return output;
        }
    }

    class MulI : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] * b;

            return output;
        }
    }

    class BanR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] & registers[b];

            return output;
        }
    }

    class BanI : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] & b;

            return output;
        }
    }

    class BorR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] | registers[b];

            return output;
        }
    }

    class BorI : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] | (long)b;

            return output;
        }
    }

    class SetR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a];

            return output;
        }
    }

    class SetI : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = a;

            return output;
        }
    }

    class GtIR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = a > registers[b] ? 1 : 0;

            return output;
        }
    }

    class GtRI : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] > b ? 1 : 0;

            return output;
        }
    }

    class GtRR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] > registers[b] ? 1 : 0;

            return output;
        }
    }

    class EqIR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = a == registers[b] ? 1 : 0;

            return output;
        }
    }

    class EqRI : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] == b ? 1 : 0;

            return output;
        }
    }

    class EqRR : IOpCode
    {
        public long[] Perform(long[] registers, int a, int b, int c)
        {
            var output = registers.ToArray();
            output[c] = registers[a] == registers[b] ? 1 : 0;

            return output;
        }
    }
}
