using System;
using System.Text.RegularExpressions;

namespace Analyzer.Tokens
{
    public class Keyword : Token, IEquatable<Keyword>
    {
        public string Value { get; }
        
        private static Regex _keywordRegex = new Regex(
            "^(class|constructor|function|method|field|static|var|int|char|boolean|void|true|false|null|this|let|do|if|else|while|return)");
        
        public Keyword(string value) : base(value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            
            if (!_keywordRegex.IsMatch(value))
                throw new ArgumentException($"{value} : invlaid keyword");
            
            Value = value;
        }

        public static bool IsKeyword(string keyword) =>_keywordRegex.IsMatch(keyword);
        
        public static Keyword Class => new Keyword("class");
        public static Keyword Constructor => new Keyword("constructor");
        public static Keyword Function => new Keyword("function");
        public static Keyword Method => new Keyword("method");
        public static Keyword Field => new Keyword("field");
        public static Keyword Static => new Keyword("static");
        public static Keyword Var => new Keyword("var");
        public static Keyword Int => new Keyword("int");
        public static Keyword Char => new Keyword("char");
        public static Keyword Boolean => new Keyword("boolean");
        public static Keyword Void => new Keyword("void");
        public static Keyword True => new Keyword("true");
        public static Keyword False => new Keyword("false");
        public static Keyword Null => new Keyword("null");
        public static Keyword This => new Keyword("this");
        public static Keyword Let => new Keyword("let");
        public static Keyword Do => new Keyword("do");
        public static Keyword If => new Keyword("if");
        public static Keyword Else => new Keyword("else");
        public static Keyword While => new Keyword("while");
        public static Keyword Return => new Keyword("return");

        public bool Equals(Keyword other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Keyword) obj);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(Keyword left, Keyword right) => Equals(left, right);
        public static bool operator !=(Keyword left, Keyword right) => !Equals(left, right);
        public override string ToString() => Value;
    }
}