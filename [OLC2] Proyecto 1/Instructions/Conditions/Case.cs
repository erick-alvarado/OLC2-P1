using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Symbol_;

namespace _OLC2__Proyecto_1.Instructions.Conditions
{
    class Case : Instruction
    {
        private Expression value;
        private LinkedList<CaseList> caseList= new LinkedList<CaseList>();
        private Statement statements;

        public Case(int line, int column,Expression value, LinkedList<CaseList> caseList, Statement statements=null)
        {
            this.value = value;
            this.caseList = caseList;
            this.statements = statements;
            setLineColumn(line, column);
        }

        public override object execute(Environment_ environment)
        {
            Return val = this.value.execute(environment);
            bool exec;
            foreach(CaseList c in this.caseList)
            {
                c.setComparable(val);
                exec = (bool) c.execute(environment);
                if (exec == true)
                {
                    return null;
                }
            }
            if (this.statements != null)
            {
                object check = this.statements.execute(environment);
                if (check != null)
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
            }
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
