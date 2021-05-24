using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Bench>();
            Console.WriteLine(summary);
        }
    }

    // -Bench.cs を作成して細かいのは対応する
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [RPlotExporter]
    [MemoryDiagnoser]
    [MinColumn, MaxColumn, RankColumn]
    public partial class Bench
    { }
}
