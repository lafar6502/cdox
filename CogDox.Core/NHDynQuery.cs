using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace CogDox.Core
{
    /// <summary>
    /// Lucene query AST node
    /// </summary>
    public abstract class QueryNode
    {
        
        public virtual QueryNode And(QueryNode rhs)
        {
            return new AndNode(this, rhs);
        }

        public virtual QueryNode Or(QueryNode rhs)
        {
            return new OrNode(this, rhs);
        }

        public QueryNode Add(QueryNode rhs)
        {
            return new PlusNode(this, rhs);
        }

        public virtual QueryNode Not()
        {
            return new NotNode(this);
        }

        public static bool operator true(QueryNode expr)
        {
            return false; // never true to disable short-circuit evaluation of a || b
        }

        public static bool operator false(QueryNode expr)
        {
            return false; // never false to disable short-circuit evaluation of a && b
        }

        public static QueryNode operator &(QueryNode lhs, QueryNode rhs)
        {
            if (lhs is NotNode) return rhs;
            if (rhs is NotNode) return lhs;
            return lhs.And(rhs);
        }

        public static QueryNode operator |(QueryNode lhs, QueryNode rhs)
        {
            if (lhs is NotNode) return rhs;
            if (rhs is NotNode) return lhs;
            return lhs.Or(rhs);
        }

        public static QueryNode operator +(QueryNode lhs, QueryNode rhs)
        {
            return lhs.Add(rhs);
        }


        public static QueryNode operator !(QueryNode n)
        {
            return n.Not();
        }

        public static QueryNode Raw(string query)
        {
            return new RawQuery(query);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public abstract object Accept(IQueryNodeVisitor visitor);
    }

    public class RawQuery : QueryNode
    {
        private string _q;
        public RawQuery(string q)
        {
            _q = q;
        }

        public override string ToString()
        {
            return _q;
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }



    public class AndNode : QueryNode
    {
        public QueryNode Left { get; set; }
        public QueryNode Right { get; set; }
        public AndNode(QueryNode left, QueryNode right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format("({0}) AND ({1})", Left.ToString(), Right.ToString());
        }


        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }

    public class PlusNode : QueryNode
    {
        public QueryNode Left { get; set; }
        public QueryNode Right { get; set; }
        public PlusNode(QueryNode left, QueryNode right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Left.ToString(), Right.ToString());
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class NotNode : QueryNode
    {
        public QueryNode Inner { get; set; }

        public NotNode(QueryNode q)
        {
            Inner = q;
        }

        public override string ToString()
        {
            return string.Format("NOT ({0})", Inner.ToString());
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class OrNode : QueryNode
    {
        public QueryNode Left { get; set; }
        public QueryNode Right { get; set; }

        public OrNode(QueryNode left, QueryNode right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format("({0}) OR ({1})", Left.ToString(), Right.ToString());
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    /// <summary>
    /// Empty node (for handling missing conditions eg when parameter is not supplied)
    /// </summary>
    public class NullNode : QueryNode
    {
        public override QueryNode Not()
        {
            return this; //
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return null;
        }
    }
    /// <summary>
    /// Field boolean condition
    /// </summary>
    public class FieldPredicate : QueryNode
    {
        public Field Fld { get; set; }

        /// <summary>
        /// boolean operator types
        /// </summary>
        public enum OperatorType
        {
            EQ,
            LT,
            GT,
            NE,
            IN,
            RANGE,
            LK,
            NULL,
            NOTNULL
        }
        public OperatorType Operator { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }

        private static string FormatFieldValue(object v)
        {
            return Convert.ToString(v);
            /*
            if (v == null) return null;
            var nt = Nullable.GetUnderlyingType(v.GetType());
            if (nt != null)
            {
                PropertyInfo pi = v.GetType().GetProperty("HasValue");
                bool b = (bool)pi.GetValue(v, null);
                if (!b) return null;
                pi = v.GetType().GetProperty("Value");
                v = pi.GetValue(v, null);
            }
            if (v is DateTime)
            {
                return LuceneQueryBuilder.FormatQueryDate((DateTime)v);
            }
            else if (v is string)
            {
                return LuceneQueryBuilder.EscapeQueryText((string)v);
            }
            else
            {
                return Convert.ToString(v);
            }*/
        }

        public override string ToString()
        {
            if (Fld.Name == null)
            {
                return Convert.ToString(Value); //default field query or a raw query...
            }
            /*
            switch (Operator)
            {
                case OperatorType.EQ:
                    return string.Format("{0}:{1}", Fld.Name, FormatFieldValue(Value));
                case OperatorType.LK:
                    var str = Convert.ToString(Value);
                    if (str.EndsWith("*")) str = str.Substring(0, str.Length - 1);
                    return string.Format("{0}:{1}*", Fld.Name, LuceneQueryBuilder.EscapeQueryText(str) + "*");
                case OperatorType.NE:
                    return string.Format("-{0}:{1}", Fld.Name, FormatFieldValue(Value));
                case OperatorType.NULL:
                    return string.Format("-{0}", Fld.Name);
                case OperatorType.NOTNULL:
                    return string.Format("{0}:[* TO *]", Fld.Name);
                case OperatorType.RANGE:
                    string s1 = FormatFieldValue(Value);
                    string s2 = FormatFieldValue(Value2);
                    return string.Format("{0}:[{1} TO {2}]", Fld.Name, s1 == null ? "*" : s1, s2 == null ? "*" : s2);
                case OperatorType.LT:
                    return string.Format("{0}:[* TO {1}]", Fld.Name, FormatFieldValue(Value));
                case OperatorType.GT:
                    return string.Format("{0}:[{1} TO *]", Fld.Name, FormatFieldValue(Value));
                case OperatorType.IN:
                    StringBuilder sb = new StringBuilder();
                    List<object> l = Value as List<object>;
                    if (l == null) throw new Exception("Invalid argument in field " + Fld.Name);
                    foreach (object v in l)
                    {
                        if (sb.Length > 0) sb.Append(" OR ");
                        sb.Append(FormatFieldValue(v));
                    }
                    return string.Format("{0}:({1})", Fld.Name, sb);
                default:
                    throw new NotImplementedException(string.Format("Field: {0}, Operator: {1}, Value: {2}", Fld.Name, Operator, Value));
            }
            */
            throw new NotImplementedException();
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    /// <summary>
    /// object field expression
    /// </summary>
    public class Field : System.Dynamic.DynamicObject
    {
        public string Name { get; set; }

        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            Console.WriteLine("TryGetMember {0}", binder.Name);
            return base.TryGetMember(binder, out result);
        }

        public QueryNode EQ(object v)
        {
            if (v == null) return new NullNode();
            if (v == null) return this.IsNull();
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.EQ, Value = v };
        }

        public static QueryNode operator ==(Field lhs, object rhs)
        {
            return lhs.EQ(rhs);
        }

        public QueryNode NE(object v)
        {
            if (v == null) return new NullNode();
            if (v == null) return this.IsNotNull();
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.NE, Value = v };
        }

        public static QueryNode operator !=(Field lhs, object rhs)
        {
            return lhs.NE(rhs);
        }

        public QueryNode LT(object v)
        {
            if (v == null) return new NullNode();
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.LT, Value = v };
        }

        public static QueryNode operator <(Field lhs, object rhs)
        {
            return lhs.LT(rhs);
        }

        public QueryNode GT(object v)
        {
            if (v == null) return new NullNode();
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.GT, Value = v };
        }

        public static QueryNode operator >(Field lhs, object rhs)
        {
            return lhs.GT(rhs);
        }

        public QueryNode In<T>(IEnumerable<T> values)
        {
            if (values == null) return new NullNode();
            List<object> l = new List<object>();
            foreach (var v in values)
            {
                l.Add(v);
            }
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.IN, Value = l };
        }

        public QueryNode In<T>(params T[] values)
        {
            if (values == null) return new NullNode();
            return In<T>((IEnumerable<T>)values);
        }

        public QueryNode Between<T>(T min, T max)
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.RANGE, Value = min, Value2 = max };
        }

        public QueryNode Like(string t)
        {
            if (t == null) return new NullNode();
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.LK, Value = t };
        }

        public FieldPredicate IsNull()
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.NULL, Value = null };
        }

        public FieldPredicate IsNotNull()
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.NOTNULL, Value = null };
        }


        public static Field Named(string name)
        {
            return new Field(name);
        }

        public static Field Default
        {
            get
            {
                return Field.Named("_default");
            }
        }

        public Field(string name)
        {
            Name = name;
        }
    }




    internal class DynamicField : System.Dynamic.DynamicObject
    {
        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            result = Field.Named(binder.Name);
            return true;
        }
    }

    public interface IQueryNodeVisitor
    {
        object Visit(AndNode n);
        object Visit(OrNode n);
        object Visit(FieldPredicate n);
        object Visit(NotNode n);
        object Visit(PlusNode n);
        object Visit(RawQuery n);
    }

    public class NHibernateVisitor : IQueryNodeVisitor
    {

        public object Visit(AndNode n)
        {
            var e1 = n.Left.Accept(this) as ICriterion;
            var e2 = n.Right.Accept(this) as ICriterion;
            return NHibernate.Criterion.Expression.And(e1, e2);
        }

        public object Visit(OrNode n)
        {
            var e1 = n.Left.Accept(this) as ICriterion;
            var e2 = n.Right.Accept(this) as ICriterion;
            return NHibernate.Criterion.Expression.Or(e1, e2);
        }

        public object Visit(FieldPredicate n)
        {
            switch (n.Operator)
            {
                case FieldPredicate.OperatorType.EQ:
                    return Expression.Eq(n.Fld.Name, n.Value);
                case FieldPredicate.OperatorType.RANGE:
                    return Expression.Between(n.Fld.Name, n.Value, n.Value2);
                case FieldPredicate.OperatorType.NOTNULL:
                    return Expression.IsNotNull(n.Fld.Name);
                case FieldPredicate.OperatorType.NULL:
                    return Expression.IsNull(n.Fld.Name);
                case FieldPredicate.OperatorType.LK:
                    return Expression.Like(n.Fld.Name, n.Value);

                    
            }
            throw new NotImplementedException();
        }

        public object Visit(NotNode n)
        {
            throw new NotImplementedException();
        }

        public object Visit(PlusNode n)
        {
            throw new NotImplementedException();
        }

        public object Visit(RawQuery n)
        {
            throw new NotImplementedException();
        }
    }

    public class NHDynQuery
    {
        public static QueryNode BuildQuery(Func<dynamic, QueryNode> fun)
        {
            return fun(new DynamicField());
        }
    }
}
