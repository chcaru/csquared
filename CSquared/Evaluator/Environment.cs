using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSquared
{
    public class CSquaredEnvironment
    {
        internal class EnvironmentConstituent
        {
            public string Identifier { get; set; }
            public IExpression Expression { get; set; }
        }

        protected internal CSquaredEnvironment Parent { get; set; }

        internal IDictionary<string, EnvironmentConstituent> Constituents { get; set; }

        internal LinkedList<EnvironmentConstituent> ConstituentsOrder { get; set; }

        public CSquaredEnvironment(CSquaredEnvironment parent)
        {
            this.Parent = parent;
            this.Constituents = new Dictionary<string, EnvironmentConstituent>();
            this.ConstituentsOrder = new LinkedList<EnvironmentConstituent>();
        }

        public virtual void Set(string identifier, IExpression expression, bool addToEnd = true)
        {
            EnvironmentConstituent constituent;

            if (this.Constituents.TryGetValue(identifier, out constituent))
            {
                constituent.Expression = expression;
            }
            else
            {
                constituent = new EnvironmentConstituent
                {
                    Identifier = identifier,
                    Expression = expression
                };

                this.Constituents.Add(identifier, constituent);

                if (addToEnd)
                {
                    this.ConstituentsOrder.AddLast(constituent);
                }
                else
                {
                    this.ConstituentsOrder.AddFirst(constituent);
                }
            }
        }

        public virtual bool Update(string identifier, IExpression expression)
        {
            CSquaredEnvironment environment;

            var exists = this.GetEnvironmentFor(identifier, out environment);

            if (exists)
            {
                environment.Set(identifier, expression);
            }

            return exists;
        }

        public virtual void AddAnonymous(IExpression expression, bool addToEnd = true)
        {
            var anonymousIdentifier = Guid.NewGuid().ToString();

            this.Set(anonymousIdentifier, expression, addToEnd);
        }

        public virtual bool Lookup(string identifier, out IExpression expression)
        {
            EnvironmentConstituent constituent;

            var lookup = this.BoxedLookup(identifier, out constituent);

            expression = lookup ? constituent.Expression : null;

            return lookup;
        }

        internal bool BoxedLookup(string identifier, out EnvironmentConstituent environmentConstituent)
        {
            EnvironmentConstituent constituent;

            var lookup = this.Constituents.TryGetValue(identifier, out constituent);

            if (!lookup && !this.Parent.IsNull())
            {
                return this.Parent.BoxedLookup(identifier, out environmentConstituent);
            }
            else
            {
                environmentConstituent = constituent;
            }

            return lookup;
        }

        public virtual CSquaredEnvironment Extend()
        {
            return new CSquaredEnvironment(this);
        }

        public virtual CSquaredEnvironment GetParent()
        {
            return this.Parent != null ? this.Parent : this;
        }

        public virtual CSquaredEnvironment GetRoot()
        {
            return this.Parent != null ? this.Parent.GetRoot() : this;
        }

        protected internal virtual bool GetEnvironmentFor(string identifier, out CSquaredEnvironment environment)
        {
            var exists = this.Constituents.ContainsKey(identifier);

            if (!exists)
            {
                environment = null;

                return this.Parent != null ? this.Parent.GetEnvironmentFor(identifier, out environment) : false;
            }

            environment = this;

            return exists;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var constituent in this.ConstituentsOrder)
            {
                if (constituent.Expression != this)
                {
                    var evaluatedConstituent = constituent.Expression.Evaluate(this);

                    stringBuilder.AppendFormat("{0}: {1}\n", constituent.Identifier, evaluatedConstituent);
                }
                else
                {
                    stringBuilder.AppendFormat("{0}: {1}\n", constituent.Identifier, "THIS");
                }
            }

            return stringBuilder.ToString();
        }

    }
}
