using System;
using System.Collections.Generic;
using System.Linq;
using Analyzer;
using Analyzer.Expressions;
using Analyzer.ProgramStructures;
using Analyzer.Statements;
using Analyzer.Tokens;
using VirtualMachine;

namespace Compiler
{
    public static class JackCompiler
    {
        public static IEnumerable<Command> Compile(string source)
        {
            var jackClass = new JackClass(Tokenizer.Tokenize(source));
            return CompileClass(new JackClassCompilationUnit(jackClass));
        }
        
        private static IEnumerable<Command> CompileClass(JackClassCompilationUnit compilationUnit)
        {
            return compilationUnit.SubroutineCompilationUnits
                .SelectMany(CompileSubroutine);
        }

        private static IEnumerable<Command> CompileSubroutine(SubroutineCompilationUnit compilationUnit)
        {
            foreach (var command in compilationUnit.FunctionDeclaration())
            {
                yield return command;
            }

            foreach (var command in CompileStatements(compilationUnit, compilationUnit.Statements))
            {
                yield return command;
            }
        }

        private static IEnumerable<Command> CompileStatements(SubroutineCompilationUnit compilationUnit,
            IEnumerable<Statement> statements)
        {
            return statements.SelectMany(s => CompileStatement(compilationUnit, s));
        }

        private static IEnumerable<Command> CompileStatement(
            SubroutineCompilationUnit compilationUnit, Statement statement)
        {
            switch (statement)
            {
                case DoStatement doStatement: 
                    return CompileDoStatement(compilationUnit, doStatement);
                case IfStatement ifStatement:
                    return CompileIfStatement(compilationUnit, ifStatement);
                case LetStatement letStatement:
                    return CompileLetStatement(compilationUnit, letStatement);
                case ReturntStatment returntStatment:
                    return CompileReturnStatement(compilationUnit, returntStatment);
                case WhileStatement whileStatement:
                    return CompileWhileStatement(compilationUnit, whileStatement);
            }
            
            throw new InvalidOperationException();
        }

        private static IEnumerable<Command> CompileWhileStatement(SubroutineCompilationUnit compilationUnit,
            WhileStatement whileStatement)
        {
            var label1 = compilationUnit.LabelGenerator.Generate("while");
            var label2 = compilationUnit.LabelGenerator.Generate("while");

            yield return Commands.Label(label1);

            foreach (var command in CompileExpression(compilationUnit, whileStatement.Condition))
            {
                yield return command;
            }

            yield return Commands.Bitwise.Not();
            yield return Commands.IfGoto(label2);

            foreach (var command in CompileStatements(compilationUnit, whileStatement.Statements))
                yield return command;

            yield return Commands.Goto(label1);

            yield return Commands.Label(label2);
        }

        private static IEnumerable<Command> CompileReturnStatement(SubroutineCompilationUnit compilationUnit,
            ReturntStatment returntStatment)
        {
            if (returntStatment.Expression == null)
                yield return Commands.Push.Constant(0);
            else 
                foreach (var command in CompileExpression(compilationUnit, returntStatment.Expression)) 
                    yield return command;
            
            yield return Commands.FunctionRetrun();
        }

        private static IEnumerable<Command> CompileLetStatement(SubroutineCompilationUnit compilationUnit,
            LetStatement letStatement)
        {
            foreach (var command in CompileExpression(compilationUnit, letStatement.Expression))
            {
                yield return command;
            }
            
            switch (letStatement.Varible)
            {
                case ArrayIdentifier arrayIdentifier:
                    yield return Commands.Pop.Temp(0);
                    yield return compilationUnit.PushSymbol(arrayIdentifier.Identifier.Value);
                    foreach (var command in CompileExpression(compilationUnit, arrayIdentifier.Expression))
                        yield return command;
                    yield return Commands.Arithmetic.Add();
                    yield return Commands.Pop.Pointer(1);
                    yield return Commands.Push.Temp(0);
                    yield return Commands.Pop.That(0);
                    break;
                case Identifier identifier:
                    yield return compilationUnit.PopSymbol(identifier.Value);
                    break;
            }
        }

        private static IEnumerable<Command> CompileIfStatement(SubroutineCompilationUnit compilationUnit,
            IfStatement ifStatement)
        {
            var label1 = compilationUnit.LabelGenerator.Generate("if");
            var label2 = compilationUnit.LabelGenerator.Generate("if");
            
            foreach (var command in CompileExpression(compilationUnit, ifStatement.Condition))
                yield return command;
            
            yield return Commands.Bitwise.Not();
            yield return Commands.IfGoto(label1);
            
            foreach (var command in CompileStatements(compilationUnit, ifStatement.TrueBranch))
                yield return command;

            yield return Commands.Goto(label2);
            yield return Commands.Label(label1);

            foreach (var command in CompileStatements(compilationUnit, ifStatement.FalseBranch))
                yield return command;

            yield return Commands.Label(label2);
        }

        private static IEnumerable<Command> CompileDoStatement(SubroutineCompilationUnit compilationUnit,
            DoStatement doStatement)
        {
            foreach (var command in CompileSubroutineCall(compilationUnit, doStatement.SubroutineCall))
            {
                yield return command;
            }
            
            yield return Commands.Pop.Temp(0);
        }

        public static IEnumerable<Command> CompileExpression(
            SubroutineCompilationUnit compilationUnit, Expression expression)
        {
            foreach (var command in CompileTerm(compilationUnit, expression.Term))
            {
                yield return command;
            }

            TFirst First<TFirst, TSecond>((TFirst, TSecond) tuple) => tuple.Item1;
            TSecond Second<TFirst, TSecond>((TFirst, TSecond) tuple) => tuple.Item2;
            
            foreach (var command in expression.OperatorTermPairs
                .Select(Second)
                .SelectMany(t => CompileTerm(compilationUnit, t)))
            {
                yield return command;
            }
            
            foreach (var command in expression.OperatorTermPairs
                .Select(First)
                .Reverse()
                .Select(o => CompileOperator(compilationUnit, o)))
            {
                yield return command;
            }
        }

        private static IEnumerable<Command> CompileTerm(SubroutineCompilationUnit compilationUnit, ITerm term)
        {
            switch (term)
            {
                case Expression expression:
                    return CompileExpression(compilationUnit, expression);
                case ArrayIdentifier arrayIdentifier:
                    return CompileArrayIdentifier(compilationUnit, arrayIdentifier);
                case SubroutineCall subroutineCall:
                    return CompileSubroutineCall(compilationUnit, subroutineCall);
                case UnaryTerm unaryTerm:
                    return CompileUnaryTerm(compilationUnit, unaryTerm);
                case Identifier identifier:
                    return new[] {compilationUnit.PushSymbol(identifier.Value)};
                case IntegerConstant integerConstant:
                    return new[] {Commands.Push.Constant(integerConstant.Integer)};
                case KeywordConstant keywordConstant:
                    return CompileKeywordConstant(keywordConstant);
                case StringConstant stringConstant:
                    return CompileString(compilationUnit, stringConstant);
            }
            
            throw new InvalidOperationException();
        }

        private static IEnumerable<Command> CompileString(SubroutineCompilationUnit compilationUnit,
            StringConstant stringConstant)
        {
            yield return Commands.Push.Constant(stringConstant.Value.Length);
            yield return compilationUnit.Call("String.new", 1);
            
            foreach (var c in stringConstant.Value)
            {
                yield return Commands.Push.Constant(c);
                yield return compilationUnit.Call("String.appendChar", 2);
            }   
        }

        private static IEnumerable<Command> CompileArrayIdentifier(SubroutineCompilationUnit compilationUnit,
            ArrayIdentifier arrayIdentifier)
        {
            yield return compilationUnit.PushSymbol(arrayIdentifier.Identifier.Value);
            
            foreach (var command in CompileExpression(compilationUnit, arrayIdentifier.Expression))
                yield return command;

            yield return Commands.Arithmetic.Add();
            yield return Commands.Pop.Pointer(1);
            yield return Commands.Push.That(0);
        }

        private static Command CompileOperator(SubroutineCompilationUnit compilationUnit, Symbol @operator)
        {
            if (@operator == Symbol.Plus) return Commands.Arithmetic.Add();
            if (@operator == Symbol.Minus) return Commands.Arithmetic.Sub();
            
            if (@operator == Symbol.Ampersand) return Commands.Bitwise.And();
            if (@operator == Symbol.Pipe) return Commands.Bitwise.Or();
            
            if (@operator == Symbol.Equal) return Commands.Comparsion.Equal(compilationUnit.LabelGenerator);
            if (@operator == Symbol.GreaterThan) return Commands.Comparsion.GreateThan(compilationUnit.LabelGenerator);
            if (@operator == Symbol.LessThan) return Commands.Comparsion.LessThan(compilationUnit.LabelGenerator);
            
            if (@operator == Symbol.Star) return compilationUnit.Call("Math.multiply", 2);
            if (@operator == Symbol.Slash) return compilationUnit.Call("Math.divide", 2);

            throw new ArgumentException("Unknown operator");
        }

        private static IEnumerable<Command> CompileUnaryTerm(SubroutineCompilationUnit compilationUnit,
            UnaryTerm unaryTerm)
        {
            foreach (var command in CompileTerm(compilationUnit, unaryTerm.Term))
                yield return command;
            
            if (unaryTerm.Symbol == Symbol.Minus) yield return Commands.Arithmetic.Neg();
            if (unaryTerm.Symbol == Symbol.Tilde) yield return Commands.Bitwise.Not();
        }

        private static IEnumerable<Command> CompileKeywordConstant(KeywordConstant constant)
        {
            if (constant == KeywordConstant.Null || constant == KeywordConstant.False)
            {
                yield return Commands.Push.Constant(0);
            }
            else if(constant == KeywordConstant.True)
            {
                yield return Commands.Push.Constant(1);
                yield return Commands.Arithmetic.Neg();
            }
            else
            {
                yield return Commands.Push.Pointer(0);
            }
        }

        private static IEnumerable<Command> CompileSubroutineCall(
            SubroutineCompilationUnit compilationUnit, SubroutineCall call)
        {
            var name = call.CalledOn is null
                ? $"{compilationUnit.ClassName}.{call.SobroutineName.Value}"
                : compilationUnit.HasSymbol(call.CalledOn.Value)
                    ? $"{compilationUnit.GetSymbolType(call.CalledOn.Value).Name}.{call.SobroutineName.Value}"
                    : $"{call.CalledOn.Value}.{call.SobroutineName.Value}";

            var argc = call.Parametars.Count;
            
            if (call.CalledOn is null)
            {
                yield return Commands.Push.Pointer(0);
                argc++;
            }
            else if (compilationUnit.HasSymbol(call.CalledOn.Value))
            {
                yield return compilationUnit.PushSymbol(call.CalledOn.Value);
                argc++;
            }
            
            foreach (var command in call
                .Parametars
                .SelectMany(e => CompileExpression(compilationUnit, e)))
            {
                yield return command;
            }

            yield return compilationUnit.Call(name, argc);
        }
    }
}