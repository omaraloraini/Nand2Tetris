using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    public class HackTranslator
    {
        public static IEnumerable<string> Translate(string fileName, string[] lines,
            LabelGenerator generator = null)
        {
            generator = generator ?? new LabelGenerator();
            return ExtractCommands(lines)
                .SelectMany(line => 
                    Command.Parse(
                        fileName,
                        line,
                        generator)
                    .HackInstructions);
        }

        public static IEnumerable<string> Translate(Dictionary<string, string[]> files)
        {
            var generator = new LabelGenerator();
            return Bootstrapper(generator)
                .Concat(files.SelectMany(file => Translate(
                    file.Key,
                    file.Value,
                    generator)));
        }

        private static IEnumerable<string> ExtractCommands(IEnumerable<string> lines) =>
            lines
                .Where(line => line != string.Empty && !line.StartsWith("//"))
                .Select(line => line.Contains("//")
                    ? line.Remove(line.IndexOf("//")).Trim()
                    : line.Trim());

        private static IEnumerable<string> Bootstrapper(LabelGenerator generator)
        {
            return new[]
            {
                "@256",
                "D=A",
                "@SP",
                "M=D"
            }.Concat(FunctionCommand
                .Call("Sys.init", 0, generator).HackInstructions);
        }
    }
}