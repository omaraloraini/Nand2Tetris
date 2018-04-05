using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class SubroutineBody : ProgramStructure
    {
        public ICollection<VaribleDeclaration> VaribleDeclarations { get; } = new List<VaribleDeclaration>();
        public SubroutineBody(Tokenizer tokenizer)
        {
            tokenizer
                .CurrentIs(Symbol.OpenCurly)
                .ApplyThenMove(AddCurrent)
                .ApplyWhile(Keyword.Var, t =>
                {
                    var vb = new VaribleDeclaration(t);
                    VaribleDeclarations.Add(vb);
                    Tokens.Add(vb);
                })
                .Apply(AddStatements)
                .CurrentIs(Symbol.CloseCurly)
                .ApplyThenMove(AddCurrent);
        }
    }
}