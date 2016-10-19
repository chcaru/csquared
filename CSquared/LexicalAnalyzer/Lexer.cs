using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class Lexer
    {
        private Reader Reader { get; set; }

        private Stack<ILexeme> PushedBack { get; set; }

        private Lexer()
        {
            this.PushedBack = new Stack<ILexeme>();
        }

        public Lexer(string filePath) 
            : this()
        {
            this.Reader = new Reader(filePath);
        }

        public Lexer(Reader reader) 
            : this()
        {
            this.Reader = reader;
        }

        public void PushBack(ILexeme lexeme)
        {
            this.PushedBack.Push(lexeme);
        }

        public ILexeme Lex()
        {
            if (this.PushedBack.Count > 0)
            {
                return this.PushedBack.Pop();
            }

            var next = this.Reader.GetNextChar();

            if (next == null)
            {
                return new Lexeme 
                {
                    Type = LexemeType.EndOfInput,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }

            switch(next)
            {
                case '(': return new Lexeme
                {
                    Type = LexemeType.OpenParen,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case ')': return new Lexeme
                {
                    Type = LexemeType.CloseParen,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '{': return new Lexeme
                {
                    Type = LexemeType.OpenBracket,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '}': return new Lexeme
                {
                    Type = LexemeType.CloseBracket,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '+': return new Lexeme
                {
                    Type = LexemeType.Plus,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '*': return new Lexeme
                {
                    Type = LexemeType.Times,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '[': return new Lexeme
                {
                    Type = LexemeType.OpenSquareBracket,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case ']': return new Lexeme
                {
                    Type = LexemeType.CloseSquareBracket,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '.': return new Lexeme
                {
                    Type = LexemeType.Dot,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case ',': return new Lexeme
                {
                    Type = LexemeType.Comma,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case ';': return new Lexeme
                {
                    Type = LexemeType.Semicolon,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '^': return new Lexeme
                {
                    Type = LexemeType.ScopeEscalator,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '|': return new Lexeme
                {
                    Type = LexemeType.Or,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
                case '&': return new Lexeme
                {
                    Type = LexemeType.And,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }

            if (next == '-')
            {
                var next2 = this.Reader.GetNextChar();

                if (next2 == '>')
                {
                    return new Lexeme
                    {
                        Type = LexemeType.RightArrow,
                        Value = "->",
                        LineNumber = this.Reader.LineNumber,
                        LinePosition = this.Reader.LinePosition
                    };
                }

                this.Reader.PushBack(next2);

                return new Lexeme
                {
                    Type = LexemeType.Minus,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }
            else if (next == '/')
            {
                var next2 = this.Reader.GetNextChar();

                if (next2 == '/') // Comment, skip it, return next lex
                {
                    next2 = this.Reader.GetNextChar(false);

                    while (next2 != '\n')
                    {
                        next2 = this.Reader.GetNextChar(false);
                    }

                    if (next2 != '\n')
                    {
                        this.Reader.PushBack(next2);
                    }

                    return this.Lex();
                }

                this.Reader.PushBack(next2);

                return new Lexeme
                {
                    Type = LexemeType.Divides,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }
                
            else  if (next == '!')
            {
                var next2 = this.Reader.GetNextChar();

                if (next2 == '=')
                {
                    return new Lexeme
                    {
                        Type = LexemeType.NotEqual,
                        Value = "!=",
                        LineNumber = this.Reader.LineNumber,
                        LinePosition = this.Reader.LinePosition
                    };
                }

                this.Reader.PushBack(next2);

                return new Lexeme
                {
                    Type = LexemeType.Not,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }
            else if (next == '>')
            {
                var next2 = this.Reader.GetNextChar();

                if (next2 == '=')
                {
                    return new Lexeme
                    {
                        Type = LexemeType.GreaterThanOrEqual,
                        Value = ">=",
                        LineNumber = this.Reader.LineNumber,
                        LinePosition = this.Reader.LinePosition
                    };
                }

                this.Reader.PushBack(next2);

                return new Lexeme
                {
                    Type = LexemeType.GreaterThan,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }
            else if (next == '<')
            {
                var next2 = this.Reader.GetNextChar();

                if (next2 == '=')
                {
                    return new Lexeme
                    {
                        Type = LexemeType.LessThanOrEqual,
                        Value = "<=",
                        LineNumber = this.Reader.LineNumber,
                        LinePosition = this.Reader.LinePosition
                    };
                }
                else if (next2 == '-')
                {
                    return new Lexeme
                    {
                        Type = LexemeType.LeftArrow,
                        Value = "<-",
                        LineNumber = this.Reader.LineNumber,
                        LinePosition = this.Reader.LinePosition
                    };
                }

                this.Reader.PushBack(next2);

                return new Lexeme
                {
                    Type = LexemeType.LessThan,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }
            else if (next == '=')
            {
                var next2 = this.Reader.GetNextChar();

                if (next2 == '=')
                {
                    return new Lexeme
                    {
                        Type = LexemeType.DoubleEqual,
                        Value = "==",
                        LineNumber = this.Reader.LineNumber,
                        LinePosition = this.Reader.LinePosition
                    };
                } 
                else if (next2 == '>')
                {
                    return new Lexeme
                    {
                        Type = LexemeType.GoesTo,
                        Value = "=>",
                        LineNumber = this.Reader.LineNumber,
                        LinePosition = this.Reader.LinePosition
                    };
                }

                this.Reader.PushBack(next2);

                return new Lexeme
                {
                    Type = LexemeType.Equal,
                    Value = next,
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }
            else if (next == '\"')
            {
                var sB = new StringBuilder();

                var next2 = this.Reader.GetNextChar(false);

                while (next2 != null && next2 != '\"')
                {
                    sB.Append((char)next2);

                    next2 = this.Reader.GetNextChar(false);

                    if (next2 == null)
                    {
                        break;
                    }
                }

                if (next2 != '\"')
                {
                    this.Reader.PushBack(next2);
                }

                return new Lexeme
                {
                    Type = LexemeType.String,
                    Value = sB.ToString(),
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }
            else if (char.IsDigit((char)next))
            {
                var sB = new StringBuilder(((char)next).ToString());

                var next2 = this.Reader.GetNextChar();

                var numRealDecimals = 0;

                while (next2 != null && (char.IsDigit((char)next2) || next2 == '.'))
                {
                    if (next2 == '.')
                    {
                        if (numRealDecimals++ > 1)
                        {
                            throw new Exception("Malformatted real. Too many decimals");
                        }
                    }

                    sB.Append((char)next2);

                    next2 = this.Reader.GetNextChar();
                }

                this.Reader.PushBack(next2);

                return new Lexeme
                {
                    Type = numRealDecimals > 0 ? LexemeType.Real : LexemeType.Integer,
                    Value = numRealDecimals > 0 ? double.Parse(sB.ToString()) : int.Parse(sB.ToString()),
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }
            else if (char.IsLetter((char)next))
            {
                var sB = new StringBuilder(((char)next).ToString());

                var next2 = this.Reader.GetNextChar(false);

                while (next2 != null && char.IsLetter((char)next2))
                {
                    sB.Append((char)next2);

                    var currentLetters = sB.ToString();

                    var next3 = this.Reader.GetNextChar(false);
                    var next3IsLetter = char.IsLetter((char)next3);
                    this.Reader.PushBack(next3);

                    if (!next3IsLetter)
                    {
                        if (currentLetters == "if")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.If,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "in")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.In,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "to")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.To,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "async")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Async,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "lock")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Lock,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "else")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Else,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "null")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Null,
                                Value = null,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "true")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Bool,
                                Value = true,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "false")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Bool,
                                Value = false,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "yield")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Yield,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "return")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Return,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "while")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.While,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "parallel")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Parallel,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                        else if (currentLetters == "foreach")
                        {
                            return new Lexeme
                            {
                                Type = LexemeType.Foreach,
                                Value = currentLetters,
                                LineNumber = this.Reader.LineNumber,
                                LinePosition = this.Reader.LinePosition
                            };
                        }
                    }

                    next2 = this.Reader.GetNextChar(false);
                }

                if (!char.IsLetter((char)next2))
                {
                    this.Reader.PushBack(next2);
                }

                return new Lexeme
                {
                    Type = LexemeType.Identifier,
                    Value = sB.ToString(),
                    LineNumber = this.Reader.LineNumber,
                    LinePosition = this.Reader.LinePosition
                };
            }

            return new Lexeme
            {
                Type = LexemeType.Unknown,
                Value = next,
                LineNumber = this.Reader.LineNumber,
                LinePosition = this.Reader.LinePosition
            };

        }
    }
}
