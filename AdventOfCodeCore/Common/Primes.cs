namespace AdventOfCodeCore.Common
{
    internal static class Primes
    {
        static List<long> primeList;

        static Primes()
        {
            primeList = new List<long> { 2, 3 };
        }

        public static bool IsPrime(long number)
        {
            var lastChecked = primeList[primeList.Count - 1];

            while (lastChecked < number)
            {
                lastChecked += 2;
                if (CaculatePrime(lastChecked))
                    primeList.Add(lastChecked);
            }

            if (primeList.BinarySearch(number) >= 0)
                return true;

            return false;
        }

        static bool CaculatePrime(long number)
        {
            if (number < 2)
                return false;

            if (number < 4)
                return true;

            if ((number % 2) == 0)
                return false;

            var root = Math.Sqrt(number);

            foreach (int item in primeList)
            {
                if (root < item)
                    return true;

                if ((number % item) == 0)
                    return false;
            }

            return true;
        }
    }
}
