using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;

namespace _OLC2__Proyecto_1.Expressions
{
    public enum LogicalOption
    {
        NOT = 0,
        AND = 1,
        OR = 2
    }
    class Logical : Expression
    {
        private Expression left, right;
        private LogicalOption type;

        // Binary Operations
        public Logical(Expression left, Expression right, LogicalOption type, int line, int column)
        {
            this.left = left;
            this.right = right;
            this.type = type;

            this.setLineColumn(line, column);
        }
        // Unary Operations
        public Logical(Expression right, LogicalOption type, int line, int column)
        {
            this.right = right;
            this.type = type;

            this.setLineColumn(line, column);
        }
        public override Return execute(Environment_ environment)
        {
            Return leftValue = this.left != null ? this.left.execute(environment) : new Return(0, Type_.INTEGER);
            Return rightValue = this.right.execute(environment);

            switch (this.type)
            {
                case LogicalOption.NOT:
                    return new Return(!Boolean.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                case LogicalOption.AND:
                    return new Return(Boolean.Parse(leftValue.value.ToString()) && Boolean.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                case LogicalOption.OR:
                    return new Return(Boolean.Parse(leftValue.value.ToString()) || Boolean.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
            }
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
