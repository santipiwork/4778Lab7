using System;

namespace Lab7
{
    public static class ManagedSorter
    {
        public static void TestSort(int[] values, int length)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (length < 0 || length > values.Length)
                throw new ArgumentOutOfRangeException("length");
            Array.Sort(values, 0, length);
        }
    }
}
