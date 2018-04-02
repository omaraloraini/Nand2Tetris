using System;
using System.Text.RegularExpressions;

namespace Analyzer.Tokens
{
    public class KeywordToken : Token, IEquatable<KeywordToken>
    {
        public string Keyword { get; }
        
        private static Regex _keywordRegex = new Regex(
            "^(class|constructor|function|method|field|static|var|int|char|boolean|void|true|false|null|this|let|do|if|else|while|return)");
        
        public KeywordToken(string keyword)
        {
            if (keyword is null) throw new ArgumentNullException(nameof(keyword));
            
            if (!_keywordRegex.IsMatch(keyword))
                throw new ArgumentException($"{keyword} : invlaid keyword");
            
            Keyword = keyword;
        }

        public static bool IsKeyword(string keyword) =>_keywordRegex.IsMatch(keyword);
        
        public static KeywordToken Class => new KeywordToken("class");
        public static KeywordToken Constructor => new KeywordToken("constructor");
        public static KeywordToken Function => new KeywordToken("function");
        public static KeywordToken Method => new KeywordToken("method");
        public static KeywordToken Field => new KeywordToken("field");
        public static KeywordToken Static => new KeywordToken("static");
        public static KeywordToken Var => new KeywordToken("var");
        public static KeywordToken Int => new KeywordToken("int");
        public static KeywordToken Char => new KeywordToken("char");
        public static KeywordToken Boolean => new KeywordToken("boolean");
        public static KeywordToken Void => new KeywordToken("void");
        public static KeywordToken True => new KeywordToken("true");
        public static KeywordToken False => new KeywordToken("false");
        public static KeywordToken Null => new KeywordToken("null");
        public static KeywordToken This => new KeywordToken("this");
        public static KeywordToken Let => new KeywordToken("let");
        public static KeywordToken Do => new KeywordToken("do");
        public static KeywordToken If => new KeywordToken("if");
        public static KeywordToken Else => new KeywordToken("else");
        public static KeywordToken While => new KeywordToken("while");
        public static KeywordToken Return => new KeywordToken("retrun");

        public bool Equals(KeywordToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Keyword, other.Keyword);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeywordToken) obj);
        }

        public override int GetHashCode() => Keyword.GetHashCode();
        public static bool operator ==(KeywordToken left, KeywordToken right) => Equals(left, right);
        public static bool operator !=(KeywordToken left, KeywordToken right) => !Equals(left, right);
        public override string ToString() => Keyword;
    }
}