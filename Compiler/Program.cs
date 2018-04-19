using System.IO;
using System.Linq;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            var jackFiles = directoryInfo
                .GetFiles("*.jack")
                .Select(fileInfo => fileInfo.Name)
                .ToDictionary(
                    fileName => fileName.Replace(".jack", ".vm"),
                    File.ReadAllText);
            
            if (jackFiles.Count == 0) return;
            
            foreach (var pair in jackFiles)
            {
                File.WriteAllLines(pair.Key, JackCompiler.Compile(pair.Value).Select(c => c.Name));
            }
        }
    }
}