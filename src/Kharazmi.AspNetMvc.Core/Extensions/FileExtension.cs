using System;
using System.IO;
using System.Web;
using Kharazmi.AspNetMvc.Core.Helpers;

namespace Kharazmi.AspNetMvc.Core.Extensions
{
    public static partial class Common
    {
        public static byte[] ToByte(this HttpPostedFileBase file)
        {
            if (file == null) throw ExceptionHelper.ThrowException<ArgumentNullException>(string.Empty, nameof(file));

            using (var inputStream = file.InputStream)
            {
                if (inputStream is MemoryStream memoryStream) return memoryStream.ToArray();

                memoryStream = new MemoryStream();
                inputStream.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public static byte[] ToBytes(this MemoryStream memoryStream)
        {
            if (memoryStream == null)
                throw ExceptionHelper.ThrowException<ArgumentNullException>(string.Empty, nameof(memoryStream));

            return memoryStream.ToArray();
        }

        public static MemoryStream ToMemoryStream(this byte[] bytes)
        {
            if (bytes == null) throw ExceptionHelper.ThrowException<ArgumentNullException>(string.Empty, nameof(bytes));

            var stream = new MemoryStream(bytes);
            var streamWriter = new StreamWriter(stream);
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static void WriteToFile(this byte[] bytes, string path)
        {
            File.WriteAllBytes(path, bytes);
        }

        public static byte[] ReadBytes(this Stream stream)
        {
            var dataToRead = new byte[stream.Length];
            var currentByteRead = 0;
            var nextBytesRead = 1;
            var countDataToRead = dataToRead.Length;

            while (currentByteRead < dataToRead.Length && nextBytesRead > 0)
            {
                nextBytesRead = stream.Read(dataToRead, currentByteRead, countDataToRead - currentByteRead);

                currentByteRead = currentByteRead + nextBytesRead;
            }

            return dataToRead;
        }
    }
}