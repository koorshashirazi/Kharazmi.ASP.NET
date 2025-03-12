﻿using System;
using System.Collections;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Logging
{
    internal class ObjectDumper
    {
        private readonly int _depth;
        private int _level;
        private int _pos;
        private TextWriter _writer;

        private ObjectDumper(int depth)
        {
            _depth = depth;
        }

        public static void Write(object element)
        {
            Write(element, 0);
        }

        public static void Write(object element, int depth)
        {
            Write(element, depth, Console.Out);
        }

        public static void Write(object element, int depth, TextWriter log)
        {
            var dumper = new ObjectDumper(depth) {_writer = log};
            dumper.WriteObject(null, element);
        }

        private void Write(string s)
        {
            if (s == null) return;

            _writer.Write(s);
            _pos += s.Length;
        }

        private void WriteIndent()
        {
            for (var i = 0; i < _level; i++) _writer.Write("  ");
        }

        private void WriteLine()
        {
            _writer.WriteLine();
            _pos = 0;
        }

        private void WriteTab()
        {
            Write("  ");
            while (_pos % 8 != 0) Write(" ");
        }

        private void WriteObject(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                WriteLine();
            }
            else
            {
                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (var item in enumerableElement)
                        if (item is IEnumerable && !(item is string))
                        {
                            WriteIndent();
                            Write(prefix);
                            Write("...");
                            WriteLine();
                            if (_level < _depth)
                            {
                                _level++;
                                WriteObject(prefix, item);
                                _level--;
                            }
                        }
                        else
                        {
                            WriteObject(prefix, item);
                        }
                }
                else
                {
                    var members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    WriteIndent();
                    Write(prefix);
                    var propWritten = false;
                    foreach (var m in members)
                    {
                        var f = m as FieldInfo;
                        var p = m as PropertyInfo;
                        if (f == null && p == null) continue;

                        if (propWritten)
                            WriteTab();
                        else
                            propWritten = true;
                        Write(m.Name);
                        Write("=");
                        var t = f != null ? f.FieldType : p.PropertyType;
                        if (t.IsValueType || t == typeof(string))
                        {
                            WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                        }
                        else if (typeof(DirectoryResponse).IsAssignableFrom(t))
                        {
                            var value =
                                (f != null ? f.GetValue(element) : p.GetValue(element, null)) as DirectoryResponse;

                            if (value != null)
                            {
                                Write(string.Format(
                                    "[ ErrorMessage: {0}, MatchedDN: {1}, ResultCode: {2}, RequestId: {3}, Controls: {4}, Referrals: {5} ]",
                                    value.ErrorMessage,
                                    value.MatchedDN,
                                    value.ResultCode,
                                    value.RequestId,
#if NET35
                                    string.Join(" | ", value.Controls.Select(c => c.Type).ToArray()), string.Join(" | ", value.Referral.Select(u => u.ToString()).ToArray())));
#else
                                    string.Join(" | ", value.Controls.Select(c => c.Type)),
                                    string.Join(" | ", value.Referral.Select(u => u.ToString()))));
#endif
                            }
                        }
                        else
                        {
                            Write(typeof(IEnumerable).IsAssignableFrom(t) ? "..." : "{ }");
                        }
                    }

                    if (propWritten) WriteLine();
                    if (_level < _depth)
                        foreach (var m in members)
                        {
                            var f = m as FieldInfo;
                            var p = m as PropertyInfo;
                            if (f == null && p == null) continue;

                            var t = f != null ? f.FieldType : p.PropertyType;
                            if (t.IsValueType || t == typeof(string)) continue;

                            var value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                            if (value == null) continue;

                            _level++;
                            WriteObject(m.Name + ": ", value);
                            _level--;
                        }
                }
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
                Write("null");
            else if (o is DateTime)
                Write(((DateTime) o).ToShortDateString());
            else if (o is ValueType || o is string)
                Write(o.ToString());
            else if (o is IEnumerable)
                Write("...");
            else
                Write("{ }");
        }
    }
}