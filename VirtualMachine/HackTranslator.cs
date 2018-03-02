using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    public class HackTranslator
    {
        public static IEnumerable<string> Translate(string fileName, string[] lines) =>
            ExtractCommands(lines)
                .SelectMany(command => Command.Parse(fileName, command).HackInstructions);

        private static IEnumerable<string> ExtractCommands(IEnumerable<string> lines) =>
            lines
                .Where(line => line != string.Empty && !line.StartsWith("//"))
                .Select(line => line.Contains("//")
                    ? line.Remove(line.IndexOf("//")).Trim()
                    : line.Trim());
    }
}