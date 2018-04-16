using System;
using System.Collections.Generic;
using Analyzer.Tokens;

namespace Analyzer.ProgramStructures
{
    public class Varible : IEquatable<Varible>
    {
        public bool Equals(Varible other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Identifier, other.Identifier) && Equals(VaribleType, other.VaribleType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Varible) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Identifier != null ? Identifier.GetHashCode() : 0) * 397) ^ (VaribleType != null ? VaribleType.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Varible left, Varible right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Varible left, Varible right)
        {
            return !Equals(left, right);
        }

        public Varible(VaribleType varibleType, Identifier identifier)
        {
            VaribleType = varibleType;
            Identifier = identifier;
        }

        public Identifier Identifier { get; }
        public VaribleType VaribleType { get; }
    }
}