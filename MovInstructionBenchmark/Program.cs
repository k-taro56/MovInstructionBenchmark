// MIT License
// Refer to LICENSE.txt for more information.

// https://zenn.dev/k_taro56/articles/csharp-return-value-declare-first

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Runtime.InteropServices;

namespace MovInstructionBenchmark;

internal static class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmark>(null!, args);
    }

    public static int SumA(int[] array)
    {
        int i = 0;
        int sum = 0;

        for (; i < array.Length; i++)
        {
            sum += array[i];
        }

        return sum;
    }

    public static int SumB(int[] array)
    {
        int sum = 0;
        int i = 0;

        for (; i < array.Length; i++)
        {
            sum += array[i];
        }

        return sum;
    }
}

[DisassemblyDiagnoser(printSource: true)]
public class Benchmark
{
    private int[] array = null!;

    [Params(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20)]
    public int Length;

    [GlobalSetup]
    public void Setup()
    {
        array = new int[Length];
        new Random(42).NextBytes(MemoryMarshal.AsBytes(array.AsSpan()));
    }

    [Benchmark(Baseline = true)]
    public int SumA()
    {
        return Program.SumA(array);
    }

    [Benchmark]
    public int SumB()
    {
        return Program.SumB(array);
    }
}
