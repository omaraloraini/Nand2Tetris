using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    public class LogicalCommand : Command
    {
        public static LogicalCommand LessThan() => LessThan("");
        public static LogicalCommand GreateThan() => GreateThan("");
        public static LogicalCommand Equal() => Equal("");
        public static LogicalCommand Not() => Not("");
        public static LogicalCommand Or() => Or("");
        public static LogicalCommand And() => And("");

        private LogicalCommand(IEnumerable<string> hackInstructions) : base(hackInstructions)
        {
        }

        private static readonly Random Random = new Random();

        private static string GenerateRandomLabel()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return string.Join("",
                Enumerable
                    .Repeat(0, 6)
                    .Select(i => chars[Random.Next(0, chars.Length - 10 + i)])
                    .ToArray());
        }

        private static LogicalCommand Comparsion(string jump, string label)
        {
            return new LogicalCommand(
                new[]
                {
                    "@SP",
                    "M=M-1",
                    "A=M",
                    "D=M",
                    "A=A-1",
                    "D=D-M",
                    "M=-1",
                    $"@{label}",
                    $"D;{jump}",
                    "@SP",
                    "A=M-1",
                    "M=0",
                    $"({label})"
                });
        }

        public static LogicalCommand And(string label)
        {
            return new LogicalCommand(
                new[]
                {
                    "@SP",
                    "M=M-1",
                    "A=M",
                    "D=M",
                    "A=A-1",
                    "M=D&M"
                });
        }

        public static LogicalCommand Or(string label)
        {
            return new LogicalCommand(
                new[]
                {
                    "@SP",
                    "M=M-1",
                    "A=M",
                    "D=M",
                    "A=A-1",
                    "M=D|M"
                });
        }

        public static LogicalCommand Not(string label)
        {
            return new LogicalCommand(new[]
            {
                "@SP",
                "A=M-1",
                "M=!M"
            });
        }

        public static LogicalCommand Equal(string label)
        {
            if (label == string.Empty)
                label = GenerateRandomLabel();

            return Comparsion("JEQ", label);
        }

        private static LogicalCommand GreateThan(string label)
        {
            if (label == string.Empty)
                label = GenerateRandomLabel();

            return Comparsion("JLT", label);
        }


        private static LogicalCommand LessThan(string label)
        {
            if (label == string.Empty)
                label = GenerateRandomLabel();

            return Comparsion("JGT", label);
        }
    }
}