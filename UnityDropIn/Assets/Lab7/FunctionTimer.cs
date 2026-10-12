using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using UnityEngine;

public class FunctionTimer : MonoBehaviour
{
    public TextAsset numbersFile;

    private void Start()
    {
        try
        {
            MeasureFunctionExecutionTime();
        }
        catch (Exception error)
        {
            UnityEngine.Debug.LogError("Sorting benchmark failed: " + error.Message);
        }
    }

    private void MeasureFunctionExecutionTime()
    {
        if (numbersFile == null)
            throw new InvalidOperationException("Assign your Random.org text file to Numbers File.");

        int[] original = ParseNumbers(numbersFile.text);
        int[] nativeWarmup = (int[])original.Clone();
        int[] managedWarmup = (int[])original.Clone();
        int[] monoBehaviourWarmup = (int[])original.Clone();

        Lab7.NativeSorter.TestSort(nativeWarmup, nativeWarmup.Length);
        Lab7.ManagedSorter.TestSort(managedWarmup, managedWarmup.Length);
        TestSort(monoBehaviourWarmup, monoBehaviourWarmup.Length);

        int[] nativeNumbers = (int[])original.Clone();
        int[] managedNumbers = (int[])original.Clone();
        int[] monoBehaviourNumbers = (int[])original.Clone();
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        Lab7.NativeSorter.TestSort(nativeNumbers, nativeNumbers.Length);
        stopwatch.Stop();
        double nativeMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

        stopwatch.Reset();
        stopwatch.Start();
        Lab7.ManagedSorter.TestSort(managedNumbers, managedNumbers.Length);
        stopwatch.Stop();
        double managedMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

        stopwatch.Reset();
        stopwatch.Start();
        TestSort(monoBehaviourNumbers, monoBehaviourNumbers.Length);
        stopwatch.Stop();
        double monoBehaviourMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

        UnityEngine.Debug.Log("Native plugin took " + nativeMilliseconds.ToString("F4", CultureInfo.InvariantCulture) + " ms to execute (after warm-up).");
        UnityEngine.Debug.Log("Managed plugin took " + managedMilliseconds.ToString("F4", CultureInfo.InvariantCulture) + " ms to execute (after warm-up).");
        UnityEngine.Debug.Log("MonoBehaviour took " + monoBehaviourMilliseconds.ToString("F4", CultureInfo.InvariantCulture) + " ms to execute (after warm-up).");
    }

    private void TestSort(int[] values, int length)
    {
        if (values == null) throw new ArgumentNullException("values");
        if (length < 0 || length > values.Length)
            throw new ArgumentOutOfRangeException("length");
        Array.Sort(values, 0, length);
    }

    private static int[] ParseNumbers(string text)
    {
        string[] tokens = text.Split(new[] { ',', ' ', '\t', '\r', '\n', ';' },
            StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length < 10000)
            throw new FormatException("Provide at least 10,000 unique integers from Random.org.");

        int[] values = new int[tokens.Length];
        HashSet<int> unique = new HashSet<int>();
        for (int i = 0; i < tokens.Length; i++)
        {
            int value;
            if (!int.TryParse(tokens[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out value)
                || value < 1000000 || value > 1000000000)
                throw new FormatException("Invalid integer at position " + (i + 1) +
                    ". Use numbers from 1000000 to 1000000000 without headings or thousands separators.");
            if (!unique.Add(value))
                throw new FormatException("Duplicate integer: " + value + ". Use a unique integer set.");
            values[i] = value;
        }
        return values;
    }

}
