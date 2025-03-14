﻿using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Kharazmi.HtmlToPdf.Assets;
using Kharazmi.HtmlToPdf.Interop;

namespace Kharazmi.HtmlToPdf.WkHtmlToX
{
    sealed class WkHtmlToPdfLibrary
    {
        private const string LibraryFilename = "wkhtmltox.dll";
        private const string Compressed32BitLibraryFilename = "wkhtmltox_32.dll";
        private const string Compressed64BitLibraryFilename = "wkhtmltox_64.dll";

        public static NativeLibrary Load()
        {
            return NativeLibrary.Load(LibraryFilename, LoadLibraryContent);
        }

        private static byte[] LoadLibraryContent()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new PlatformNotSupportedException(String.Format("Platform {0} is not supported", Platform()));

            using (var wkhtmltoxZipArchive = WkHtmlToXZipArchive())
            {
                return wkhtmltoxZipArchive.ReadFile(CompressedLibraryFilename());
            }
        }

        private static  ZipArchive WkHtmlToXZipArchive()
        {
            return new ZipArchive(GetManifestResourceStream());
        }

        private static Stream GetManifestResourceStream()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("OpenHtmlToPdf.WkHtmlToPdf.Assets.wkhtmltox.zip");
        }

        private static string CompressedLibraryFilename()
        {
            return Environment.Is64BitProcess
                ? Compressed64BitLibraryFilename
                : Compressed32BitLibraryFilename;
        }

        private static string Platform()
        {
            return Enum.GetName(typeof(PlatformID), Environment.OSVersion.Platform);
        }
    }
}