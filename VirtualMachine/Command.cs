using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
// ReSharper disable SwitchStatementMissingSomeCases

namespace VirtualMachine
{
    public class Command
    {
        public IEnumerable<string> HackInstructions { get; }

        public Command(IEnumerable<string> hackInstructions)
        {
            HackInstructions = hackInstructions;
        }

        public static Command Parse(string fileName, string line, LabelGenerator generator)
        {
            var parts = line.Split(' ');
            var command = parts[0];
            var arg1 = parts.Length > 1 ? parts[1] : null;
            var arg2 = parts.Length > 2 ? int.Parse(parts[2]) : 0;
            
            switch (command)
            {
                case "add": return Arithmetic.Add();
                case "sub": return Arithmetic.Sub();
                case "neg": return Arithmetic.Neg();
                case "and": return Bitwise.And();
                case "or": return Bitwise.Or();
                case "not": return Bitwise.Not();
                case "eq": return Comparsion.Equal(generator);
                case "gt": return Comparsion.GreateThan(generator);
                case "lt": return Comparsion.LessThan(generator);
                case "function": return Function.DeclareFunction(arg1, arg2);
                case "call": return Function.Call(arg1, arg2, generator);
                case "return": return Function.Retrun();
                case "goto": return Branching.Goto(arg1);
                case "if-goto": return Branching.IfGoto(arg1);
                case "label": return Branching.Label(arg1);
                case "push":
                {
                    switch (arg1)
                    {
                        case "constant": return Push.Constant(arg2);
                        case "local": return Push.Local(arg2);
                        case "argument": return Push.Argument(arg2);
                        case "this": return Push.This(arg2);
                        case "that": return Push.That(arg2);
                        case "temp": return Push.Temp(arg2);
                        case "pointer": return Push.Pointer(arg2);
                        case "static": return Push.Static(fileName ,arg2);
                        default: throw new ArgumentException("Invalid segment");
                    }
                }
                case "pop":
                {
                    switch (arg1)
                    {
                        case "local": return Pop.Local(arg2);
                        case "argument": return Pop.Argument(arg2);
                        case "this": return Pop.This(arg2);
                        case "that": return Pop.That(arg2);
                        case "temp": return Pop.Temp(arg2);
                        case "pointer": return Pop.Pointer(arg2);
                        case "static": return Pop.Static(fileName ,arg2);
                        default: throw new ArgumentException("Invalid segment");
                    }
                }
                default: throw new InvalidOperationException("Invalid command");
            }
        }
    }
}