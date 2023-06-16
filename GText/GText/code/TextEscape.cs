using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Gal.Core
{
    /// <summary>
    /// 文本转义
    /// </summary>
    /// <para>author gouanlin</para>
    public static class TextEscape
    {
        private static readonly Dictionary<char, string> m_DefaultEscapeTable = new() {
            ['\\'] = "\\\\"
            , ['\"'] = "\\\""
            , ['\n'] = "\\n"
            , ['\r'] = "\\r"
            , ['\t'] = "\\t"
            , ['\b'] = "\\b"
            , ['\f'] = "\\f"
            , ['\''] = "\\'"
            ,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string Exec(string text, IDictionary<char, string> escapeTable = default, bool forceAscii = false) {
            var forecastLength = (int)(text.Length * 1.1f);
            if (forecastLength <= 256) {
                RefWriter<char> writer = new(stackalloc char[forecastLength]);
                try {
                    Exec(text, ref writer, escapeTable, forceAscii);
                    return writer.writtenSpan.ToString();
                } finally {
                    writer.Dispose();
                }
            } else {
                RefWriter<char> writer = new(forecastLength);
                try {
                    Exec(text, ref writer, escapeTable, forceAscii);
                    return writer.writtenSpan.ToString();
                } finally {
                    writer.Dispose();
                }
            }
        }

        public static void Exec(ReadOnlySpan<char> text, ref RefWriter<char> writer, IDictionary<char, string> escapeTable = default, bool forceAscii = false) {
            escapeTable ??= m_DefaultEscapeTable;
            if (forceAscii) {
                foreach (var c in text) {
                    if (escapeTable.TryGetValue(c, out var t)) {
                        writer.Write(t);
                    } else if (c < ' ' || c > 127) {
                        writer.Write('\\', 'u');
                        writer.Write(((ushort)c).ToString("X4"));
                    } else {
                        writer.Write(c);
                    }
                }
            } else {
                foreach (var c in text) {
                    if (escapeTable.TryGetValue(c, out var t)) {
                        writer.Write(t);
                    } else if (c < ' ') {
                        writer.Write('\\', 'u');
                        writer.Write(((ushort)c).ToString("X4"));
                    } else {
                        writer.Write(c);
                    }
                }
            }
        }
    }
}