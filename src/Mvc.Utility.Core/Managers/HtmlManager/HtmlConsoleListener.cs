using System;
using System.Diagnostics;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    internal class HtmlConsoleListener : TraceListener
    {
        #region Public Methods

        public override void Write(string message)
        {
            Write(message, "");
        }

        public override void Write(string message, string category)
        {
            Console.Write("T:" + category + ": " + message);
        }

        public override void WriteLine(string message)
        {
            Write(message + "\n");
        }

        public override void WriteLine(string message, string category)
        {
            Write(message + "\n", category);
        }

        #endregion Public Methods
    }
}