using System.Collections.Generic;
using Gal.Core;
using Xunit;

namespace Text.Test
{
    public static class TextReplaceTest
    {
        [Fact]
        public static void Test1() {
            const string template = "这是文本替换测试1,后面花括号中的内容将被替换,{content1},{con}前面花括号内的内容被替换没有?01234";
            Dictionary<string, string> dict = new() { ["content1"] = "[这是替换了的内容]" };

            Assert.Equal("这是文本替换测试1,后面花括号中的内容将被替换,[这是替换了的内容],{con}前面花括号内的内容被替换没有?01234",
                TextReplace.Replace(template, dict, "{", "}"));
        }

        [Fact]
        public static void Test2() {
            const string template = "这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{content1}},{con}前面花括号内的内容被替换没有?{01234";
            Dictionary<string, string> dict = new() { ["content1"] = "[这是替换了的内容]" };

            Assert.Equal("这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,[这是替换了的内容]},{con}前面花括号内的内容被替换没有?{01234",
                TextReplace.Replace(template, dict));
        }

        [Fact]
        public static void Test3() {
            const string template = "这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{{{{{{{{}{content1}},{con}前面花括号内的内容被替换没有?{01234";
            Dictionary<string, string> dict = new() { ["content1"] = "[这是替换了的内容]" };

            Assert.Equal("这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{{{{{{{{}[这是替换了的内容]},{con}前面花括号内的内容被替换没有?{01234",
                TextReplace.Replace(template, dict));
        }

        [Fact]
        public static void Test4() {
            const string template = "这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{{{{{{{{}{content1}},{con}前面花括号内的{content1}内容被替换没有?{01234";
            Dictionary<string, string> dict = new() { ["content1"] = "[这是替换了的内容]" };

            Assert.Equal("这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{{{{{{{{}[这是替换了的内容]},{con}前面花括号内的[这是替换了的内容]内容被替换没有?{01234",
                TextReplace.Replace(template, dict));
        }

        [Fact]
        public static void Test5() {
            const string template = "这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{{{{{{{{}{content1}},{con}前面花括号内的{content1}内容被替换没有?{01234{content1";
            Dictionary<string, string> dict = new() { ["content1"] = "[这是替换了的内容]" };

            Assert.Equal("这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{{{{{{{{}[这是替换了的内容]},{con}前面花括号内的[这是替换了的内容]内容被替换没有?{01234{content1",
                TextReplace.Replace(template, dict));
        }

        [Fact]
        public static void Test6() {
            const string template = "这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{{{{{{{{}{content1}},{con}前面花括号内的{content1}内容被替换没有?{01234{content1}}}}}}";
            Dictionary<string, string> dict = new() { ["content1"] = "[这是替换了的内容]" };

            Assert.Equal("这是文本替换测{}fasf试1,后面花括号中的{内容将被替换,{{{{{{{{}[这是替换了的内容]},{con}前面花括号内的[这是替换了的内容]内容被替换没有?{01234[这是替换了的内容]}}}}}",
                TextReplace.Replace(template, dict));
        }

        [Fact]
        public static void Test7() {
            const string template = "这是文本替换测{}fasf试1,后面花{以后}中的{内容将被替换,{{{{{{{{}{content1}},{con}前面花括号内的{content1}内容被替换没有?{01234{content1}}}}}}";
            Dictionary<string, string> dict = new() {
                ["content1"] = "[这是替换了的内容1]"
                , ["con"] = "[this is replace]"
                , ["con00"] = "[this is replace]"
                , ["以后"] = "未来"
                ,
            };

            Assert.Equal("这是文本替换测{}fasf试1,后面花未来中的{内容将被替换,{{{{{{{{}[这是替换了的内容1]},[this is replace]前面花括号内的[这是替换了的内容1]内容被替换没有?{01234[这是替换了的内容1]}}}}}",
                TextReplace.Replace(template, dict));
        }
    }
}