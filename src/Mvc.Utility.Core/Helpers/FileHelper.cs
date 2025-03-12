using System.IO;

namespace Mvc.Utility.Core.Helpers
{
    public static class FileHelper
    {
        public static void CreateDirectoryIfMissing(string pathDirectoryI)
        {
            if (!Directory.Exists(pathDirectoryI)) Directory.CreateDirectory(pathDirectoryI);
        }

        public static void DeleteDirectory(string pathFiles, bool recursive)
        {
            if (!Directory.Exists(pathFiles)) return;

            foreach (var file in Directory.GetFiles(pathFiles)) File.Delete(file);

            if (recursive)
                foreach (var dir in Directory.GetDirectories(pathFiles))
                    DeleteDirectory(dir, true);

            Directory.Delete(pathFiles);
        }

        public static void DeleteFile(string physicalPath)
        {
            if (File.Exists(physicalPath)) File.Delete(physicalPath);
        }

        public static void SaveFileBytes(byte[] bytes, string path)
        {
            File.WriteAllBytes(path, bytes);
        }

        public static byte[] LoadFileBytes(string path)
        {
            if (File.Exists(path)) return File.ReadAllBytes(path);

            throw ExceptionHelper.ThrowException<FileNotFoundException>(string.Empty, nameof(path));
        }

        public static FileStream LoadFileStream(string path)
        {
            if (File.Exists(path)) return new FileStream(path, FileMode.Open, FileAccess.Read);

            throw ExceptionHelper.ThrowException<FileNotFoundException>(string.Empty, nameof(path));
        }
    }
}