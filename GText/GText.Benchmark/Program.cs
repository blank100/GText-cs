#if !DEBUG
using BenchmarkDotNet.Running;

#endif

namespace Text.Benchmark {
	public class Program {
		public static void Main(string[] args) {
#if DEBUG
			DebugRunner.Run();
#else
			BenchmarkRunner.Run<TextReplaceBenchmark>();
			// BenchmarkRunner.Run<StringSplit2DBenchmark>();
#endif
		}
	}
}