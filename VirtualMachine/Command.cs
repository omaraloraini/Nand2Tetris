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
                case "add": return ArithmeticCommand.Add();
                case "sub": return ArithmeticCommand.Sub();
                case "neg": return ArithmeticCommand.Neg();
                case "and": return Bitwise.And();
                case "or": return Bitwise.Or();
                case "not": return Bitwise.Not();
                case "eq": return Comparsion.Equal(generator);
                case "gt": return Comparsion.GreateThan(generator);
                case "lt": return Comparsion.LessThan(generator);
                case "return": return FunctionCommand.Retrun();
                case "goto": return BranchingCommand.Goto(arg1);
                case "if-goto": return BranchingCommand.IfGoto(arg1);
                case "label": return BranchingCommand.Label(arg1);
                case "push": return MemoryCommand.Push(fileName, arg1, arg2);
                case "pop": return MemoryCommand.Pop(fileName, arg1, arg2);
                case "call": return FunctionCommand.Call(arg1, arg2, generator);
                case "function": return FunctionCommand.DeclareFunction(arg1, arg2);
                default: throw new InvalidOperationException("Invalid command");
            }
        }
    }
}