using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VirtualMachine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                SingleFileTranslation(args[0]);
                return;
            }
        
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            var vmFiles = directoryInfo
                .GetFiles("*.vm")
                .Select(fileInfo => fileInfo.Name)
                .ToDictionary(
                    fileName => fileName.Replace(".vm", ""),
                    Read);


            if (vmFiles.Count == 0) return;


            var outputFile = directoryInfo.Name + ".asm";
            Write(outputFile, HackTranslator.Translate(vmFiles));
        }

        private static void SingleFileTranslation(string fileName)
        {
            try
            {
                var name = fileName.Replace(".vm", "");
                var outputFile = fileName.Replace(".vm", ".asm");
                var assembly = HackTranslator.Translate(name, Read(fileName));
                Write(outputFile, assembly);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void Write(string fileName, IEnumerable<string> assembly)
        {
            File.WriteAllLines(fileName, assembly, Encoding.ASCII);
        }

        private static string[] Read(string fileName)
        {
            return File.ReadAllLines(fileName);
        }
    }
}