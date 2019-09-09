using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace System.Linq.HasDuplicates.Benchmark
{
    [InProcessAttribute]
    public class DuplicatesBenchmark
    {
        private List<string> dataWithoutDuplicates;

        private List<string> dataWithDuplicates;

        [Params(100,  100000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            var fullWordList = File.ReadLines("words.txt").ToArray();
            dataWithoutDuplicates = new Span<string>(fullWordList, 0, N).ToArray().ToList();

            dataWithDuplicates = new Span<string>(fullWordList, 0, N/2).ToArray().ToList();
            dataWithDuplicates.AddRange(new Span<string>(fullWordList, 0, N / 2).ToArray().ToList());

        }

        [Benchmark]
        public bool LinqExtensionHasDuplicatesWithDuplicates() => dataWithDuplicates.HasDuplicates();

        [Benchmark]
        public bool LinqGroupByWithDuplicates() => dataWithDuplicates.GroupBy(x => x).Any(x => x.Count() > 1);

        [Benchmark]
        public bool LinqCountDistinctWithDuplicates() => dataWithDuplicates.Count == dataWithDuplicates.Distinct().Count();

        [Benchmark]
        public bool LinqExtensionHasDuplicatesWithoutDuplicates() => dataWithoutDuplicates.HasDuplicates();

        [Benchmark]
        public bool LinqGroupByWithoutDuplicates() => dataWithoutDuplicates.GroupBy(x => x).Any(x => x.Count() > 1);

        [Benchmark]
        public bool LinqCountDistinctWithoutDuplicates() => dataWithoutDuplicates.Count == dataWithDuplicates.Distinct().Count();
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DuplicatesBenchmark>();
        }
    }
}
