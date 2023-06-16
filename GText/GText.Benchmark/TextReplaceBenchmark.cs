using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

using Gal.Core;

namespace Text.Benchmark {
	[MemoryDiagnoser]
	public class TextReplaceBenchmark {
		private const string TEMPLATE = "这是文本替换测试1,后面花括号中的内容将被替换,{0},{1}前面花括号内的内容被替换没有?{2}";
		private       string arg0     = "参数1";
		private       string arg1     = "参数2";
		private       string arg2     = "参数3";

		[Benchmark]
		public void ReplaceWithParams() {
			for (var i = 0; i < 1000; i++) {
				var t = TextReplace.Replace(TEMPLATE, arg0, arg1, arg2);
			}
		}

		[Benchmark]
		public void ReplaceWithSpan() {
			var arguments = new[] {
				arg0,
				arg1,
				arg2
			};

			for (var i = 0; i < 1000; i++) {
				var t = TextReplace.Replace(TEMPLATE, arguments);
			}
		}

		[Benchmark]
		public void ReplaceWithDict() {
			Dictionary<string, string> dict = new() {
				["0"] = arg0,
				["1"] = arg1,
				["2"] = arg2
			};

			for (var i = 0; i < 1000; i++) {
				var t = TextReplace.Replace(TEMPLATE, dict);
			}
		}

		[Benchmark(Baseline = true)]
		public void ReplaceWithJoin() {
			for (var i = 0; i < 1000; i++) {
				var t = $"这是文本替换测试1,后面花括号中的内容将被替换,{arg0},{arg1}前面花括号内的内容被替换没有?{arg2}";
			}
		}

		[Benchmark]
		public void ReplaceWithStringFormat() {
			for (var i = 0; i < 1000; i++) {
				var t = string.Format(TEMPLATE, arg0, arg1, arg2);
			}
		}
	}
}