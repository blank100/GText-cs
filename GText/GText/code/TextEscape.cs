using System.Collections.Generic;

namespace Gal.Core
{
	/// <summary>
	/// 文本转义
	/// </summary>
	/// <para>author gouanlin</para>
	public static class TextEscape
	{
		private static readonly Dictionary<char, string> m_ComparisionTable = new() {
			['\\'] = "\\\\",
			['\"'] = "\\\"",
			['\n'] = "\\n",
			['\r'] = "\\r",
			['\t'] = "\\t",
			['\b'] = "\\b",
			['\f'] = "\\f",
			['\''] = "\\'",
		};

        public static unsafe string Escape(string text, bool forceAscii = false) {
	        var forecastLength = (int)(text.Length * 1.1f);
            if (forecastLength <= 256) {
	            RefWriter<char> writer = new(stackalloc char[forecastLength]);
	            try {
		            Escape(text, ref writer, forceAscii);
		            return writer.writtenSpan.ToString();
	            } finally {
		            writer.Dispose();
	            }
            } else {
	            RefWriter<char> writer = new(forecastLength);
	            try {
		            Escape(text, ref writer, forceAscii);
		            return writer.writtenSpan.ToString();
	            } finally {
		            writer.Dispose();
	            }
            }
        }

		public static void Escape(string text, ref RefWriter<char> writer, bool forceAscii = false) {
			var comparisionTable = m_ComparisionTable;
			if (forceAscii) {
				foreach (var c in text) {
					if (comparisionTable.TryGetValue(c, out var t)) {
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
					if (comparisionTable.TryGetValue(c, out var t)) {
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