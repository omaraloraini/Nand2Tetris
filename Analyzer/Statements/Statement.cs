using System;
using System.Collections.Generic;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public abstract class Statement
    {
        public static Statement Parse(Tokenizer tokenizer)
        {
            if (tokenizer.Current is Keyword keyword)
            {
                if (keyword == Keyword.Let)
                    return new LetStatement(tokenizer);
                if (keyword == Keyword.If)
                    return new IfStatement(tokenizer);
                if (keyword == Keyword.While)
                    return new WhileStatement(tokenizer);
                if (keyword == Keyword.Do)
                    return new DoStatement(tokenizer);
                if (keyword == Keyword.Return)
                    return new ReturntStatment(tokenizer);
            }
            
            throw new ArgumentException($"Expected a keyword, found : {tokenizer.Current}");
        }

        public static bool CanParse(Tokenizer tokenizer)
        {
            if (!(tokenizer.Current is Keyword keyword)) return false;

            return keyword == Keyword.Let ||
                   keyword == Keyword.If ||
                   keyword == Keyword.While ||
                   keyword == Keyword.Do ||
                   keyword == Keyword.Return;
        }

        public static IEnumerable<Statement> ParseStatements(Tokenizer tokenizer)
        {
            var statements = new List<Statement>();
            while (CanParse(tokenizer))
                statements.Add(Parse(tokenizer));
            return statements;
        }
    }
}