namespace Mvc.Utility.Core.Extensions
{
    public static partial class Common
    {
        public static bool IsPrime(this int number)
        {
            var result = true;

            for (long i = 2; i < number; i++)
            {
                if (number % i != 0) continue;

                result = false;
                break;
            }

            return result;
        }

        public static bool IsOdd(this int number)
        {
            var result = true;

            for (long i = 1; i < number; i++)
            {
                if (i % 2 == 0) continue;

                result = false;
                break;
            }

            return result;
        }

        public static bool IsEven(this int number)
        {
            var result = true;

            for (long i = 0; i < number; i++)
            {
                if (i % 2 != 0) continue;

                result = false;
                break;
            }

            return result;
        }
    }
}