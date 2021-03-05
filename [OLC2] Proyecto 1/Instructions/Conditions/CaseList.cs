using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;

namespace _OLC2__Proyecto_1.Instructions.Conditions
{
    class CaseList: Instruction
    {
        private LinkedList<Expression> expressionList;
        private Statement statements;
        private Return temp;
        public CaseList(LinkedList<Expression> expressionList, Statement statements)
        {
            this.expressionList = expressionList;
            this.statements = statements;
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
                        Expressions.Access a = (Expressions.Access)check;
                        if (a.id.Equals("CONTINUE"))
                        {
                            throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.id);
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


        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
