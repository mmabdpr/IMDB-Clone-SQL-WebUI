using System.IO;
using System.Linq;

namespace P0.Helpers
{
    public static class DBIndexHelper
    {
        public static int GetLastIndex(string filePath) => Helpers.TextFileHelper.IsTextFileEmpty(filePath) ?
            0 :
            File.ReadAllLines(filePath).Select(x => int.Parse(x.Split('-').First())).Max();
    }
}
