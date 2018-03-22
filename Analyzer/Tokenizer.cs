using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    internal static class Tokenizer
    {     
        public static IEnumerable<Token> Tokenize(string source)
        {
            var tokens = new List<Token>();

            for (var i = 0; i < source.Length; i++)
            {
                if (char.IsWhiteSpace(source[i])) continue;
                
                if (source[i] == '/' && source[i + 1] == '/')
                {
                    while (source[i] != '\n') i++;
                    continue;
                }
                
                if (source[i] == '/' && source[i + 1] == '*')
                {
                    while (source[i] != '*' || source[i + 1] != '/') i++;
                    i++;
                    continue;
                }
                
                if (Token.IsSymbol(source[i]))
                {
                    tokens.Add(Token.Symbol(source[i]));
                    continue;
                }
                
                var j = i + 1;
                if (source[i] == '"')
                {
                    while (j < source.Length && source[j] != '"') j++;
                    tokens.Add(Token.StringConsant(source.Substring(i + 1, j - i - 1)));
                    i = j;
                    continue;
                }

                while (j < source.Length && !char.IsWhiteSpace(source[j]) &&
                       source[j] != '"' && !Token.IsSymbol(source[j]))
                {
                    j++;
                }
                
                tokens.Add(Token.Parse(source.Substring(i, j - i)));
                i = j - 1;
            }
            
            return tokens;
        }
    }
}