using System;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    internal class HtmlCmdLine
    {
        #region Static Members

        internal static bool Help;

        #endregion Static Members

        #region Constructors

        static HtmlCmdLine()
        {
            Help = false;
            ParseArgs();
        }

        #endregion Constructors

        #region Internal Methods

        internal static string GetOption(string name, string def)
        {
            var p = def;
            var args = Environment.GetCommandLineArgs();
            for (var i = 1; i < args.Length; i++) GetStringArg(args[i], name, ref p);

            return p;
        }

        internal static string GetOption(int index, string def)
        {
            var p = def;
            var args = Environment.GetCommandLineArgs();
            var j = 0;
            for (var i = 1; i < args.Length; i++)
                if (GetStringArg(args[i], ref p))
                {
                    if (index == j) return p;

                    p = def;

                    j++;
                }

            return p;
        }

        internal static bool GetOption(string name, bool def)
        {
            var p = def;
            var args = Environment.GetCommandLineArgs();
            for (var i = 1; i < args.Length; i++) GetBoolArg(args[i], name, ref p);

            return p;
        }

        internal static int GetOption(string name, int def)
        {
            var p = def;
            var args = Environment.GetCommandLineArgs();
            for (var i = 1; i < args.Length; i++) GetIntArg(args[i], name, ref p);

            return p;
        }

        #endregion Internal Methods

        #region Private Methods

        private static void GetBoolArg(string arg, string name, ref bool argValue)
        {
            if (arg.Length < name.Length + 1) // -name is 1 more than name
                return;

            if ('/' != arg[0] && '-' != arg[0]) // not a param
                return;

            if (arg.Substring(1, name.Length).ToLowerInvariant() == name.ToLowerInvariant()) argValue = true;
        }

        private static void GetIntArg(string arg, string name, ref int argValue)
        {
            if (arg.Length < name.Length + 3) // -name:12 is 3 more than name
                return;

            if ('/' != arg[0] && '-' != arg[0]) // not a param
                return;

            if (arg.Substring(1, name.Length).ToLowerInvariant() == name.ToLowerInvariant())
                try
                {
                    argValue = Convert.ToInt32(arg.Substring(name.Length + 2, arg.Length - name.Length - 2));
                }
                catch
                {
                    // ignored
                }
        }

        private static bool GetStringArg(string arg, ref string argValue)
        {
            if ('/' == arg[0] || '-' == arg[0]) return false;

            argValue = arg;
            return true;
        }

        private static void GetStringArg(string arg, string name, ref string argValue)
        {
            if (arg.Length < name.Length + 3) // -name:x is 3 more than name
                return;

            if ('/' != arg[0] && '-' != arg[0]) // not a param
                return;

            if (arg.Substring(1, name.Length).ToLowerInvariant() == name.ToLowerInvariant())
                argValue = arg.Substring(name.Length + 2, arg.Length - name.Length - 2);
        }

        private static void ParseArgs()
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 1; i < args.Length; i++)
            {
                // help
                GetBoolArg(args[i], "?", ref Help);
                GetBoolArg(args[i], "h", ref Help);
                GetBoolArg(args[i], "help", ref Help);
            }
        }

        #endregion Private Methods
    }
}