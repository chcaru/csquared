using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public enum LexemeType
    {
        OpenParen,
        CloseParen,
        OpenSquareBracket,
        CloseSquareBracket,
        OpenBracket,
        CloseBracket,
        DoubleQuote,
        SingleQuote,
        Semicolon,
        Comma,
        Plus,
        Minus,
        Times,
        Divides,
        GreaterThan,
        LessThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        And, 
        Or,
        GoesTo,
        DoubleEqual,
        Equal,
        Dot,
        RightArrow,
        LeftArrow,
        Not,
        NotEqual,
        ScopeEscalator,
        Null,
        Bool,
        Integer,
        Real,
        Identifier,
        String,
        If,
        Else,
        While,
        Foreach,
        In,
        To,
        Yield,
        Return,
        Var,
        Async,
        Parallel,
        Lock,
        EndOfInput,
        Unknown
    }

    public interface ILexeme
    {
        LexemeType Type { get; }
        string Name { get; }
        object Value { get; }
        int LineNumber { get; }
        int LinePosition { get; }
    }

    public class Lexeme : ILexeme
    {
        public LexemeType Type { get; set; }
        public string Name { get { return this.Type.ToString(); } }
        public object Value { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
