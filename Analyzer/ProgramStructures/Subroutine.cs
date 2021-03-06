﻿using System;
using System.Collections.Generic;
using System.Linq;
using Analyzer.Statements;
using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public abstract class Subroutine
    {
        public Identifier Name { get; }
        public VaribleType ReturnType { get; }
        public IReadOnlyList<Varible> Arguments { get; }
        public IReadOnlyList<Varible> LocalVaribles { get; }
        public IEnumerable<Statement> Statements { get; }
        public bool IsVoid => ReturnType.Name == "void";

        protected Subroutine(Identifier name,
            VaribleType returnType,
            IEnumerable<Varible> arguments,
            IEnumerable<Varible> localVaribles,
            IEnumerable<Statement> statements)
        {
            Name = name;
            ReturnType = returnType;
            Arguments = arguments.ToList();
            LocalVaribles = localVaribles.ToList();
            Statements = statements;
        }

        public static Subroutine Parse(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(t =>
                t is Keyword k && (
                    k == Keyword.Constructor ||
                    k == Keyword.Function ||
                    k == Keyword.Method));

            var type = tokenizer.GetCurrentThenMove() as Keyword;

            tokenizer.CurrentIs(VaribleType.IsValid);
            var returnType = new VaribleType(tokenizer.GetCurrentThenMove());
            tokenizer.CurrentIsIdentifier();
            var name = tokenizer.GetCurrentThenMove() as Identifier;

            var parameters = ParseParameters(tokenizer);
            
            tokenizer.CurrentIs(Symbol.OpenCurly).Move();
            var localVaribles = ParseLocalVaribles(tokenizer);
            var statements = Statement.ParseStatements(tokenizer);
            tokenizer.CurrentIs(Symbol.CloseCurly).Move();
            
            if (type == Keyword.Constructor)
                return new Constructor(name, returnType, parameters, localVaribles, statements);
            if (type == Keyword.Method)
                return new Method(name, returnType, parameters, localVaribles, statements);
            return new Function(name, returnType, parameters, localVaribles, statements);
        }

        private static IEnumerable<Varible> ParseParameters(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Symbol.OpenParenthesis).Move();
            var parameters = new List<Varible>();
            while (!tokenizer.Current.Equals(Symbol.CloseParenthesis))
            {
                tokenizer.CurrentIs(VaribleType.IsValid);
                parameters.Add(new Varible(
                    new VaribleType(tokenizer.GetCurrentThenMove()),
                    tokenizer.GetCurrentThenMove() as Identifier));

                if (tokenizer.Current.Equals(Symbol.Commna)) tokenizer.Move();
            }

            tokenizer.Move(); // )
            return parameters;
        }

        private static IEnumerable<Varible> ParseLocalVaribles(Tokenizer tokenizer)
        {
            var varibles = new List<Varible>();
            while (tokenizer.Current is Keyword keyword && keyword == Keyword.Var)
            {
                tokenizer.Move().CurrentIs(VaribleType.IsValid);
                var type = new VaribleType(tokenizer.GetCurrentThenMove());

                tokenizer.CurrentIsIdentifier();
                varibles.Add(new Varible(type, tokenizer.GetCurrentThenMove() as Identifier));

                while (tokenizer.Current.Equals(Symbol.Commna))
                {
                    tokenizer.Move().CurrentIsIdentifier();
                    varibles.Add(new Varible(type, tokenizer.GetCurrentThenMove() as Identifier));
                }

                tokenizer.CurrentIs(Symbol.SemiColon).Move();
            }

            return varibles;
        }
    }

    public class Constructor : Subroutine
    {
        public Constructor(Identifier name, VaribleType returnType, IEnumerable<Varible> arguments,
            IEnumerable<Varible> localVaribles, IEnumerable<Statement> statements) : base(name, returnType, arguments,
            localVaribles, statements)
        {
        }
    }

    public class Function : Subroutine
    {
        public Function(Identifier name, VaribleType returnType, IEnumerable<Varible> arguments,
            IEnumerable<Varible> localVaribles, IEnumerable<Statement> statements) : base(name, returnType, arguments,
            localVaribles, statements)
        {
        }
    }

    public class Method : Subroutine
    {
        public Method(Identifier name, VaribleType returnType, IEnumerable<Varible> arguments,
            IEnumerable<Varible> localVaribles, IEnumerable<Statement> statements) : base(name, returnType, arguments,
            localVaribles, statements)
        {
        }
    }
}