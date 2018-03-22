using System;
using System.Collections.Generic;

namespace Analyzer
{
    internal class Token
    {
        public Token(TokenType type, TokenName name, string value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public TokenType Type { get; }
        public TokenName Name { get; }
        public string Value { get; }

        private static Token Identifier(string value)
        {
            return new Token(TokenType.Identifier, TokenName.Identifier, value);
        }

        private static Token IntegerConstant(string value)
        {
            return new Token(TokenType.IntegerConstant, 
                TokenName.IntegerConstant, value);
        }

        public static Token StringConsant(string value)
        {
            return new Token(TokenType.StringConstant, 
                TokenName.StringConstant, value);
        }

        public static Token Parse(string value)
        {
            if (int.TryParse(value, out _)) return IntegerConstant(value);

            if (_keyordTokens.ContainsKey(value))
                return _keyordTokens[value];

            if (value.Length == 1 && _symbolTokens.ContainsKey(value[0]))
                return _symbolTokens[value[0]];

            return Identifier(value);
        }

        public static Token Symbol(char c)
        {
            if (!IsSymbol(c)) throw new InvalidOperationException("Not a symbol");
            return _symbolTokens[c];
        }

        public static bool IsSymbol(char c) => _symbolTokens.ContainsKey(c);

        internal enum TokenType
        {
            Keyword,
            Symbol,
            Identifier,
            IntegerConstant,
            StringConstant
        }
        
        internal enum TokenName
        {
            Class,
            Constructor,
            Function,
            Method,
            Field,
            Static,
            Var,
            Int,
            Char,
            Boolean,
            Void,
            True,
            Fasle,
            Null,
            This,
            Let,
            Do,
            If,
            Else,
            While,
            Return,
            OpenCurly,
            CloseCurly,
            OpenParenthesis,
            CloseParenthesis,
            OpenBracket,
            CloseBracket,
            Dot,
            Commna,
            SemiColon,
            Plus,
            Minus,
            Star,
            Slash,
            Ampersand,
            Pipe,
            LessThan,
            GreaterThan,
            Equal,
            Tilde,
            IntegerConstant,
            StringConstant,
            Identifier
        }

        
        private static Dictionary<string, Token> _keyordTokens = 
            new Dictionary<string, Token>
            {
                ["class"] = new Token(TokenType.Keyword, TokenName.Class, "class"),
                ["constructor"] = new Token(TokenType.Keyword, TokenName.Constructor, "constructor"),
                ["function"] = new Token(TokenType.Keyword, TokenName.Function, "function"),
                ["method"] = new Token(TokenType.Keyword, TokenName.Method, "method"),
                ["field"] = new Token(TokenType.Keyword, TokenName.Field, "field"),
                ["static"] = new Token(TokenType.Keyword, TokenName.Static, "static"),
                ["var"] = new Token(TokenType.Keyword, TokenName.Var, "var"),
                ["int"] = new Token(TokenType.Keyword, TokenName.Int, "int"),
                ["char"] = new Token(TokenType.Keyword, TokenName.Char, "char"),
                ["boolean"] = new Token(TokenType.Keyword, TokenName.Boolean, "boolean"),
                ["void"] = new Token(TokenType.Keyword, TokenName.Void, "void"),
                ["true"] = new Token(TokenType.Keyword, TokenName.True, "true"),
                ["false"] = new Token(TokenType.Keyword, TokenName.Fasle, "false"),
                ["null"] = new Token(TokenType.Keyword, TokenName.Null, "null"),
                ["this"] = new Token(TokenType.Keyword, TokenName.This, "this"),
                ["let"] = new Token(TokenType.Keyword, TokenName.Let, "let"),
                ["do"] = new Token(TokenType.Keyword, TokenName.Do, "do"),
                ["if"] = new Token(TokenType.Keyword, TokenName.If, "if"),
                ["else"] = new Token(TokenType.Keyword, TokenName.Else, "else"),
                ["while"] = new Token(TokenType.Keyword, TokenName.While, "while"),
                ["return"] = new Token(TokenType.Keyword, TokenName.Return, "return"),
            };
        
        private static Dictionary<char, Token> _symbolTokens = 
            new Dictionary<char,Token>
            {
                ['{'] = new Token(TokenType.Symbol, TokenName.OpenCurly, "{"),
                ['}'] = new Token(TokenType.Symbol, TokenName.CloseCurly, "}"),
                ['('] = new Token(TokenType.Symbol, TokenName.OpenParenthesis, "("),
                [')'] = new Token(TokenType.Symbol, TokenName.CloseParenthesis, ")"),
                ['['] = new Token(TokenType.Symbol, TokenName.OpenBracket, "["),
                [']'] = new Token(TokenType.Symbol, TokenName.CloseBracket, "]"),
                ['.'] = new Token(TokenType.Symbol, TokenName.Dot, "."),
                [','] = new Token(TokenType.Symbol, TokenName.Commna, ","),
                [';'] = new Token(TokenType.Symbol, TokenName.SemiColon, ";"),
                ['+'] = new Token(TokenType.Symbol, TokenName.Plus, "+"),
                ['-'] = new Token(TokenType.Symbol, TokenName.Minus, "-"),
                ['*'] = new Token(TokenType.Symbol, TokenName.Star, "*"),
                ['/'] = new Token(TokenType.Symbol, TokenName.Slash, "/"),
                ['&'] = new Token(TokenType.Symbol, TokenName.Ampersand, "&"),
                ['|'] = new Token(TokenType.Symbol, TokenName.Pipe, "|"),
                ['>'] = new Token(TokenType.Symbol, TokenName.GreaterThan, ">"),
                ['<'] = new Token(TokenType.Symbol, TokenName.LessThan, "<"),
                ['='] = new Token(TokenType.Symbol, TokenName.Equal, "="),
                ['~'] = new Token(TokenType.Symbol, TokenName.Tilde, "~"),
            };


    }
}