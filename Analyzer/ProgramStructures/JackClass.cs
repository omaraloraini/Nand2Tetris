using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class JackClass : ProgramStructure
    {
        public JackClass(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Keyword.Class)
                .ApplyThenMove(AddCurrent)
                .CurrentIsIdentifier()
                .ApplyThenMove(AddCurrent)
                .CurrentIs(Symbol.OpenCurly)
                .ApplyThenMove(AddCurrent)
                .Apply(ClassVariableDeclarations)
                .Apply(SubroutineDeclarations)
                .CurrentIs(Symbol.CloseCurly)
                .ApplyThenMove(AddCurrent);
        }

        private void SubroutineDeclarations(Tokenizer tokenizer)
        {
            while (tokenizer.Current is Keyword keyword && (
                       keyword == Keyword.Constructor || keyword == Keyword.Function || keyword == Keyword.Method))
            {
                Tokens.Add(new SubroutineDeclaration(tokenizer));
            }
        }
        
        private void ClassVariableDeclarations(Tokenizer tokenizer)
        {
            while (tokenizer.Current is Keyword keyword && (keyword == Keyword.Static || keyword == Keyword.Field))
            {
                Tokens.Add(new ClassVariableDeclaration(tokenizer));
            }
        }
    }
}