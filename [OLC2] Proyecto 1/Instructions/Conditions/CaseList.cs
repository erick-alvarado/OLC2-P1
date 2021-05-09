using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;

namespace _OLC2__Proyecto_1.Instructions.Conditions
{
    class CaseList: Instruction
    {
        private LinkedList<Expression> expressionList;
        private Statement statements;
        private Return temp;

        private Expression temp_compile;

        public CaseList(LinkedList<Expression> expressionList, Statement statements)
        {
            this.expressionList = expressionList;
            this.statements = statements;
        }

        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {
            Generator gen = Generator.getInstance();
            foreach(Expression e in this.expressionList)
            {
                gen.AddCom("Case");
                Expressions.Relational relation = new Expressions.Relational(this.temp_compile, e, Expressions.RelationalOption.EQUALSEQUALS, 0, 0);
                Return ret = relation.compile(environment,lbl_end);

                lbl_end = gen.newLabel();
                gen.addGoto(lbl_end);

                gen.addLabel(ret.value.ToString());
                this.statements.compile(environment, "", lbl_break,lbl_continue) ;
                gen.addGoto(lbl_end);

                gen.addLabel(lbl_end);
            }
            return null;
        }
        public override object execute(Environment_ environment)
        {
            Return val;
            foreach(Expression e in expressionList)
            {
                val = e.execute(environment);
                if(val.type != this.temp.type)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Comparacion de tipos incorrecto en switch");
                }
                if (temp.value.Equals(val.value))
                {
                    object check = this.statements.execute(environment);
                    if (check != null)
                    {
                        try
                        {
                            Break a = (Break)check;
                            if (a.type.Equals("CONTINUE"))
                            {
                                throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.type);
                            }
                            else
                            {
                                return a;
                            }
                        }
                        catch (Exception)
                        {
                            return check;
                        }
                        
                    }
                    return true;
                }
            }
            return false;
        }
        public void setComparable(Return temp)
        {
            this.temp = temp;
        }

        public void setComparable(Expression temp)
        {
            this.temp_compile = temp;
        }


        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
