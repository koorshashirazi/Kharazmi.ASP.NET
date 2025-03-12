using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Logging
{
    /// <summary>
    ///     Simple implementation of <see cref="TextWriter" /> that writes to <see cref="System.Diagnostics.Debug" />.
    ///     Pulled from http://damieng.com/blog/2008/07/30/linq-to-sql-log-to-debug-window-file-memory-or-multiple-writers.
    /// </summary>
    public class TraceTextWriter : TextWriter
    {
        private TraceTextWriter()
        {
        }

        /// <summary>
        ///     Lazy instance of this class.
        /// </summary>
        public static TraceTextWriter Instance => Nested.NestedInstance;

        /// <summary>
        ///     Defaults to <see cref="System.Text.Encoding.Default" />.
        /// </summary>
        public override Encoding Encoding => Encoding.Default;

        /// <summary>
        ///     Delegates writing to <see cref="Debug.Write(string)" />
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="index">index</param>
        /// <param name="count">count</param>
        public override void Write(char[] buffer, int index, int count)
        {
            Trace.Write(new string(buffer, index, count));
        }

        /// <summary>
        ///     Delegates writing to <see cref="Debug.Write(string)" />
        /// </summary>
        /// <param name="value">The value.</param>
        public override void Write(string value)
        {
            Trace.Write(value);
        }

        /// <summary>
        ///     Delegates writing to <see cref="Debug.WriteLine(string)" />
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="index">index</param>
        /// <param name="count">count</param>
        public override void WriteLine(char[] buffer, int index, int count)
        {
            Trace.WriteLine(new string(buffer, index, count));
        }

        /// <summary>
        ///     Delegates writing to <see cref="Debug.WriteLine(string)" />
        /// </summary>
        /// <param name="value">The value.</param>
        public override void WriteLine(string value)
        {
            Trace.WriteLine(value);
        }

        /// <summary>
        ///     Calls <see cref="Debug.WriteLine(string)" /> and passes <see cref="string.Empty" />.
        /// </summary>
        public override void WriteLine()
        {
            Trace.WriteLine(string.Empty);
        }

        private class Nested
        {
            internal static readonly TraceTextWriter NestedInstance = new TraceTextWriter();

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
        }
    }
}