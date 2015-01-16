using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSquared
{
    public abstract class Primary<T, J> : 
        IPrimary, 
        IInitializable, 
        ICastable<CSquaredString>, 
        ICastable<CSquaredBoolean>
        where J : Primary<T, J>, new()
    {
        public virtual T Value { get; set; }

        public static J From(T value)
        {
            return new J
            {
                Value = value
            };
        }

        public virtual IExpression Initialize(CSquaredEnvironment environment)
        {
            return new J
            {
                Value = this.Value
            };
        }

        public virtual IExpression Evaluate(CSquaredEnvironment environment)
        {
            return this;
        }

        CSquaredString ICastable<CSquaredString>.CastTo()
        {
            return CSquaredString.From(this.ToString());
        }

        CSquaredBoolean ICastable<CSquaredBoolean>.CastTo()
        {
            return CSquaredBoolean.From(!(this is Null));
        }

        public virtual IPrimary Add(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted addition of an unsupported type: " + this.GetType());
        }

        public virtual IPrimary Subtract(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted subtraction of an unsupported type: " + this.GetType());
        }

        public virtual IPrimary Multiply(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted multiplication of an unsupported type: " + this.GetType());
        }

        public virtual IPrimary Divide(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted division of an unsupported type: " + this.GetType());
        }

        public virtual CSquaredBoolean LessThanOrEqual(CSquaredEnvironment environment, IExpression expression)
        {
            return CSquaredBoolean.From(this.LessThan(environment, expression).Value || this.Equal(environment, expression).Value);
        }

        public virtual CSquaredBoolean GreaterThanOrEqual(CSquaredEnvironment environment, IExpression expression)
        {
            return CSquaredBoolean.From(this.GreaterThan(environment, expression).Value || this.Equal(environment, expression).Value);
        }

        public virtual CSquaredBoolean LessThan(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted logical less than of an unsupported type: " + this.GetType());
        }

        public virtual CSquaredBoolean GreaterThan(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted logical greater than of an unsupported type: " + this.GetType());
        }

        public virtual CSquaredBoolean Equal(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted logical equivilance of an unsupported type: " + this.GetType());
        }

        public virtual CSquaredBoolean And(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted logical and of an unsupported type: " + this.GetType());
        }

        public virtual CSquaredBoolean Or(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted logical or of an unsupported type: " + this.GetType());
        }

        public virtual CSquaredBoolean NotEqual(CSquaredEnvironment environment, IExpression expression)
        {
            return CSquaredBoolean.From(!this.Equal(environment, expression).Value);
        }

        public virtual CSquaredBoolean Not(CSquaredEnvironment environment)
        {
            throw new NotSupportedException("Attempted take the logical negation of an unsupported type: " + this.GetType());
        }

        public virtual IExpression Access(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted to access a member of an unsupported type: " + this.GetType());
        }

        public virtual IExpression InsertLeft(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted to insert left on an unsupported type: " + this.GetType());
        }

        public virtual IExpression InsertRight(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted to insert right on an unsupported type: " + this.GetType());
        }

        public virtual IExpression Index(CSquaredEnvironment environment, IExpression expression)
        {
            throw new NotSupportedException("Attempted to index an unsupported type: " + this.GetType());
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}