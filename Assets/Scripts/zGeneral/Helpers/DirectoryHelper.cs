using System.IO;

namespace Lines.Scripts.General.Helpers
{
    public static class DirectoryHelper
    {
        public static string GetScriptDataDirectory()
        {
            char[] separators = {
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar
            };
            string currentPath = Directory.GetCurrentDirectory();

            return currentPath.TrimEnd(separators) + GetScriptDataPart();
        }

        private static string GetScriptDataPart()
        {
            return "/Assets/ScriptData";
        }
    }
}
