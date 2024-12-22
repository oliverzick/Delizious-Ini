namespace Delizious.Ini
{
    using System.Linq;

    internal static class HashCode
    {
        public static int Calculate(params int[] hashCodes)
            => hashCodes.Aggregate(0, Add);

        private static int Add(int hashCode, int value)
        {
            unchecked
            {
                return hashCode ^ (hashCode << 5) + (hashCode >> 2) + value;
            }
        }
    }
}
