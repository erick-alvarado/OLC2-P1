using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;

namespace _OLC2__Proyecto_1.Expressions
{
    class Relational : Expression
    {
        private Expression left, right;
        private RelationalOption type;
        
        // Binary Operations
        public Relational(Expression left, Expression right, RelationalOption type, int line, int column)
        {
            this.left = left;
            this.right = right;
            this.type = type;

            this.setLineColumn(line, column);
        }
        public override Return execute(Environment_ environment)
        {
            Return leftValue = this.left.execute(environment);
            Return rightValue = this.right.execute(environment);
            try
            {
                switch (this.type)
                {
                    case RelationalOption.LESS:
                        return new Return(Double.Parse(leftValue.value.ToString()) < Double.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                    case RelationalOption.GREATER:
                        return new Return(Double.Parse(leftValue.value.ToString()) > Double.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                    case RelationalOption.LESSEQ:
                        return new Return(Double.Parse(leftValue.value.ToString()) <= Double.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                    case RelationalOption.GREAEQ:
                        return new Return(Double.Parse(leftValue.value.ToString()) >= Double.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                    case RelationalOption.EQUALSEQUALS:
                        return new Return(Double.Parse(leftValue.value.ToString()) == Double.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                    case RelationalOption.DISTINT:
                        return new Return(Double.Parse(leftValue.value.ToString()) != Double.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                }
            }
            catch(Exception e)
            {
                throw new Error_(this.line, this.column, "Semantico", "Comparacion de tipos no validos");
            }

            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
    public enum RelationalOption
    {
        LESS = 0,
        GREATER = 1,
        LESSEQ = 2,
        GREAEQ = 3,
        EQUALSEQUALS = 4,
        DISTINT = 5
    }
}
