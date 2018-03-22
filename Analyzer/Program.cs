using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = "if (x < 100) {" +
                         "    x = x * 2 + 1;" +
                         "}";

            var tokens = Tokenizer.Tokenize(source);

            var xDocument = new XDocument(
                new XElement("tokens",
                    tokens
                        .Select(t => 
                            new XElement(
                                t.GetType().Name.Replace("Token", "").ToLower(),
                                t))
                )
            );


            Console.WriteLine(xDocument);
        }
        
    }
}