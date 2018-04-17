using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualMachine
{
    public class Function
    {
        private readonly List<Command> _commands = new List<Command>();
        public IEnumerable<string> HackInstructions => 
            _commands.SelectMany(c => c.HackInstructions);
        
        public string FileName { get; }
        public string Name { get; }
        private LabelGenerator LabelGenerator { get; }
        public int Argc { get; }

        public Function(string fileName, string name, int argc)
        {
            FileName = fileName;
            Name = name;
            Argc = argc;
            LabelGenerator = new LabelGenerator(name);
            _commands.Add(Commands.DeclareFunction(name, argc));
        }

        public Function AddCommand(Command command)
        {
            _commands.Add(command);
            return this;
        }
        
        public Function AddCommands(IEnumerable<Command> command)
        {
            _commands.AddRange(command);
            return this;
        }

        public Function AddCommand(string command) => 
            AddCommand(Command.Parse(FileName, command, LabelGenerator));

        public Function Call(string callerName, int argc)
        {
            _commands.Add(Commands.FunctionCall(Name, argc, LabelGenerator));
            return this;
        }

        public Function Return()
        {
            _commands.Add(Commands.FunctionRetrun());
            return this;
        }

        public static Function Parse(string fileName, Queue<string> lines)
        {
            var declaration = lines.Dequeue().Split(' ');
            if (declaration[0] !="function") 
                throw new ArgumentException($"Expected function, Found : {declaration[0]}");

            var function = new Function(fileName, declaration[1], int.Parse(declaration[2]));
            
            while (lines.Count > 0 && !lines.Peek().StartsWith("function"))
            {
                var line = lines.Dequeue();
                function._commands.Add(Command
                    .Parse(fileName, line, function.LabelGenerator));
            }

            return function;
        }
    }
}