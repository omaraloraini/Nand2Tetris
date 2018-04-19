using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VirtualMachine
{
    public class LabelGenerator
    {
        private readonly string _functionName;

        private readonly Dictionary<string, int> _labelCounter = new Dictionary<string, int>();

        public LabelGenerator(string functionName)
        {
            _functionName = functionName;
        }
        
        public Label Generate(string name)
        {
            if (name is null) throw new ArgumentNullException();

            if (!_labelCounter.ContainsKey(name))
            {
                _labelCounter.Add(name, 0);
            }
            
            return new Label($"{_functionName}${name}.{_labelCounter[name]++}");
        }
    }
    
    public class Label
    {
        public readonly string Declaration; 
        public readonly string Address;
        public readonly string Text;
        
        public Label(string label)
        {
            Text = label;
            Declaration = $"({label})";
            Address = $"@{label}";
        }

        public static implicit operator Label(string label) => new Label(label);
    }
}