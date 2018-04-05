using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class ClassVariableDeclaration : ProgramStructure
    {
        public ClassVariableDeclaration(Tokenizer tokenizer)
        {
            tokenizer
                .ApplyThenMove(AddCurrent) // STATIC OR FIELD
                .CurrentIs(IsTypeDeclaration)
                .ApplyThenMove(AddCurrent)
                .CurrentIsIdentifier()
                .ApplyThenMove(AddCurrent)
                .ApplyWhile(Symbol.Commna, t => t
                    .ApplyThenMove(AddCurrent)
                    .CurrentIsIdentifier()
                    .ApplyThenMove(AddCurrent))
                .CurrentIs(Symbol.SemiColon)
                .ApplyThenMove(AddCurrent);
        }
    }
}