using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
// ReSharper disable SwitchStatementMissingSomeCases

namespace VirtualMachine
{
    public class Command
    {
        public string Name { get;}
        public IEnumerable<string> HackInstructions { get; }

        public Command(string name, IEnumerable<string> hackInstructions)
        {
            Name = name;
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
                case "add": return Commands.Arithmetic.Add();
                case "sub": return Commands.Arithmetic.Sub();
                case "neg": return Commands.Arithmetic.Neg();
                case "and": return Commands.Bitwise.And();
                case "or": return Commands.Bitwise.Or();
                case "not": return Commands.Bitwise.Not();
                case "eq": return Commands.Comparsion.Equal(generator);
                case "gt": return Commands.Comparsion.GreateThan(generator);
                case "lt": return Commands.Comparsion.LessThan(generator);
                case "function": return Commands.DeclareFunction(arg1, arg2);
                case "call": return Commands.FunctionCall(arg1, arg2, generator);
                case "return": return Commands.FunctionRetrun();
                case "goto": return Commands.Goto(arg1);
                case "if-goto": return Commands.IfGoto(arg1);
                case "label": return Commands.Label(arg1);
                case "push":
                {
                    switch (arg1)
                    {
                        case "constant": return Commands.Push.Constant(arg2);
                        case "local": return Commands.Push.Local(arg2);
                        case "argument": return Commands.Push.Argument(arg2);
                        case "this": return Commands.Push.This(arg2);
                        case "that": return Commands.Push.That(arg2);
                        case "temp": return Commands.Push.Temp(arg2);
                        case "pointer": return Commands.Push.Pointer(arg2);
                        case "static": return Commands.Push.Static(fileName ,arg2);
                        default: throw new ArgumentException("Invalid segment");
                    }
                }
                case "pop":
                {
                    switch (arg1)
                    {
                        case "local": return Commands.Pop.Local(arg2);
                        case "argument": return Commands.Pop.Argument(arg2);
                        case "this": return Commands.Pop.This(arg2);
                        case "that": return Commands.Pop.That(arg2);
                        case "temp": return Commands.Pop.Temp(arg2);
                        case "pointer": return Commands.Pop.Pointer(arg2);
                        case "static": return Commands.Pop.Static(fileName ,arg2);
                        default: throw new ArgumentException("Invalid segment");
                    }
                }
                default: throw new InvalidOperationException("Invalid command");
            }
        }
    }
}