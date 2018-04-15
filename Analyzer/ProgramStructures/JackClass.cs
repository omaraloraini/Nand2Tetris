using System;
using System.Collections.Generic;
using Analyzer.Statements;
using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class JackClass
    {
        public Identifier ClassName { get; set; }
        public List<Varible> Fields { get; } = new List<Varible>();
        public List<Varible> StaticFields { get; } = new List<Varible>();
        public IEnumerable<Subroutine> Subroutines { get; set; }

        public JackClass(Tokenizer tokenizer)
        {
            tokenizer.CurrentIs(Keyword.Class).Move().CurrentIsIdentifier();
            ClassName = tokenizer.GetCurrentThenMove() as Identifier;
            tokenizer.CurrentIs(Symbol.OpenCurly).Move();
            AddClassLevelVaribles(tokenizer);
            Subroutines = ParseSubroutine(tokenizer);
        }

        private static IEnumerable<Subroutine> ParseSubroutine(Tokenizer tokenizer)
        {
            while (tokenizer.Current is Keyword keyword && (
                       keyword == Keyword.Constructor || keyword == Keyword.Function || keyword == Keyword.Method))
            {
                yield return Subroutine.Parse(tokenizer);
            }
        }
        
        private void AddClassLevelVaribles(Tokenizer tokenizer)
        {
            while (tokenizer.Current is Keyword keyword && (
                       keyword == Keyword.Static || keyword == Keyword.Field))
            {
                tokenizer.Move().CurrentIs(VaribleType.IsValid);
                var type = new VaribleType(tokenizer.GetCurrentThenMove());

                tokenizer.CurrentIsIdentifier();
                AddVarible(keyword, 
                    new Varible(type, tokenizer.GetCurrentThenMove() as Identifier));

                while (tokenizer.Current.Equals(Symbol.Commna))
                {
                    tokenizer.Move().CurrentIsIdentifier();
                    AddVarible(keyword, 
                        new Varible(type, tokenizer.GetCurrentThenMove() as Identifier));
                }

                tokenizer.CurrentIs(Symbol.SemiColon).Move();
            }

            void AddVarible(Keyword keyword, Varible varible)
            {
                if (keyword == Keyword.Field)
                    Fields.Add(varible);
                else
                    StaticFields.Add(varible);
            }
        }
    }
}