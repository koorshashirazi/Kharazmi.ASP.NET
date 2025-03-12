using System;
using System.Runtime.InteropServices;

namespace Kharazmi.HtmlToPdf.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VoidCallback(IntPtr converter);
}