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
        }

        private static XElement ParseToXml(string source)
        {
            var tokenizer = Tokenizer.Tokenize(source);
            var jackProgram = new JackProgram(tokenizer);
            var xElement = new XElement("class");
            ParseToXml(jackProgram.Tokens, xElement);
            return xElement;
        }

        private static void ParseToXml(IList<IToken> tokens, XElement xElement)
        {
            foreach (var iToken in tokens)
            {
                XElement element;
                switch (iToken)
                {
                    case JackProgram.ClassVariableDeclaration variableDeclaration:
                        element = new XElement("classVarDec", string.Empty);
                        ParseToXml(variableDeclaration.Tokens, element);
                        xElement.Add(element);
                        break;
                    case JackProgram.SubroutineBody subroutineBody:
                        element = new XElement("subroutineBody", string.Empty);
                        ParseToXml(subroutineBody.Tokens, element);
                        xElement.Add(element);
                        break;
                    case JackProgram.SubroutineDeclaration subroutineDeclaration:
                        element = new XElement("subroutineDec", string.Empty);
                        ParseToXml(subroutineDeclaration.Tokens, element);
                        xElement.Add(element);
                        break;
                    case JackProgram.VaribleDeclaration varibleDeclaration:
                        element = new XElement("varDec", string.Empty);
                        ParseToXml(varibleDeclaration.Tokens, element);
                        xElement.Add(element);
                        break;
                    case CompositeToken compositeToken:
                        element = new XElement(ToCamelCase(compositeToken.GetType().Name), string.Empty);
                        ParseToXml(compositeToken.Tokens, element);
                        xElement.Add(element);
                        break;
                    case Token token:
                        xElement.Add(new XElement(ToCamelCase(token.GetType().Name), token.Value));
                        break;
                }
            }
        }

        private static string ToCamelCase(string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}