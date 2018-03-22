using System.Collections.Generic;
using System.Linq;

namespace Analyzer
{
    public class CompositeToken : IToken
    {
        public string Name { get; }
        public IEnumerable<IToken> Tokens { get; } = new List<IToken>();

        public CompositeToken(string name)
        {
            Name = name;
        }
    }
}