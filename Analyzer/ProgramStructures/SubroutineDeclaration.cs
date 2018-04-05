using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{  
    public class SubroutineDeclaration : ProgramStructure
    {
        public SubroutineDeclaration(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(t =>
                    t is Keyword k && ( k == Keyword.Constructor || k == Keyword.Function || k == Keyword.Method))
                .ApplyThenMove(AddCurrent)
                .CurrentIs(t =>
                    t is Keyword k && k == Keyword.Void ||IsTypeDeclaration(t))
                .ApplyThenMove(AddCurrent)
                .CurrentIsIdentifier()
                .ApplyThenMove(AddCurrent)
                .CurrentIs(Symbol.OpenParenthesis)
                .ApplyThenMove(AddCurrent)
                .Apply(t => Tokens.Add(new ParameterList(t)))
                .CurrentIs(Symbol.CloseParenthesis)
                .ApplyThenMove(AddCurrent)
                .Apply(t => Tokens.Add(new SubroutineBody(t)));
            
        }
    }
}