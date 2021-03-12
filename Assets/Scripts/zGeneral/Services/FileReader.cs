using System.IO;

namespace Lines.Scripts.General.Services
{
    public static class FileReader
    {
        public static string Read(string path, string fileName, string fileExtension)
        {
            char[] separators = {
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar
            };
            var fullPath = path.TrimEnd(separators)
               + Path.AltDirectorySeparatorChar
               + fileName.TrimStart(separators)
               + "."
               + fileExtension.TrimStart('.');

            return new StreamReader(fullPath).ReadToEnd();
        }
    }
}
