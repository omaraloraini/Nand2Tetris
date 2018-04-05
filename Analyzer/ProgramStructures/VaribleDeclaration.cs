using System.Collections.Generic;
using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class VaribleDeclaration : ProgramStructure
    {
        public ICollection<string> Varibles { get; } = new List<string>(); 
        public VaribleDeclaration(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Keyword.Var)
                .ApplyThenMove(AddCurrent) // VAR
                .CurrentIs(IsTypeDeclaration)
                .ApplyThenMove(AddCurrent)
                .CurrentIsIdentifier()
                .Apply(t => Varibles.Add(t.Current.Value))
                .ApplyThenMove(AddCurrent)
                .ApplyIf(Symbol.Commna, s => s
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .Apply(t => Varibles.Add(t.Current.Value))
                    .ApplyThenMove(AddCurrent))
                .CurrentIs(Symbol.SemiColon)
                .ApplyThenMove(AddCurrent);
        }
    }
}