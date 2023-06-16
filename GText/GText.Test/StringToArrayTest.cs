using System.Collections.Generic;
using Gal.Core;
using Xunit;

namespace Text.Test
{
    public static class StringToArrayTest
    {
        [Fact]
        public static void TestTo1DIntArray1() {
            var text = "1;2;;4;5";
            var list = StringToArray.ToInt32Array(text);
            Assert.NotNull(list);
            Assert.Equal(5, list.Length);
            Assert.Equal(1, list[0]);
            Assert.Equal(2, list[1]);
            Assert.Equal(0, list[2]);
            Assert.Equal(4, list[3]);
            Assert.Equal(5, list[4]);
        }

        [Fact]
        public static void TestTo1DIntArray2() {
            var text = "1;2;;4;5;";
            var list = StringToArray.ToInt32Array(text);
            Assert.NotNull(list);
            Assert.Equal(6, list.Length);
            Assert.Equal(1, list[0]);
            Assert.Equal(2, list[1]);
            Assert.Equal(0, list[2]);
            Assert.Equal(4, list[3]);
            Assert.Equal(5, list[4]);
            Assert.Equal(0, list[5]);
        }

        [Fact]
        public static void TestTo1DIntArray3() {
            var text = ";1;2;;4;5";
            var list = StringToArray.ToInt32Array(text);
            Assert.NotNull(list);
            Assert.Equal(6, list.Length);
            Assert.Equal(0, list[0]);
            Assert.Equal(1, list[1]);
            Assert.Equal(2, list[2]);
            Assert.Equal(0, list[3]);
            Assert.Equal(4, list[4]);
            Assert.Equal(5, list[5]);
        }

        [Fact]
        public static void TestTo2DIntArray1() {
            var text = "1;2;;4;5;|3;5;;6;|;5;8;4;6";
            var list = StringToArray.ToInt32Array2D(text);
            Assert.NotNull(list);
            Assert.Equal(3, list.Length);
            Assert.Equal("1;2;0;4;5;0", string.Join(';', list[0]));
            Assert.Equal("3;5;0;6;0", string.Join(';', list[1]));
            Assert.Equal("0;5;8;4;6", string.Join(';', list[2]));
        }

        [Fact]
        public static void TestTo2DIntArray2() {
            var text = "1;2;;4;5;|3;5;;6;|;5;8;4;6|";
            var list = StringToArray.ToInt32Array2D(text);
            Assert.NotNull(list);
            Assert.Equal(4, list.Length);
            Assert.Equal("1;2;0;4;5;0", string.Join(';', list[0]));
            Assert.Equal("3;5;0;6;0", string.Join(';', list[1]));
            Assert.Equal("0;5;8;4;6", string.Join(';', list[2]));
            Assert.Equal("", string.Join(';', list[3]));
        }

        [Fact]
        public static void TestTo2DIntArray3() {
            var text = "|1;2;;4;5;|3;5;;6;|;5;8;4;6";
            var list = StringToArray.ToInt32Array2D(text);
            Assert.NotNull(list);
            Assert.Equal(4, list.Length);
            Assert.Equal("", string.Join(';', list[0]));
            Assert.Equal("1;2;0;4;5;0", string.Join(';', list[1]));
            Assert.Equal("3;5;0;6;0", string.Join(';', list[2]));
            Assert.Equal("0;5;8;4;6", string.Join(';', list[3]));
        }
    }
}