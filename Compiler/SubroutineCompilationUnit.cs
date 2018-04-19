using System;
using System.Collections.Generic;
using System.Linq;
using Analyzer.Expressions;
using Analyzer.ProgramStructures;
using Analyzer.Statements;
using VirtualMachine;

namespace Compiler
{
    public class SubroutineCompilationUnit
    {
        private readonly JackClassCompilationUnit _classCompilationUnit;
        private readonly Subroutine _subroutine;

        private readonly Dictionary<string, (VaribleType, int)> _argumentSymbols = 
            new Dictionary<string, (VaribleType, int)>();
        private readonly Dictionary<string, (VaribleType, int)> _localSymbols = 
            new Dictionary<string, (VaribleType, int)>();

        public string Name { get; }
        public string ClassName { get; set; }
        public LabelGenerator LabelGenerator { get;}
        public IEnumerable<Statement> Statements => _subroutine.Statements;
        
        public SubroutineCompilationUnit(JackClassCompilationUnit classCompilationUnit,
            Subroutine subroutine)
        {   
            _subroutine = subroutine;
            _classCompilationUnit = classCompilationUnit;
            if (subroutine is Method)
            {
                _argumentSymbols.Add("this", 
                    (new VaribleType(classCompilationUnit.ClassName), 0));
            }

            foreach (var argument in subroutine.Arguments)
                _argumentSymbols.Add(argument.Identifier.Value,
                    (argument.VaribleType, _argumentSymbols.Count));
            
            foreach (var local in subroutine.LocalVaribles)
                _localSymbols.Add(local.Identifier.Value,
                    (local.VaribleType ,_localSymbols.Count));
            
            
            Name = $"{classCompilationUnit.ClassName}.{subroutine.Name.Value}";
            ClassName = classCompilationUnit.ClassName;
            LabelGenerator = new LabelGenerator(Name);
        }
        
        public bool HasSymbol(string symbol)
        {
            return _localSymbols.ContainsKey(symbol) ||
                   _argumentSymbols.ContainsKey(symbol) ||
                   _classCompilationUnit.HasSymbol(symbol);
        }

        public Command PushSymbol(string symbol)
        {
            if (!HasSymbol(symbol)) throw new ArgumentException("Unknown symbol");

            if (_localSymbols.ContainsKey(symbol))
                return Commands.Push.Local(_localSymbols[symbol].Item2);

            if (_argumentSymbols.ContainsKey(symbol))
                return Commands.Push.Argument(_argumentSymbols[symbol].Item2);

            return _classCompilationUnit.PushSymbol(symbol);
        }

        public Command PopSymbol(string symbol)
        {
            if (!HasSymbol(symbol)) throw new ArgumentException("Unknown symbol");

            if (_localSymbols.ContainsKey(symbol))
                return Commands.Pop.Local(_localSymbols[symbol].Item2);

            if (_argumentSymbols.ContainsKey(symbol))
                return Commands.Pop.Argument(_argumentSymbols[symbol].Item2);

            return _classCompilationUnit.PopSymbol(symbol);
        }
        
        public VaribleType GetSymbolType(string symbol)
        {
            if (!HasSymbol(symbol)) throw new ArgumentException("Unknown symbol");

            if (_argumentSymbols.ContainsKey(symbol))
                return _argumentSymbols[symbol].Item1;

            if (_localSymbols.ContainsKey(symbol))
                return _localSymbols[symbol].Item1;

            return _classCompilationUnit.GetSymbolType(symbol);
        }

        public IEnumerable<Command> FunctionDeclaration()
        {
            yield return Commands.DeclareFunction(Name, _localSymbols.Count);
            
            if (_subroutine is Constructor)
            {
                yield return Commands.Push.Constant(_classCompilationUnit.FieldsCount);
                yield return Call("Memory.alloc", 1);
            }

            if (_subroutine is Method)
                yield return Commands.Push.Argument(0);

            if (_subroutine is Constructor || _subroutine is Method)
                yield return Commands.Pop.Pointer(0);
        }

        public Command Call(string functionName, int argc) =>
            Commands.FunctionCall(functionName, argc, LabelGenerator);
    }
}