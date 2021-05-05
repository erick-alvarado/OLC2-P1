using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;

namespace _OLC2__Proyecto_1.Expressions
{
    public enum ArithmeticOption
    {
        PLUS = 0,
        MINUS = 1,
        TIMES = 2,
        DIV = 3,
        MODULE = 4
    }
    class Arithmetic : Expression
    {
        private Expression left, right;
        private ArithmeticOption type;
        
        public Arithmetic(Expression left, Expression right, ArithmeticOption type, int line, int column)
        {
            this.left = left;
            this.right = right;
            this.type = type;

            this.setLineColumn(line, column);
        }

        public Arithmetic(Expression right, ArithmeticOption type, int line, int column)
        {
            this.right = right;
            this.type = type;

            this.setLineColumn(line, column);
        }
        public override Return compile(Environment_ environment)
        {
            Generator gen = Generator.getInstance();
            Return leftValue = this.left != null ? this.left.compile(environment) : new Return(0, Type_.INTEGER);
            Return rightValue = this.right.compile(environment);
            String temp = gen.newTemp();

            if (leftValue.type == Type_.HEAP || rightValue.type == Type_.HEAP)
            {
                Literal aux = new Literal(leftValue.aux_value.ToString() + rightValue.aux_value.ToString(), Type_.STRING, 0, 0);
                Return ret = aux.compile(environment);
                return new Return(ret.value, Type_.HEAP,ret.aux_value);
            }

            switch (this.type)
            {
                case ArithmeticOption.PLUS:
                    gen.AddExp(temp, leftValue.value.ToString(), rightValue.value.ToString(), " + ");
                    break;
                case ArithmeticOption.MINUS:
                    gen.AddExp(temp, leftValue.value.ToString(), rightValue.value.ToString(), " - ");
                    break;
                case ArithmeticOption.TIMES:
                    gen.AddExp(temp, leftValue.value.ToString(), rightValue.value.ToString(), " * ");
                    break;
                default:
                    gen.AddExp(temp, leftValue.value.ToString(), rightValue.value.ToString(), " / ");
                    break;
            }
            return new Return(temp, Type_.STACK);

        }
        public override Return execute(Environment_ environment)
        {
            Return leftValue = this.left != null ? this.left.execute(environment) : new Return(0, Type_.INTEGER);
            Return rightValue = this.right.execute(environment);
            try
            {
                switch (this.type)
                {
                    case ArithmeticOption.PLUS:
                        if ((leftValue.type == Type_.STRING || rightValue.type == Type_.STRING ))
                        {
                            return new Return(leftValue.value.ToString() + rightValue.value.ToString(), Type_.STRING);
                        }
                        else if (leftValue.type == Type_.INTEGER && rightValue.type == Type_.INTEGER)
                        {
                            return new Return(int.Parse(leftValue.value.ToString()) + int.Parse(rightValue.value.ToString()), Type_.INTEGER);
                        }
                        else if (leftValue.type == Type_.REAL || rightValue.type == Type_.REAL)
                        {
                            if ((leftValue.type == Type_.INTEGER || rightValue.type == Type_.INTEGER) || (leftValue.type == Type_.REAL && rightValue.type == Type_.REAL))
                            {
                                return new Return(Double.Parse(leftValue.value.ToString()) + Double.Parse(rightValue.value.ToString()), Type_.REAL);
                            }
                        }
                        throw new Error_(this.line, this.column, "Semantico", "No se puede sumar " + leftValue.type.ToString() + " con " + rightValue.type.ToString());
                    case ArithmeticOption.MINUS:
                        if (leftValue.type == Type_.REAL || rightValue.type == Type_.REAL)
                        {
                            return new Return(Double.Parse(leftValue.value.ToString()) - Double.Parse(rightValue.value.ToString()), Type_.REAL);
                        }
                        else if (leftValue.type == Type_.INTEGER && rightValue.type == Type_.INTEGER)
                        {
                            return new Return(int.Parse(leftValue.value.ToString()) - int.Parse(rightValue.value.ToString()), Type_.INTEGER);
                        }
                        else
                        {
                            throw new Error_(this.line, this.column, "Semantico", "No se puede restar " + leftValue.type.ToString() + " con " + rightValue.type.ToString());
                        }
                    case ArithmeticOption.TIMES:
                        if (leftValue.type == Type_.REAL || rightValue.type == Type_.REAL)
                        {
                            return new Return(Double.Parse(leftValue.value.ToString()) * Double.Parse(rightValue.value.ToString()), Type_.REAL);
                        }
                        else if (leftValue.type == Type_.INTEGER && rightValue.type == Type_.INTEGER)
                        {
                            return new Return(int.Parse(leftValue.value.ToString()) * int.Parse(rightValue.value.ToString()), Type_.INTEGER);
                        }
                        else
                        {
                            throw new Error_(this.line, this.column, "Semantico", "No se puede multiplicar " + leftValue.type.ToString() + " con " + rightValue.type.ToString());
                        }
                    default:
                        if (rightValue.Equals(0))
                        {
                            throw new Error_(this.line, this.column, "Semantico", "No se puede dividir sobre 0 ");
                        }
                        if (leftValue.type == Type_.REAL || rightValue.type == Type_.REAL)
                        {
                            return new Return(Double.Parse(leftValue.value.ToString()) / Double.Parse(rightValue.value.ToString()), Type_.REAL);
                        }
                        else if (leftValue.type == Type_.INTEGER && rightValue.type == Type_.INTEGER)
                        {
                            return new Return(int.Parse(leftValue.value.ToString()) / int.Parse(rightValue.value.ToString()), Type_.INTEGER);
                        }
                        else
                        {
                            throw new Error_(this.line, this.column, "Semantico", "No se puede dividir " + leftValue.type.ToString() + " con " + rightValue.type.ToString());
                        }
                }

            }
            catch (Exception e )
            {
                throw new Error_(this.line, this.column, "Semantico",e.Message);
            }
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
}
