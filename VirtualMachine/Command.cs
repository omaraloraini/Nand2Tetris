using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace VirtualMachine
{
    public class Command
    {
        private static Dictionary<string, Func<Command>> _arithmeticLogicalMap = 
            new Dictionary<string, Func<Command>>
            {
                ["add"] = ArithmeticCommand.Add,
                ["sub"] = ArithmeticCommand.Sub,
                ["neg"] = ArithmeticCommand.Neg,
                ["and"] = LogicalCommand.And,
                ["or"] = LogicalCommand.Or,
                ["not"] = LogicalCommand.Not,
                ["eq"] = LogicalCommand.Equal,
                ["gt"] = LogicalCommand.GreateThan,
                ["lt"] = LogicalCommand.LessThan
            };
        
        public IEnumerable<string> HackInstructions { get; }
        
        protected Command(IEnumerable<string> hackInstructions)
        {
            HackInstructions = hackInstructions;
        }

        public Command Combine(Command other)
        {
            return new Command(HackInstructions.Concat(other.HackInstructions));
        }
        
        public Command Combine(IEnumerable<Command> commands)
        {
            return commands.Aggregate(this, (a, b) => a.Combine(b));
        }

        public static Command Label(string label)
        {
            if (!label.StartsWith('('))
                label = '(' + label;
            
            if (!label.EndsWith(')')) 
                label = label + ')';
            
            return new Command(new[] {label});
        }

        public static Command Parse(string fileName, string line)
        {
            var parts = line.Split(' ');
            var command = parts[0];
            
            if (parts.Length == 1)
            {
                if (_arithmeticLogicalMap.ContainsKey(command))
                    return _arithmeticLogicalMap[command].Invoke();

                if (command == "return")
                    return FunctionCommand.Retrun();
            }

            if (parts.Length == 2)
            {
                switch (command)
                {
                    case "goto":
                        return BranchingCommand.Goto(parts[1]);
                    case "if-goto":
                        return BranchingCommand.IfGoto(parts[1]);
                    case "label":
                        return Label(parts[1]);
                }
            }

            if (parts.Length == 3)
            {
                var segment = parts[1];
                var index = int.Parse(parts[2]);
                
                switch (command)
                {
                    case "push":
                        return MemoryCommand.Push(fileName, segment, index);
                    case "pop":
                        return MemoryCommand.Pop(fileName, segment, index);
                    case "function":
                        return FunctionCommand.DeclareFunction(segment, index);
                    case "call":
                        return FunctionCommand.Call(fileName, segment, index);
                }
            }
            
            throw new InvalidOperationException("Invalid command");
        }
    }
}