using System;
using System.IO;
using System.Text;

namespace VirtualMachine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please supply a file.");
                return;
            }

            try
            {
                var fileName = args[0].Substring(0, args[0].IndexOf('.'));
                var lines = File.ReadAllLines(args[0]);
                var assemble = HackTranslator.Translate(fileName, lines);
                var outputFile = fileName + ".asm";
                File.WriteAllLines(outputFile, assemble, Encoding.ASCII);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}