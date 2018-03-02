using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VirtualMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please supply a file.");
                return;
            }

            try
            {
                var fileName = args[0].Substring(0, args[0].IndexOf('.'));
                Console.WriteLine($"Reading {fileName}");
                var lines = File.ReadAllLines(args[0]);
                var assemble = HackTranslator.Translate(fileName, lines);
                var outputFile = fileName + ".asm";
                Console.WriteLine($"Writing to {outputFile}");
                File.WriteAllLines(outputFile, assemble, Encoding.ASCII);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }
    }

    public class HackTranslator
    {
        public static string[] Translate(string fileName, string[] lines)
        {
            return ExtractCommands(lines)
                .SelectMany(command => Command.Parse(fileName, command).HackInstructions)
                .ToArray();
        }
        
        private static List<string> ExtractCommands(string[] lines)
        {
            return lines
                .Where(line => line != string.Empty && !line.StartsWith("//"))
                .Select(line => line.Trim())
                .ToList();
        }
    }

    internal class Instruction
    {   
            
        
    }
}