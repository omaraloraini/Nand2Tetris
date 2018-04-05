using Analyzer.Tokens;

namespace Analyzer.Statements
{
    public class Statements : CompositeToken
    {
        public Statements(Tokenizer tokenizer)
        {
            while (Statement.CanParse(tokenizer))
            {
                Tokens.Add(Statement.Parse(tokenizer));
            }
        }
    }
}