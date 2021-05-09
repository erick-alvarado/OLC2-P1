using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;

namespace _OLC2__Proyecto_1.Expressions
{
    class Logical : Expression
    {
        private Expression left, right;
        private LogicalOption type;

       
        public Logical(Expression left, Expression right, LogicalOption type, int line, int column)
        {
            this.left = left;
            this.right = right;
            this.type = type;

            this.setLineColumn(line, column);
        }


        //Minus
        public Logical(Expression right, LogicalOption type, int line, int column)
        {
            this.right = right;
            this.type = type;

            this.setLineColumn(line, column);
        }

        public override Return compile(Environment_ environment, String lbl_end)
        {
            Generator gen = Generator.getInstance();
            Return rightValue = this.right.compile(environment,lbl_end);

            if (this.left == null)
            {
                String lbl = gen.newLabel();
                gen.addGoto(lbl);
                gen.addLabel(rightValue.value.ToString());
                gen.addGoto(lbl_end);
                return new Return(lbl, Type_.BOOLEAN);
            }


            Return leftValue = this.left != null ? this.left.compile(environment,lbl_end) : new Return(0, Type_.INTEGER);

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
        public override Return execute(Environment_ environment)
        {
            Return leftValue = this.left != null ? this.left.execute(environment) : new Return(0, Type_.INTEGER);
            Return rightValue = this.right.execute(environment);
            try
            {
                switch (this.type)
                {
                    case LogicalOption.NOT:
                        return new Return(!Boolean.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                    case LogicalOption.AND:
                        return new Return(Boolean.Parse(leftValue.value.ToString()) && Boolean.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                    case LogicalOption.OR:
                        return new Return(Boolean.Parse(leftValue.value.ToString()) || Boolean.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                    default:
                        throw new Error_(this.line, this.column, "Semantico", "Operacion logica sobre un tipo de dato incorrecto");
                }
            }
            catch(Exception)
            {
                throw new Error_(this.line, this.column, "Semantico", "Operacion logica sobre un tipo de dato incorrecto");
            }
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
    public enum LogicalOption
    {
        NOT = 0,
        AND = 1,
        OR = 2
    }
}
