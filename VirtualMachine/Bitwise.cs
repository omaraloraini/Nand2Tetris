﻿namespace VirtualMachine
{
    public class Bitwise
    {
        public static Command And()
        {
            return new Command(
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

        public static Command Or()
        {
            return new Command(
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

        public static Command Not()
        {
            return new Command(new[]
            {
                "@SP",
                "A=M-1",
                "M=!M"
            });
        }
    }
}