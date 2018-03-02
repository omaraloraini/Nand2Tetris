using System;
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

        public static Command Parse(string fileName, string line)
        {
            var parts = line.Split(' ');
            var command = parts[0];
            
            if (parts.Length == 1 && _arithmeticLogicalMap.ContainsKey(command))
            {
                return _arithmeticLogicalMap[command].Invoke();
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
                }
            }
            
            throw new InvalidOperationException("Invalid command");
        }
    }
}