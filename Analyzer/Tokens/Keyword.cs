using System;
using System.Text.RegularExpressions;
using Analyzer.Expressions;

namespace Analyzer.Tokens
{
    public class KeywordConstant : Keyword, ITerm
    {
        public KeywordConstant(string value) : base(value)
        {
            if (!IsKeywordConstant(value)) throw new ArgumentException("Invalid KeywordConstant");
        }

        public static bool IsKeywordConstant(string value) =>
            value == "true" ||
            value == "false" ||
            value == "null" ||
            value == "this";
        
        public static KeywordConstant True { get; } = new KeywordConstant("true");
        public static KeywordConstant False { get; } = new KeywordConstant("false");
        public static KeywordConstant Null { get; } = new KeywordConstant("null");
        public static KeywordConstant This { get; } = new KeywordConstant("this");
    }
    public class Keyword : Token, IEquatable<Keyword>
    {
        private static Regex _keywordRegex = new Regex(
            "^(class|constructor|function|method|field|static|var|int|char|boolean|void|true|false|null|this|let|do|if|else|while|return)$");
        
        public Keyword(string value) : base(value)
        {   
            if (!_keywordRegex.IsMatch(value))
                throw new ArgumentException($"{value} : invlaid keyword");   
        }

        public static bool IsKeyword(string keyword) =>_keywordRegex.IsMatch(keyword);
        
        public static Keyword Class { get; } = new Keyword("class");
        public static Keyword Constructor { get; } = new Keyword("constructor");
        public static Keyword Function { get; } = new Keyword("function");
        public static Keyword Method { get; } = new Keyword("method");
        public static Keyword Field { get; } = new Keyword("field");
        public static Keyword Static { get; } = new Keyword("static");
        public static Keyword Var { get; } = new Keyword("var");
        public static Keyword Int { get; } = new Keyword("int");
        public static Keyword Char { get; } = new Keyword("char");
        public static Keyword Boolean { get; } = new Keyword("boolean");
        public static Keyword Void { get; } = new Keyword("void");
        public static Keyword Let { get; } = new Keyword("let");
        public static Keyword Do { get; } = new Keyword("do");
        public static Keyword If { get; } = new Keyword("if");
        public static Keyword Else { get; } = new Keyword("else");
        public static Keyword While { get; } = new Keyword("while");
        public static Keyword Return { get; } = new Keyword("return");

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