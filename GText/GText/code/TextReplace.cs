using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Gal.Core
{
    /// <summary>
    /// </summary>
    /// <para>author gouanlin</para>
    public static class TextReplace
    {
        private static readonly string[] m_Arguments1 = new string[1];
        private static readonly string[] m_Arguments2 = new string[2];
        private static readonly string[] m_Arguments3 = new string[3];
        private static readonly string[] m_Arguments4 = new string[4];

        /// <summary>
        /// 非多线程安全
        /// </summary>
        /// <param name="text"></param>
        /// <param name="argument0"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Replace(ReadOnlySpan<char> text, string argument0) {
            m_Arguments1[0] = argument0;
            return Replace(text, m_Arguments1, '{', '}');
        }

        /// <summary>
        /// 非多线程安全
        /// </summary>
        /// <param name="text"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Replace(ReadOnlySpan<char> text, string argument0, string argument1) {
            m_Arguments2[0] = argument0;
            m_Arguments2[1] = argument1;
            return Replace(text, m_Arguments2, '{', '}');
        }

        /// <summary>
        /// 非多线程安全
        /// </summary>
        /// <param name="text"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Replace(ReadOnlySpan<char> text, string argument0, string argument1, string argument2) {
            m_Arguments3[0] = argument0;
            m_Arguments3[1] = argument1;
            m_Arguments3[2] = argument2;
            return Replace(text, m_Arguments3, '{', '}');
        }

        /// <summary>
        /// 非多线程安全
        /// </summary>
        /// <param name="text"></param>
        /// <param name="argument0"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        /// <param name="argument3"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Replace(ReadOnlySpan<char> text, string argument0, string argument1, string argument2, string argument3) {
            m_Arguments4[0] = argument0;
            m_Arguments4[1] = argument1;
            m_Arguments4[2] = argument2;
            m_Arguments4[3] = argument3;
            return Replace(text, m_Arguments4, '{', '}');
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static string Replace(ReadOnlySpan<char> text, params string[] arguments) => Replace(text, arguments, '{', '}');

        /// <summary>
        /// 同 string.Format
        /// </summary>
        /// <param name="text"></param>
        /// <param name="arguments"></param>
        /// <param name="prefixDelimiter"></param>
        /// <param name="suffixDelimiter"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string Replace(ReadOnlySpan<char> text, ReadOnlySpan<string> arguments, char prefixDelimiter = '{', char suffixDelimiter = '}') {
            var forecastLength = (int)(text.Length * 1.25f);
            if (forecastLength <= 256) {
                RefWriter<char> buffer = new(stackalloc char[forecastLength]);
                try {
                    Replace(ref buffer, text, arguments, prefixDelimiter, suffixDelimiter);
                    return buffer.writtenSpan.ToString();
                } finally {
                    buffer.Dispose();
                }
            } else {
                RefWriter<char> buffer = new(forecastLength);
                try {
                    Replace(ref buffer, text, arguments, prefixDelimiter, suffixDelimiter);
                    return buffer.writtenSpan.ToString();
                } finally {
                    buffer.Dispose();
                }
            }
        }

        public static void Replace(ref RefWriter<char> buffer, ReadOnlySpan<char> text, ReadOnlySpan<string> arguments, char prefixDelimiter = '{', char suffixDelimiter = '}') {
            var argCount = arguments.Length;
            while (true) {
                int i;
                if ((i = text.IndexOfAny(prefixDelimiter, suffixDelimiter)) > -1) {
                    var c = text[i];
                    if (c == suffixDelimiter) {
                        buffer.Write(text[..++i]);
                        text = text[i..];
                        continue;
                    }

                    Rematch:
                    buffer.Write(text[..i++]);
                    text = text[i..];
                    if ((i = text.IndexOfAny(prefixDelimiter, suffixDelimiter)) > -1) {
                        c = text[i];
                        if (c == prefixDelimiter) {
                            buffer.Write(prefixDelimiter);
                            goto Rematch;
                        }

                        if (int.TryParse(text[..i], out var number) && argCount > number) {
                            buffer.Write(arguments[number]);
                            text = text[++i..];
                        } else {
                            buffer.Write(prefixDelimiter);
                            buffer.Write(text[..++i]);
                            text = text[i..];
                        }

                        continue;
                    }

                    buffer.Write(prefixDelimiter);
                }

                buffer.Write(text);
                break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string Replace(ReadOnlySpan<char> text, IDictionary<string, string> replaceQueryTable, char prefixDelimiter = '{', char suffixDelimiter = '}') {
            var forecastLength = (int)(text.Length * 1.25f);
            if (forecastLength <= 256) {
                RefWriter<char> buffer = new(stackalloc char[forecastLength]);
                try {
                    Replace(ref buffer, text, replaceQueryTable, prefixDelimiter, suffixDelimiter);
                    return buffer.writtenSpan.ToString();
                } finally {
                    buffer.Dispose();
                }
            } else {
                RefWriter<char> buffer = new(forecastLength);
                try {
                    Replace(ref buffer, text, replaceQueryTable, prefixDelimiter, suffixDelimiter);
                    return buffer.writtenSpan.ToString();
                } finally {
                    buffer.Dispose();
                }
            }
        }

        public static void Replace(ref RefWriter<char> buffer, ReadOnlySpan<char> text, IDictionary<string, string> replaceQueryTable, char prefixDelimiter = '{', char suffixDelimiter = '}') {
            while (true) {
                int i;
                if ((i = text.IndexOfAny(prefixDelimiter, suffixDelimiter)) > -1) {
                    var c = text[i];
                    if (c == suffixDelimiter) {
                        buffer.Write(text[..++i]);
                        text = text[i..];
                        continue;
                    }

                    Rematch:
                    buffer.Write(text[..i++]);
                    text = text[i..];
                    if ((i = text.IndexOfAny(prefixDelimiter, suffixDelimiter)) > -1) {
                        c = text[i];
                        if (c == prefixDelimiter) {
                            buffer.Write(prefixDelimiter);
                            goto Rematch;
                        }

                        var name = text[..i];
                        if (replaceQueryTable.TryGetValue(name.ToString(), out var content)) {
                            buffer.Write(content);
                        } else {
                            buffer.Write(prefixDelimiter);
                            buffer.Write(name);
                            buffer.Write(suffixDelimiter);
                        }

                        text = text[++i..];
                        continue;
                    }

                    buffer.Write(prefixDelimiter);
                }

                buffer.Write(text);
                break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string Replace(ReadOnlySpan<char> text, IDictionary<string, string> replaceQueryTable, ReadOnlySpan<char> prefixDelimiter, ReadOnlySpan<char> suffixDelimiter) {
            var forecastLength = (int)(text.Length * 1.25f);
            if (forecastLength <= 256) {
                RefWriter<char> buffer = new(stackalloc char[forecastLength]);
                try {
                    Replace(ref buffer, text, replaceQueryTable, prefixDelimiter, suffixDelimiter);
                    return buffer.writtenSpan.ToString();
                } finally {
                    buffer.Dispose();
                }
            } else {
                RefWriter<char> buffer = new(forecastLength);
                try {
                    Replace(ref buffer, text, replaceQueryTable, prefixDelimiter, suffixDelimiter);
                    return buffer.writtenSpan.ToString();
                } finally {
                    buffer.Dispose();
                }
            }
        }

        public static void Replace(ref RefWriter<char> buffer, ReadOnlySpan<char> text, IDictionary<string, string> replaceQueryTable, ReadOnlySpan<char> prefixDelimiter, ReadOnlySpan<char> suffixDelimiter) {
            var pLen = prefixDelimiter.Length;
            var sLen = suffixDelimiter.Length;

            while (true) {
                int i;
                if ((i = text.IndexOf(prefixDelimiter)) != -1) {
                    buffer.Write(text[..i]);
                    text = text[(i + pLen)..];
                    if ((i = text.IndexOf(suffixDelimiter)) != -1) {
                        if (replaceQueryTable.TryGetValue(text[..i].ToString(), out var content)) {
                            buffer.Write(content);
                        } else {
                            buffer.Write(prefixDelimiter);
                            buffer.Write(text[..(i + sLen)]);
                        }

                        text = text[(i + sLen)..];
                        continue;
                    }

                    buffer.Write(prefixDelimiter);
                }

                buffer.Write(text);
                break;
            }
        }
    }
}