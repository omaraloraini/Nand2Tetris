using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Analyzer.Expressions;
using Analyzer.ProgramStructures;
using Analyzer.Statements;
using Analyzer.Tokens;

namespace Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please supply a file");
                return;
            }

            var text = File.ReadAllText(args[0]);
            var xml = ParseToXml(text);
            File.WriteAllText(args[0].Replace(".jack", ".xml"), xml.ToString());
        }

        private static XElement ParseToXml(string source)
        {
            var tokenizer = Tokenizer.Tokenize(source);
            var jackProgram = new JackClass(tokenizer);
            return ParseToXml(new List<IToken> {jackProgram});
        }

        private static XElement ParseToXml(ICollection<IToken> tokens)
        {
            if (tokens.Count != 1) throw new ArgumentException();
            if (!(tokens.First() is JackClass)) throw new ArgumentException();
            
            var root = new XElement(nameof(JackClass));
            Parse(((CompositeToken)tokens.First()).Tokens, root);
            return root;

            void Parse(IList<IToken> list, XContainer parent)
            {
                foreach (var iToken in list)
                {
                    switch (iToken)
                    {
                        case CompositeToken compositeToken:
                            var element = new XElement(compositeToken.GetType().Name);
                            Parse(compositeToken.Tokens, element);
                            parent.Add(element);
                            break;
                        case Token token:
                            parent.Add(new XElement(ToCamelCase(token.GetType().Name), token.Value));
                            break;
                    }
                }
            }
        }

        private static string ToCamelCase(string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}