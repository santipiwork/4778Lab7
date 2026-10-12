using System;
using System.Runtime.InteropServices;

namespace Lab7
{
    public static class NativeSorter
    {
        [DllImport("NativeSorter", EntryPoint = "TestSort", ExactSpelling = true,
            CallingConvention = CallingConvention.Cdecl)]
        private static extern void SortNative([In, Out] int[] values, int length);
        public static void TestSort(int[] values, int length)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (length < 0 || length > values.Length)
                throw new ArgumentOutOfRangeException("length");
            if (length <= 1) return;
            SortNative(values, length);
        }
    }
}
