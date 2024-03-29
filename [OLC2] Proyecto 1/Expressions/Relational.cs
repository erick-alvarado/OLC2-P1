﻿using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;

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

        public override Return compile(Environment_ environment,String lbl_end)
        {
            Return leftValue = this.left.compile(environment,lbl_end);
            Return rightValue = this.right.compile(environment,lbl_end);
            Generator gen = Generator.getInstance();
            String lbl = gen.newLabel();
            if(leftValue.type==Type_.HEAP || rightValue.type == Type_.HEAP)
            {
                String temp1 = gen.newTemp();
                String temp2 = gen.newTemp();
                String result = gen.newTemp();
                
                //Mover environment 
                int var_count = environment.getVarCount();
                gen.AddExp("SP", "SP", var_count.ToString(), "+");
                gen.addSP(var_count);

                
                gen.AddExp(temp1, "SP+1");
                gen.AddExp(temp2, "SP+2");

                gen.SetStack(temp1, leftValue.value.ToString());
                gen.SetStack(temp2, rightValue.value.ToString());

                gen.addCode("compareString();");
                
                
                
                gen.AddExp(result, "stack[(int)SP]");

                //Retornar environment
                gen.AddExp("SP", "SP", var_count.ToString(), "-");
                gen.addSP(-var_count);

                gen.addIf(result, "==", "1",lbl);

                return new Return(lbl, Type_.BOOLEAN);


            }
            switch (this.type)
            {
                case RelationalOption.LESS:
                    gen.addIf(leftValue.value.ToString(), "<", rightValue.value.ToString(), lbl);
                    return new Return(lbl, Type_.BOOLEAN);
                case RelationalOption.GREATER:
                    gen.addIf(leftValue.value.ToString(), ">", rightValue.value.ToString(), lbl);
                    return new Return(lbl, Type_.BOOLEAN);
                case RelationalOption.LESSEQ:
                    gen.addIf(leftValue.value.ToString(), "<=", rightValue.value.ToString(), lbl);
                    return new Return(lbl, Type_.BOOLEAN);
                case RelationalOption.GREAEQ:
                    gen.addIf(leftValue.value.ToString(), ">=", rightValue.value.ToString(), lbl);
                    return new Return(lbl, Type_.BOOLEAN);
                case RelationalOption.EQUALSEQUALS:
                    gen.addIf(leftValue.value.ToString(), "==", rightValue.value.ToString(), lbl);
                    return new Return(lbl, Type_.BOOLEAN);
                case RelationalOption.DISTINT:
                    gen.addIf(leftValue.value.ToString(), "!=", rightValue.value.ToString(), lbl);
                    return new Return(lbl, Type_.BOOLEAN);
            }

            return null;
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
                        try
                        {
                            return new Return(Double.Parse(leftValue.value.ToString()) == Double.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                        }
                        catch (Exception)
                        {
                            if (leftValue.type==Type_.STRING && Type_.STRING == rightValue.type)
                            {
                                return new Return(leftValue.value.ToString() == rightValue.value.ToString(), Type_.BOOLEAN);
                            }
                            else
                            {
                                throw new Error_(this.line, this.column, "Semantico", "Comparacion de tipos no validos");
                            }
                        }
                    case RelationalOption.DISTINT:
                        try
                        {
                            return new Return(Double.Parse(leftValue.value.ToString()) != Double.Parse(rightValue.value.ToString()), Type_.BOOLEAN);
                        }
                        catch (Exception)
                        {
                            if (leftValue.type == Type_.STRING && Type_.STRING == rightValue.type)
                            {
                                return new Return(leftValue.value.ToString() != rightValue.value.ToString(), Type_.BOOLEAN);
                            }
                            else
                            {
                                throw new Error_(this.line, this.column, "Semantico", "Comparacion de tipos no validos");
                            }
                        }
                }
            }
            catch(Exception)
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
