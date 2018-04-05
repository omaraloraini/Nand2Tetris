using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public abstract class ProgramStructure : CompositeToken
    {
        protected static bool IsTypeDeclaration(Token token)
        {
            return
                token is Identifier ||
                token is Keyword k && (
                    k == Keyword.Int || k == Keyword.Char || k == Keyword.Boolean);
        }
    }
}