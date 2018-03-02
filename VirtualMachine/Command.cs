using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public abstract class Command
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

        public static Command Parse(string fileName, string command)
        {
            var parts = command.Split(' ');
            if (parts.Length == 1 && _arithmeticLogicalMap.ContainsKey(command))
            {
                return _arithmeticLogicalMap[command].Invoke();
            }


            if (parts.Length == 3)
            {
                if (parts[0] == "push")
                    return MemoryCommand.Push(fileName, parts[1], int.Parse(parts[2]));

                if (parts[0] == "pop")
                    return MemoryCommand.Pop(fileName, parts[1], int.Parse(parts[2]));
            }
            
            throw new InvalidOperationException("Unsported command");
        }
    }
}