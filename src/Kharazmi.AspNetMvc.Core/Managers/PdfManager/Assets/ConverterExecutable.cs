using System;
using System.IO;
using System.Reflection;

namespace Kharazmi.AspNetMvc.Core.Managers.PdfManager.Assets
{
    internal sealed class ConverterExecutable
    {
        private ConverterExecutable()
        {
        }

        public string FullConverterExecutableFilename => ResolveFullPathToConverterExecutableFile();

        public static ConverterExecutable Get()
        {
            var bundledFile = new ConverterExecutable();

            bundledFile.CreateIfConverterExecutableDoesNotExist();

            return bundledFile;
        }

        private void CreateIfConverterExecutableDoesNotExist()
        {
            if (!File.Exists(FullConverterExecutableFilename)) Create(GetConverterExecutableContent());
        }

        private static byte[] GetConverterExecutableContent()
        {
            using (var resourceStream = GetConverterExecutable())
            {
                var resource = new byte[resourceStream.Length];

                resourceStream.Read(resource, 0, resource.Length);

                return resource;
            }
        }

        private static Stream GetConverterExecutable()
        {
            return Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"{nameof(Assets)}.{Constants.CONVERTER_EXECUTABLE_FILENAME}");
        }

        private void Create(byte[] fileContent)
        {
            try
            {
                if (!Directory.Exists(BundledFilesDirectory())) Directory.CreateDirectory(BundledFilesDirectory());


                using (var file = File.Open(FullConverterExecutableFilename, FileMode.Create))
                {
                    file.Write(fileContent, 0, fileContent.Length);
                }
            }
            catch (IOException)
            {
            }
        }

        private static string ResolveFullPathToConverterExecutableFile()
        {
            return Path.Combine(BundledFilesDirectory(), Constants.CONVERTER_EXECUTABLE_FILENAME);
        }

        private static string BundledFilesDirectory()
        {
            return Path.Combine(Path.GetTempPath(), nameof(Mvc.Utility.Core), Version());
        }

        private static string Version()
        {
            return $"{Assembly.GetExecutingAssembly().GetName().Version}_{(Environment.Is64BitProcess ? 64 : 32)}";
        }
    }
}