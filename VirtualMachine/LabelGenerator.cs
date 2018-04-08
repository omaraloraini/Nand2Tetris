using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VirtualMachine
{
    public class LabelGenerator
    {
        private readonly string _functionName;

        private readonly Dictionary<string, int> _labelCounter = new Dictionary<string, int>
        {
            ["eq"] = 0,
            ["gt"] = 0,
            ["lt"] = 0,
            ["ret"] = 0,
        };
        
        private Dictionary<string, string> _nameMap = new Dictionary<string, string>
        {
            ["Equal"] = "eq",
            ["GreateThan"] = "gt",
            ["LessThan"] = "lt",
            ["Call"] = "ret",
        };

        public LabelGenerator(string functionName)
        {
            _functionName = functionName;
        }
        
        public Label Generate([CallerMemberName] string name = null)
        {
            if (name is null) throw new ArgumentNullException();
            
            if (!_nameMap.ContainsKey(name)) 
                throw new ArgumentException("Caller not allowed");
            
            name = _nameMap[name];

            return new Label($"{_functionName}${name}.{_labelCounter[name]++}");
        }
    }
    
    public class Label
    {
        public readonly string Declaration; 
        public readonly string Address; 
        public Label(string label)
        {
            Declaration = $"({label})";
            Address = $"@{label}";
        }

        public static implicit operator Label(string label) => new Label(label);
    }
}