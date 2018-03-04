using System;
using System.Collections.Generic;

namespace VirtualMachine
{
    public class MemoryCommand : Command
    {
        private MemoryCommand(IEnumerable<string> hackInstructions) : base(hackInstructions)
        {
        }

        public static MemoryCommand Push(string fileName, string segment, int index)
        {
            var indexPrefixed = $"@{index}";
            switch (segment)
            {
                case "constant":
                    return PushConstant(indexPrefixed);
                case "local":
                    return PushFromSegment("@LCL", indexPrefixed);
                case "argument":
                    return PushFromSegment("@ARG", indexPrefixed);
                case "this":
                    return PushFromSegment("@THIS", indexPrefixed);
                case "that":
                    return PushFromSegment("@THAT", indexPrefixed);
                case "temp":
                    return PushFromLabel("@" + (5 + index));
                case "pointer":
                    return (index == 0)
                        ? PushFromLabel("@THIS")
                        : PushFromLabel( "@THAT");
                case "static":
                    return PushFromLabel($"@{fileName}.{index}");
                default:
                    throw new ArgumentException(nameof(segment));
            }
        }

        public static MemoryCommand Pop(string fileName, string segment, int index)
        {
            var indexPrefixed = $"@{index}";
            switch (segment)
            {
                case "local":
                    return PopToSegment("@LCL", indexPrefixed);
                case "argument":
                    return PopToSegment("@ARG", indexPrefixed);
                case "this":
                    return PopToSegment("@THIS", indexPrefixed);
                case "that":
                    return PopToSegment("@THAT", indexPrefixed);
                case "temp":
                    return PopToLabel("@" + (5 + index));
                case "pointer":
                    return (index == 0)
                        ? PopToLabel("@THIS")
                        : PopToLabel("@THAT");
                case "static":
                    return PopToLabel($"@{fileName}.{index}");
                default:
                    throw new ArgumentException(nameof(segment));
            }
        }

        internal static MemoryCommand PushConstant(string constant)
        {
            return new MemoryCommand(new[]
            {
                constant,
                "D=A",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D"
            });
        }
        
        internal static MemoryCommand PushFromLabel(string label)
        {
            return new MemoryCommand(new[]
            {
                label,
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D"
            });
        }

        private static MemoryCommand PushFromSegment(string baseAddress, string offset)
        {
            return new MemoryCommand(new[]
            {
                baseAddress,
                "D=M",
                offset,
                "A=D+A",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",
            });
        }

        private static MemoryCommand PopToLabel(string label)
        {
            return new MemoryCommand(new[]
            {
                "@SP",
                "M=M-1",
                "A=M",
                "D=M",
                label,
                "M=D"
            });
        }

        private static MemoryCommand PopToSegment(string baseAddress, string offset)
        {
            return new MemoryCommand(new[]
            {
                baseAddress,
                "D=M",
                offset,
                "D=D+A",
                "@SP",
                "M=M-1",
                "A=M+1",
                "M=D",
                "A=A-1",
                "D=M",
                "A=A+1",
                "A=M",
                "M=D"
            });
        }
    }
}