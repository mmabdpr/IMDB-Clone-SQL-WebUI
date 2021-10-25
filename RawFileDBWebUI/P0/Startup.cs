using System.IO;

namespace P0
{
    public static class Startup
    {
        public static void Initialize()
        {
            EnsureDBFilesExists(Settings.FileSettings.FilePaths);
        }

        private static void EnsureDBFilesExists(string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                if (!File.Exists(filePath))
                    File.CreateText(filePath).Dispose();
            }
        }
    }
}
