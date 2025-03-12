using System;
using System.Runtime.InteropServices;

namespace Kharazmi.HtmlToPdf.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void IntCallback(IntPtr converter, int str);
}