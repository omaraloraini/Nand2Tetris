using System;
using System.Collections.Generic;
using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public abstract class Statement : CompositeToken
    {
        public static Statement Parse(Tokenizer tokenizer)
        {
            if (tokenizer.Current is KeywordToken keyword)
            {
                if (keyword == KeywordToken.Let)
                    return new LetStatement(tokenizer);
                if (keyword == KeywordToken.If)
                    return new IfStatemenst(tokenizer);
                if (keyword == KeywordToken.While)
                    return new WhileStatement(tokenizer);
                if (keyword == KeywordToken.Do)
                    return new DoStatement(tokenizer);
                if (keyword == KeywordToken.Return)
                    return new ReturntStatment(tokenizer);
            }
            
            throw new ArgumentException($"Expected a keyword, found : {tokenizer.Current}");
        }

        public static bool CanParse(Tokenizer tokenizer)
        {
            if (!(tokenizer.Current is KeywordToken keyword)) return false;

            return keyword == KeywordToken.Let ||
                   keyword == KeywordToken.If ||
                   keyword == KeywordToken.While ||
                   keyword == KeywordToken.Do ||
                   keyword == KeywordToken.Return;
        }
        
        public static IEnumerable<Statement> ParseStatements(Tokenizer tokenizer)
        {
            while (CanParse(tokenizer))
            {
                yield return Parse(tokenizer);
            }
        }
    }
}