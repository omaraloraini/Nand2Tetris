using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    public static partial class Commands
    {
        public class Comparsion
        {
            private static Command Compare(string name, string jump, Label label)
            {
                return new Command(
                    name,
                    new[]
                    {
                        "@SP",
                        "AM=M-1",
                        "D=M",
                        "A=A-1",
                        "D=D-M",
                        "M=-1",
                        label.Address,
                        $"D;{jump}",
                        "@SP",
                        "A=M-1",
                        "M=0",
                        label.Declaration
                    });
            }

            public static Command Equal(LabelGenerator generator) => 
                Compare("eq", "JEQ", generator.Generate());

            public static Command GreateThan(LabelGenerator generator) => 
                Compare("gt", "JLT", generator.Generate());

            public static Command LessThan(LabelGenerator generator) => 
                Compare("lt", "JGT", generator.Generate());
        }
    }
}