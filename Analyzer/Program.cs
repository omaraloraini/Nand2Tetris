using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Analyzer.Expressions;
using Analyzer.Statements;
using Analyzer.Tokens;

namespace Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokenizer = Tokenizer.Tokenize(
                "do Output.printInt(sum / length);");
            
            var subroutineCall = new DoStatement(tokenizer);
            
            Print(subroutineCall, 0);
        }

        private static void Print(IToken token, int i)
        {
            switch (token)
            {
                case CompositeToken compositeToken:
                    Console.WriteLine($"{new string(' ', i*2)} {compositeToken.GetType().Name}");
                    compositeToken.Tokens.ForEach(t => Print(t, i + 1));
                    break;
                case Token token1:
                    Console.WriteLine($"{new string(' ', i*2)} {token1}");
                    break;
            }
        }

        private static XElement ParseToXml(string source)
        {
            var tokenizer = Tokenizer.Tokenize(source);
            var jackProgram = new JackProgram(tokenizer);
            return ParseToXml(null, jackProgram.Tokens, 0);
        }

        private static XElement ParseToXml(XElement xElement, IList<IToken> tokens, int i)
        {
            if (tokens.Count == i) return xElement;
            
            switch (tokens[i])
            {
                case JackProgram jackProgram:
                    var x = new XElement("class");
                    return ParseToXml(x, tokens, i + 1);
                case DoStatement doStatement:
                    break;
                case IfStatemenst ifStatemenst:
                    break;
                case LetStatement letStatement:
                    break;
                case ReturntStatment returntStatment:
                    break;
                case WhileStatement whileStatement:
                    break;
                case Term term:
                    break;
                case ExpressionList expressionList:
                    break;
                case SubroutineCall subroutineCall:
                    return ParseToXml(new XElement("expression"), subroutineCall.Tokens, 0);
                case Expression expression:
                    return ParseToXml(new XElement("expression"), expression.Tokens, 0);
                case IdentifierToken identifierToken:
                    xElement.Add("identifier", identifierToken.Identifier);
                    return xElement;
                case IntegerToken integerToken:
                    xElement.Add("integerConstant", integerToken.Integer);
                    return xElement;
                case KeywordToken keywordToken:
                    xElement.Add("keyword", keywordToken.Keyword);
                    return xElement;
                case StringToken stringToken:
                    xElement.Add("stringConstant", stringToken.String);
                    return xElement;
                case SymbolToken symbolToken:
                    xElement.Add("symbol", symbolToken.Symbol);
                    return xElement;
            }
            
            throw new InvalidOperationException();
        }
    }
}