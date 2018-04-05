using System;
using System.Linq;
using Analyzer.Tokens;

namespace Analyzer
{
    public static class TokenizerExtensions
    {
        public static Tokenizer CurrentIs(this Tokenizer tokenizer, Token token)
        {
            if (!tokenizer.Current.Equals(token))
            {
                throw new ArgumentException($"Expected {token}, Found {tokenizer.Current}");
            }

            return tokenizer;
        }
        
        public static Tokenizer CurrentIs(this Tokenizer tokenizer, 
            Func<Token, bool> predicate)
        {
            if (!predicate(tokenizer.Current))
            {
                throw new ArgumentException($"Unexpected token: {tokenizer.Current}");
            }

            return tokenizer;
        }

        public static Tokenizer CurrentOfType(this Tokenizer tokenizer, Type type)
        {
            if (tokenizer.Current.GetType() != type)
            {
                throw new ArgumentException($"Expected {type}," +
                                            $" Found {tokenizer.Current.GetType()}");
            }

            return tokenizer;
        }

        public static Tokenizer CurrentIsKeyword(this Tokenizer tokenizer) =>
            CurrentOfType(tokenizer, typeof(Keyword));
        public static Tokenizer CurrentIsSymbol(this Tokenizer tokenizer) =>
            CurrentOfType(tokenizer, typeof(Symbol));
        public static Tokenizer CurrentIsIntegerConstant(this Tokenizer tokenizer) =>
            CurrentOfType(tokenizer, typeof(IntegerConstant));
        public static Tokenizer CurrentIsStringConstant(this Tokenizer tokenizer) =>
            CurrentOfType(tokenizer, typeof(StringConstant));
        public static Tokenizer CurrentIsIdentifier(this Tokenizer tokenizer) =>
            CurrentOfType(tokenizer, typeof(Identifier));

        public static Tokenizer ApplyThenMove(this Tokenizer tokenizer, Action<Tokenizer> action)
        {
            action(tokenizer);
            return tokenizer.Move();
        }
        
        public static Tokenizer ApplyIf(this Tokenizer tokenizer,Token token,
            Action<Tokenizer> action)
        {
            if (token.Equals(tokenizer.Current))
                action(tokenizer);
            return tokenizer;
        }

        public static Tokenizer ApplyIfNot(this Tokenizer tokenizer, Token token,
            Action<Tokenizer> action)
        {
            if (!token.Equals(tokenizer.Current))
                action(tokenizer);
            return tokenizer;
        }

        public static Tokenizer ApplyWhile(this Tokenizer tokenizer, Token token,
            Action<Tokenizer> action)
        {
            while (token.Equals(tokenizer.Current))
            {
                action(tokenizer);
            }

            return tokenizer;
        }

        public static T Apply<T>(this T t, Action<T> action)
        {
            action(t);
            return t;
        }
    }
}