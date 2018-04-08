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
                    File.ReadAllLines);


            if (vmFiles.Count == 0) return;


            var outputFile = directoryInfo.Name + ".asm";
            File.WriteAllLines(outputFile, HackTranslator.Translate(vmFiles));
        }

        private static void SingleFileTranslation(string fileName)
        {
            try
            {
                var name = fileName.Replace(".vm", "");
                var outputFile = fileName.Replace(".vm", ".asm");
                var assembly = HackTranslator.Translate(name, File.ReadAllLines(fileName));
                File.WriteAllLines(outputFile, assembly);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}