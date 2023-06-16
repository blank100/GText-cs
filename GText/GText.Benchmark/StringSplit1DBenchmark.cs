using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Gal.Core;

namespace Text.Benchmark
{
    [MemoryDiagnoser]
    public class StringSplit1DBenchmark
    {
        public static string text = "2002;1500;2001;1500;1012;1500;1011;1500;1002;1500;2002;2500;2001;2500;1012;2500;1011;;2500;1002;2500;2002;3500;2001;3500;1012;3500;1011;3500;1002;3500;";

        [Benchmark(Baseline = true)]
        public void SystemSplit1D() {
            var array = text.Split(';');
            var list = new int[array.Length];
            for (var i = 0; i < array.Length; i++) {
                var e = array[i];
                list[i] = e.Length > 0 ? int.Parse(e) : 0;
            }
        }

        [Benchmark()]
        public void StringSplit1D() {
            var list = StringToArray.ToInt32Array(text);
        }
    }
}