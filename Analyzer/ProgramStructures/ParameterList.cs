using System.Collections.Generic;
using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class ParameterList : ProgramStructure
    {
        public ICollection<string> Paramerers { get; } = new List<string>();
        public ParameterList(Tokenizer tokenizer)
        {
            while (!tokenizer.Current.Equals(Symbol.CloseParenthesis))
            {
                tokenizer
                    .CurrentIs(IsTypeDeclaration)
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .Apply(t => Paramerers.Add(t.Current.Value))
                    .ApplyThenMove(AddCurrent)
                    .ApplyIf(Symbol.Commna, t => t
                        .ApplyThenMove(AddCurrent));
            }
        }
    }
}