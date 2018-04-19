using System;
using System.Collections.Generic;
using System.Linq;
using Analyzer.ProgramStructures;
using VirtualMachine;

namespace Compiler
{
    public class JackClassCompilationUnit
    {
        private readonly Dictionary<string, (VaribleType, int)> _fieldSymbols = 
            new Dictionary<string, (VaribleType, int)>();
        
        private readonly Dictionary<string, (VaribleType, int)> _staticSymbols = 
            new Dictionary<string, (VaribleType, int)>();

        public string ClassName { get; }
        public int FieldsCount => _fieldSymbols.Count;
        
        public IEnumerable<SubroutineCompilationUnit> SubroutineCompilationUnits { get; }
        
        public JackClassCompilationUnit(JackClass jackClass)
        {
            ClassName = jackClass.ClassName.Value;
            foreach (var field in jackClass.Fields)
            {
                _fieldSymbols.Add(field.Identifier.Value, 
                    (field.VaribleType, _fieldSymbols.Count));
            }
            
            foreach (var staticField in jackClass.StaticFields)
            {
                _staticSymbols.Add(staticField.Identifier.Value,
                    (staticField.VaribleType, _staticSymbols.Count));
            }

            SubroutineCompilationUnits = jackClass.Subroutines
                .Select(subroutine => new SubroutineCompilationUnit(this, subroutine));
        }

        public bool HasSymbol(string symbol)
        {
            return _fieldSymbols.ContainsKey(symbol) || _staticSymbols.ContainsKey(symbol);
        }

        public Command PushSymbol(string symbol)
        {
            if (!HasSymbol(symbol)) throw new ArgumentException("Unknown symbol");
            
            return _fieldSymbols.ContainsKey(symbol) 
                ? Commands.Push.This(_fieldSymbols[symbol].Item2) 
                : Commands.Push.Static(ClassName, _staticSymbols[symbol].Item2);
        }

        public Command PopSymbol(string symbol)
        {
            if (!HasSymbol(symbol)) throw new ArgumentException("Unknown symbol");
            
            return _fieldSymbols.ContainsKey(symbol) 
                ? Commands.Pop.This(_fieldSymbols[symbol].Item2) 
                : Commands.Pop.Static(ClassName, _staticSymbols[symbol].Item2);
        }

        public VaribleType GetSymbolType(string symbol)
        {
            if (!HasSymbol(symbol)) throw new ArgumentException("Unknown symbol");

            return _fieldSymbols.ContainsKey(symbol)
                ? _fieldSymbols[symbol].Item1
                : _staticSymbols[symbol].Item1;
        }
    }
}