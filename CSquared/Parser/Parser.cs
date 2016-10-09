using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public class Parser
    {
        private Lexer Lexer { get; set; }
        private ILexeme LastLexeme { get; set; }
        private ILexeme CurrentLexeme { get; set; }

        public Parser(string programPath)
        {
            this.Lexer = new Lexer(programPath);
            this.Advance();
        }

        public Parser(Lexer lexer)
        {
            this.Lexer = lexer;
            this.Advance();
        }

        public Program Parse()
        {
            return new Program
            {
                Statements = this.Program()
            };
        }

        private bool Check(LexemeType type)
        {
            return this.CurrentLexeme.Type == type;
        }

        private void Advance()
        {
            this.LastLexeme = this.CurrentLexeme;
            this.CurrentLexeme = this.Lexer.Lex();
        }

        private ILexeme Match(LexemeType type, bool advance = true)
        {
            this.MatchNoAdvance(type);
            var match = this.CurrentLexeme;
            this.Advance();
            return match;
        }

        //Used when looking ahead by one, only used in pending.
        private void Pushback()
        {
            this.Lexer.PushBack(this.CurrentLexeme);
            this.CurrentLexeme = this.LastLexeme;
        }

        private void MatchNoAdvance(LexemeType type)
        {
            if (!this.Check(type))
            {
                throw new Exception(
                    string.Format(
                        "Syntax Error. Expected {0} at line {1} position {2}, got {3} instead.",
                        type.ToString(),
                        this.CurrentLexeme.LineNumber,
                        this.CurrentLexeme.LinePosition,
                        this.CurrentLexeme.Name));
            }
        }

        private IEnumerable<IStatement> Program()
        {
            var statements = this.Statement().FromSingle();

            if (this.ProgramPending())
            {
                statements = statements.Concat(this.Program());
            }

            return statements;
        }

        private bool ProgramPending()
        {
            return this.StatementPending();
        }

        private VariableDeclaration VariableDeclaration()
        {
            this.Match(LexemeType.Arr);

            var identifierDeclaration = this.IdentifierDeclaration();

            IExpression expression;

            if (this.Check(LexemeType.Semicolon))
            {
                this.Match(LexemeType.Semicolon);
                expression = null;
            }
            else
            {
                this.Match(LexemeType.Equal);
                expression = this.Expression();
                this.Match(LexemeType.Semicolon);
            }

            return new VariableDeclaration
            {
                IdentifierDeclaration = identifierDeclaration,
                Expression = expression
            };
        }

        private bool VariableDeclarationPending()
        {
            return this.Check(LexemeType.Arr);
        }

        private IdentifierDeclaration IdentifierDeclaration()
        {
            var scopeEscalated = false;

            if (this.Check(LexemeType.ScopeEscalator))
            {
                this.Match(LexemeType.ScopeEscalator);

                scopeEscalated = true;
            }

            return new IdentifierDeclaration
            {
                ScopeEscalated = scopeEscalated,
                Identifier = this.Match(LexemeType.Identifier)
            };
        }

        private IExpression Expression()
        {
            IExpression expression = null;

            if (this.UnaryOperatorPending())
            {
                expression = new UnaryOperationExpression
                {
                    UnaryOperator = this.UnaryOperator(),
                    Expression = this.Expression()
                };
            }
            else if (this.Check(LexemeType.Async))
            {
                this.Match(LexemeType.Async);

                expression = new AsyncExpression
                {
                    Expression = this.Expression()
                };
            }
            else if (this.LambdaPending())
            {
                expression = this.Lambda();
            }
            else if (this.LambdaCallPending())
            {
                expression = this.LambdaCall();
            }
            else if (this.PrimaryPending())
            {
                expression = this.Primary();
            }

            if (!expression.IsNull())
            {
                if (this.Check(LexemeType.OpenSquareBracket))
                {
                    var indexerOperator = this.Match(LexemeType.OpenSquareBracket);

                    var indexerExpression = this.Expression();

                    this.Match(LexemeType.CloseSquareBracket);

                    expression = new BinaryOperationExpression
                    {
                        PreExpression = expression,
                        BinaryOperator = indexerOperator,
                        PostExpression = indexerExpression
                    };
                }
                else if (this.BinaryOperatorPending())
                {
                    expression = new BinaryOperationExpression
                    {
                        PreExpression = expression,
                        BinaryOperator = this.BinaryOperator(),
                        PostExpression = this.Expression()
                    };
                }
            }

            return expression;
        }

        private bool ExpressionPending()
        {
            return this.UnaryOperatorPending()
                || this.LambdaPending()
                || this.LambdaCallPending()
                || this.PrimaryPending()
                || this.Check(LexemeType.Async);
        }

        private ILexeme UnaryOperator()
        {
            return this.Match(LexemeType.Not);
        }

        private bool UnaryOperatorPending()
        {
            return this.Check(LexemeType.Not);
        }

        private LambdaExpression Lambda()
        {
            this.Match(LexemeType.OpenParen);

            var paramList = this.OptionalParamList();

            this.Match(LexemeType.CloseParen);

            this.Match(LexemeType.GoesTo);

            return new LambdaExpression
            {
                Parameters = paramList,
                Body = this.LambdaBody()
            };
        }

        private bool LambdaPending()
        {
            return this.Check(LexemeType.OpenParen);
        }

        private LambdaCall LambdaCall()
        {
            var identifier = this.Match(LexemeType.Identifier);

            this.Match(LexemeType.OpenParen);

            var arguments = this.OptionalArgumentList();

            this.Match(LexemeType.CloseParen);

            return new LambdaCall
            {
                Identifier = identifier,
                Arguments = arguments
            };
        }

        private bool LambdaCallPending()
        {
            var pending = this.Check(LexemeType.Identifier);

            if (pending)
            {
                this.Match(LexemeType.Identifier);

                pending &= this.Check(LexemeType.OpenParen);

                this.Pushback();
            }

            return pending;
        }

        private IPrimary Primary()
        {
            if (this.Check(LexemeType.Bool))
            {
                return new CSquaredBoolean
                {
                    Value = (bool)this.Match(LexemeType.Bool).Value
                };
            }
            else if (this.Check(LexemeType.Null))
            {
                this.Match(LexemeType.Null);
                return new Null();
            }
            else if (this.Check(LexemeType.Integer))
            {
                return new Integer
                {
                    Value = (int)(double)this.Match(LexemeType.Integer).Value
                };
            }
            else if (this.Check(LexemeType.Real))
            {
                return new Real
                {
                    Value = (double)this.Match(LexemeType.Real).Value
                };
            }
            else if (this.Check(LexemeType.String))
            {
                return new CSquaredString
                {
                    Value = (string)this.Match(LexemeType.String).Value
                };
            }
            else if (this.Check(LexemeType.Identifier))
            {
                return new Identifier
                {
                    Value = (string)this.Match(LexemeType.Identifier).Value
                };
            }
            else if (this.ArrayPending())
            {
                return this.Array();
            }
            else
            {
                return this.Object();
            }
        }

        private bool PrimaryPending()
        {
            return this.Check(LexemeType.Bool)
                || this.Check(LexemeType.Null)
                || this.Check(LexemeType.Integer)
                || this.Check(LexemeType.Real)
                || this.Check(LexemeType.String)
                || this.Check(LexemeType.Identifier)
                || this.ArrayPending()
                || this.ObjectPending();
        }

        private ILexeme BinaryOperator()
        {
            if (this.Check(LexemeType.Plus))
            {
                return this.Match(LexemeType.Plus);
            }
            else if (this.Check(LexemeType.Minus))
            {
                return this.Match(LexemeType.Minus);
            }
            else if (this.Check(LexemeType.Times))
            {
                return this.Match(LexemeType.Times);
            }
            else if (this.Check(LexemeType.Divides))
            {
                return this.Match(LexemeType.Divides);
            }
            else if (this.Check(LexemeType.LessThanOrEqual))
            {
                return this.Match(LexemeType.LessThanOrEqual);
            }
            else if (this.Check(LexemeType.GreaterThanOrEqual))
            {
                return this.Match(LexemeType.GreaterThanOrEqual);
            }
            else if (this.Check(LexemeType.LessThan))
            {
                return this.Match(LexemeType.LessThan);
            }
            else if (this.Check(LexemeType.GreaterThan))
            {
                return this.Match(LexemeType.GreaterThan);
            }
            else if (this.Check(LexemeType.DoubleEqual))
            {
                return this.Match(LexemeType.DoubleEqual);
            }
            else if (this.Check(LexemeType.And))
            {
                return this.Match(LexemeType.And);
            }
            else if (this.Check(LexemeType.Or))
            {
                return this.Match(LexemeType.Or);
            }
            else if (this.Check(LexemeType.NotEqual))
            {
                return this.Match(LexemeType.NotEqual);
            }
            else if (this.Check(LexemeType.Dot))
            {
                return this.Match(LexemeType.Dot);
            }
            else if (this.Check(LexemeType.LeftArrow))
            {
                return this.Match(LexemeType.LeftArrow);
            }
            else
            {
                return this.Match(LexemeType.RightArrow);
            }
        }

        private bool BinaryOperatorPending()
        {
            return this.Check(LexemeType.Plus)
                || this.Check(LexemeType.Minus)
                || this.Check(LexemeType.Times)
                || this.Check(LexemeType.Divides)
                || this.Check(LexemeType.LessThanOrEqual)
                || this.Check(LexemeType.GreaterThanOrEqual)
                || this.Check(LexemeType.LessThan)
                || this.Check(LexemeType.GreaterThan)
                || this.Check(LexemeType.DoubleEqual)
                || this.Check(LexemeType.NotEqual)
                || this.Check(LexemeType.Dot)
                || this.Check(LexemeType.LeftArrow)
                || this.Check(LexemeType.RightArrow);
        }

        private IEnumerable<ILexeme> OptionalParamList()
        {
            if (this.ParamListPending())
            {
                return this.ParamList();
            }

            return Enumerable.Empty<ILexeme>();
        }

        private bool OptionalParamListPending()
        {
            return true; //Can always be true b/c of empty
        }

        private IEnumerable<IStatement> LambdaBody()
        {
            if (this.BlockPending())
            {
                return this.Block();
            }
            else
            {
                return new ReturnStatement
                {
                    ReturnExpression = this.Expression()
                }.FromSingle();
            }
        }

        private bool LambdaBodyPending()
        {
            return this.BlockPending() || this.ExpressionPending();
        }

        private IEnumerable<IExpression> OptionalArgumentList()
        {
            if (this.ArgumentListPending())
            {
                return this.ArgumentList();
            }

            return Enumerable.Empty<IExpression>();
        }

        private bool OptionalArgumentListPending()
        {
            return true;
        }

        private IEnumerable<ILexeme> ParamList()
        {
            var parameters = this.Match(LexemeType.Identifier).FromSingle();

            if (this.Check(LexemeType.Comma))
            {
                this.Match(LexemeType.Comma);

                parameters = parameters.Concat(this.ParamList());
            }

            return parameters;
        }

        private bool ParamListPending()
        {
            return this.Check(LexemeType.Identifier);
        }

        private IEnumerable<IStatement> Block()
        {
            this.Match(LexemeType.OpenBracket);

            var statements = this.OptionalStatementList();

            this.Match(LexemeType.CloseBracket);

            return statements;
        }

        private bool BlockPending()
        {
            return this.Check(LexemeType.OpenBracket);
        }

        private Array Array()
        {
            this.Match(LexemeType.OpenSquareBracket);

            var expressions = this.OptionalExpressionList();

            this.Match(LexemeType.CloseSquareBracket);

            return new Array
            {
                Expressions = expressions,
                SizeExpression = this.SizeModifier()
            };
        }

        private bool ArrayPending()
        {
            return this.Check(LexemeType.OpenSquareBracket);
        }

        private IExpression SizeModifier()
        {
            if (this.Check(LexemeType.OpenParen))
            {
                this.Match(LexemeType.OpenParen);

                var expression = this.Expression();

                this.Match(LexemeType.CloseParen);

                return expression;
            }

            return null;
        }

        private CSquaredObject Object()
        {
            /*
            this.Match(LexemeType.OpenBracket);

            var variableDeclarations = this.OptionalVariableDeclarationList();

            this.Match(LexemeType.CloseBracket);
            */

            var variableDeclarations = this.Block();

            return new CSquaredObject
            {
                VariableDeclarations = variableDeclarations
            };
        }

        private bool ObjectPending()
        {
            return this.Check(LexemeType.OpenBracket);
        }

        private IEnumerable<IExpression> ArgumentList()
        {
            var arguments = this.Argument().FromSingle();

            if (this.Check(LexemeType.Comma))
            {
                this.Match(LexemeType.Comma);

                arguments = arguments.Concat(this.ArgumentList());
            }

            return arguments;
        }

        private bool ArgumentListPending()
        {
            return this.ArgumentPending();
        }

        private IEnumerable<IStatement> OptionalStatementList()
        {
            if (this.StatementListPending())
            {
                return this.StatementList();
            }

            return Enumerable.Empty<IStatement>();
        }

        private bool OptionalStatementListPending()
        {
            return true;
        }

        private IEnumerable<IExpression> OptionalExpressionList()
        {
            if (this.ExpressionListPending())
            {
                return this.ExpressionList();
            }

            return Enumerable.Empty<IExpression>();
        }

        private bool OptionalExpressionListPending()
        {
            return true;
        }

        private IExpression Argument()
        {
            return this.Expression();
        }

        private bool ArgumentPending()
        {
            return this.ExpressionPending();
        }

        private IEnumerable<IStatement> StatementList()
        {
            var statements = this.Statement().FromSingle();
            
            if (this.StatementListPending())
            {
                statements = statements.Concat(this.StatementList());
            }

            return statements;
        }

        private bool StatementListPending()
        {
            return this.StatementPending();
        }

        private IEnumerable<IExpression> ExpressionList()
        {
            var expressions = this.Expression().FromSingle();

            if (this.Check(LexemeType.Comma))
            {
                this.Match(LexemeType.Comma);

                expressions = expressions.Concat(this.ExpressionList());
            }

            return expressions;
        }

        private bool ExpressionListPending()
        {
            return this.ExpressionPending();
        }

        private IStatement Statement()
        {
            if (this.VariableAssignmentPending())
            {
                var variableAssignment = this.VariableAssignment();

                this.Match(LexemeType.Semicolon);

                return variableAssignment;
            }
            else if (this.ExpressionPending())
            {
                var expression = this.Expression();

                this.Match(LexemeType.Semicolon);

                return expression;
            }
            else if (this.VariableDeclarationPending())
            {
                return this.VariableDeclaration();
            }
            else if (this.IfStatementPending())
            {
                return this.IfStatement();
            }
            else if (this.WhileStatementPending())
            {
                return this.WhileStatement();
            }
            else if (this.ForeachStatementPending())
            {
                return this.ForeachStatement();
            }
            else if (this.LockStatementPending())
            {
                return this.LockStatement();
            }
            else if (this.ReturnStatementPending())
            {
                return this.ReturnStatement();
            }
            else
            {
                return this.YieldStatement();
            }
        }

        private bool StatementPending()
        {
            return this.ExpressionPending()
                || this.VariableDeclarationPending()
                || this.IfStatementPending()
                || this.WhileStatementPending()
                || this.ForeachStatementPending()
                || this.ReturnStatementPending()
                || this.YieldStatementPending()
                || this.LockStatementPending();
        }

        private IfStatement IfStatement()
        {
            this.Match(LexemeType.If);

            this.Match(LexemeType.OpenParen);

            var expression = this.Expression();

            this.Match(LexemeType.CloseParen);

            return new IfStatement
            {
                ConditionalExpression = expression,
                Body = this.Block(),
                ElseContainer = this.OptionalElse()
            };
        }

        private bool IfStatementPending()
        {
            return this.Check(LexemeType.If);
        }

        private WhileStatement WhileStatement()
        {
            this.Match(LexemeType.While);

            this.Match(LexemeType.OpenParen);

            var expression = this.Expression();

            this.Match(LexemeType.CloseParen);

            return new WhileStatement
            {
                ConditionalExpression = expression,
                Body = this.Block()
            };
        }

        private bool WhileStatementPending()
        {
            return this.Check(LexemeType.While);
        }

        private ForeachStatement ForeachStatement()
        {
            var parallel = this.Check(LexemeType.Parallel);

            if (parallel)
            {
                this.Match(LexemeType.Parallel);
            }

            this.Match(LexemeType.Foreach);

            this.Match(LexemeType.OpenParen);

            var outIdentifier = this.Match(LexemeType.Identifier);

            this.Match(LexemeType.In);

            var inIdentifier = this.Match(LexemeType.Identifier);

            this.Match(LexemeType.CloseParen);

            
            return
                parallel ?
                    new ParallelForeachStatement
                    {
                        OutIdentifier = outIdentifier,
                        InIdentifier = inIdentifier,
                        Body = this.Block()
                    } :
                    new ForeachStatement
                    {
                        OutIdentifier = outIdentifier,
                        InIdentifier = inIdentifier,
                        Body = this.Block()
                    };
        }

        private bool ForeachStatementPending()
        {
            return this.Check(LexemeType.Parallel) || this.Check(LexemeType.Foreach);
        }

        private ElseContainer OptionalElse()
        {
            if (this.Check(LexemeType.Else))
            {
                this.Match(LexemeType.Else);

                if (this.BlockPending())
                {
                    return new ElseContainer
                    {
                        Body = this.Block()
                    };
                }
                else
                {
                    return new ElseContainer
                    {
                        IfStatement = this.IfStatement()
                    };
                }
            }

            return null;
        }

        private bool OptionalElsePending()
        {
            return true;
        }

        private IEnumerable<VariableDeclaration> OptionalVariableDeclarationList()
        {
            if (this.VariableDeclarationListPending())
            {
                return this.VariableDeclarationList();
            }

            return Enumerable.Empty<VariableDeclaration>();
        }

        private IEnumerable<VariableDeclaration> VariableDeclarationList()
        {
            var variableDeclarations = this.VariableDeclaration().FromSingle();

            if (this.VariableDeclarationListPending())
            {
                variableDeclarations = variableDeclarations.Concat(this.VariableDeclarationList());
            }

            return variableDeclarations;
        }

        private bool VariableDeclarationListPending()
        {
            return this.VariableDeclarationPending();
        }

        public ReturnStatement ReturnStatement()
        {
            this.Match(LexemeType.Return);

            var returnExpression = this.Expression();

            this.Match(LexemeType.Semicolon);

            return new ReturnStatement
            {
                ReturnExpression = returnExpression
            };
        }

        public bool ReturnStatementPending()
        {
            return this.Check(LexemeType.Return);
        }

        public YieldStatement YieldStatement()
        {
            this.Match(LexemeType.Yield);

            var yieldExpression = this.Expression();

            this.Match(LexemeType.To);

            var yieldTo = this.Match(LexemeType.Identifier);

            this.Match(LexemeType.Semicolon);

            return new YieldStatement
            {
                YieldExpression = yieldExpression,
                YieldTo = yieldTo
            };
        }

        public bool YieldStatementPending()
        {
            return this.Check(LexemeType.Yield);
        }

        public VariableAssignment VariableAssignment()
        {
            var identifier = this.Match(LexemeType.Identifier);

            IExpression indexExpression = null;

            if (this.Check(LexemeType.OpenSquareBracket))
            {
                this.Match(LexemeType.OpenSquareBracket);

                indexExpression = this.Expression();

                this.Match(LexemeType.CloseSquareBracket);
            }

            this.Match(LexemeType.Equal);

            return new VariableAssignment
            {
                Identifier = identifier,
                Expression = this.Expression(),
                IndexExpression = indexExpression
            };
        }

        public bool VariableAssignmentPending()
        {
            if (this.Check(LexemeType.Identifier))
            {
                this.Match(LexemeType.Identifier);

                var pending = this.Check(LexemeType.Equal) || this.Check(LexemeType.OpenSquareBracket);

                this.Pushback();

                return pending;
            }

            return false;
        }

        private LockStatement LockStatement()
        {
            this.Match(LexemeType.Lock);

            this.Match(LexemeType.OpenParen);

            var identifier = this.Match(LexemeType.Identifier);

            this.Match(LexemeType.CloseParen);

            return new LockStatement
            {
                Identifier = identifier,
                Body = this.Block()
            };
        }

        private bool LockStatementPending()
        {
            return this.Check(LexemeType.Lock);
        }
    }
}
