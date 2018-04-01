using System.Collections.Generic;
using System.Text;
using Analyzer.Tokens;

namespace Analyzer
{
    public class Tokenizer
    {
        private LinkedList<Token> _list;
        private LinkedListNode<Token> _node;
        public Token Current => _list.First.Value;
        public Token Next => _list.First.Next?.Value;
        public bool IsEmpty => _list.Count == 0;

        private Tokenizer(){}
        public Tokenizer(IEnumerable<Token> tokens)
        {
            _list = new LinkedList<Token>(tokens);
            _node = _list.First;
        }
        
        public Tokenizer Move()
        {
            return new Tokenizer {_list = _list, _node = _node.Next};
        }
        
        public static Tokenizer Tokenize(string source)
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
                
                if (SymbolToken.IsSymbol(source[i]))
                {
                    tokens.Add(Token.Parse(source[i].ToString()));
                    continue;
                }
                
                var j = i + 1;
                if (source[i] == '"')
                {
                    while (j < source.Length && source[j] != '"') j++;
                    tokens.Add(Token.Parse(source.Substring(i, j - i + 1)));
                    i = j;
                    continue;
                }

                while (j < source.Length && !char.IsWhiteSpace(source[j]) &&
                       source[j] != '"' && !SymbolToken.IsSymbol(source[j]))
                {
                    j++;
                }
                
                tokens.Add(Token.Parse(source.Substring(i, j - i)));
                i = j - 1;
            }
            
            return new Tokenizer(tokens);
        }
    }
}