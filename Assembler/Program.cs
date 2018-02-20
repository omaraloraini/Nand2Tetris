using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Assembler
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
                var lines = File.ReadAllLines(args[0]);
                var assemble = HackAssembler.Assemble(lines);
                var outputFile = args[0].Replace(".asm", ".hack");
                File.WriteAllLines(outputFile, assemble, Encoding.ASCII);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}