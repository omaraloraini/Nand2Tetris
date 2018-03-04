using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VirtualMachine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
                var vmFiles = directoryInfo
                    .GetFiles("*.vm")
                    .Select(fileInfo => fileInfo.Name)
                    .ToArray();
                
                
                if (vmFiles.Any())
                {
                    var assembly = vmFiles
                        .SelectMany(file => 
                            HackTranslator.Translate(file, Read(file)));

                    var bootstrapper = new[]
                    {
                        "@261",
                        "D=A",
                        "@SP",
                        "M=D",
                        "@Sys.init",
                        "0;JMP"
                    };
                    
                    

                    Write("a.asm", bootstrapper.Concat(assembly));
                }
                else
                {
                    Console.WriteLine("Please supply a file.");
                }
                
                return;
            }
            
            
            try
            {
                var fileName = args[0];
                var outputFile = fileName.Replace(".vm", ".asm");
                var lines = Read(fileName);
                Write(outputFile, HackTranslator.Translate(fileName, lines));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
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