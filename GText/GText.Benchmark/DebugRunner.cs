using Gal.Core;
using Xunit;

namespace Text.Benchmark;

public class DebugRunner
{
    public static void Run() {
        var text = "1;2;;4;5;|3;5;;6;|;5;8;4;6";
        var list = StringToArray.ToInt32Array2D(text);
        Assert.NotNull(list);
        Assert.Equal(3, list.Length);
        Assert.Equal("1;2;0;4;5;0", string.Join(';', list[0]));
        Assert.Equal("3;5;0;6;0", string.Join(';', list[1]));
        Assert.Equal("0;5;8;4;6", string.Join(';', list[2]));
    }
}