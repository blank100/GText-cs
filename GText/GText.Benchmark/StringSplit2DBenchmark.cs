using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Gal.Core;

namespace Text.Benchmark
{
    [MemoryDiagnoser]
    public class StringSplit2DBenchmark
    {
        public static string text = "2002;1500;2001;1500;1012;1500;1011;1500;1002;1500|2002;2500;2001;2500;1012;2500;1011;2500;1002;2500|2002;3500;2001;3500;1012;3500;1011;3500;1002;3500|2002;1500;2001;1500;1012;1500;1011;1500;1002;1500|2002;2500;2001;2500;1012;2500;1011;2500;1002;2500|2002;3500;2001;3500;1012;3500;1011;3500;1002;3500;|2002;1500;2001;1500;1012;1500;1011;1500;1002;1500|2002;2500;2001;2500;1012;2500;1011;2500;1002;2500|2002;3500;2001;3500;1012;3500;1011;3500;1002;3500";

        [Benchmark(Baseline = true)]
        public void SystemSplit2D() {
            var array = text.Split('|');
            var list = new int[array.Length][];
            for (var i = 0; i < array.Length; i++) {
                var e = array[i];
                var array2 = e.Split(';');
                var list1 = new int[array2.Length];
                for (var j = 0; j < array2.Length; j++) {
                    var s = array2[j];
                    list1[j] = s.Length > 0 ? int.Parse(s) : default;
                }
                list[i] = list1;
            }
        }

        [Benchmark()]
        public void StringSplit2D() {
            var list = StringToArray.ToInt32Array2D(text);
        }
    }
}