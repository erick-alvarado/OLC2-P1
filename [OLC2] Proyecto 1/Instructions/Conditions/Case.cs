﻿using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;

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
        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {
            Generator gen = Generator.getInstance();
            lbl_end = gen.newLabel();

            gen.AddCom("Switch");
            foreach (CaseList c in this.caseList)
            {
                c.setComparable(this.value);
                c.compile(environment,lbl_end,lbl_break,lbl_continue);
            }


            if (this.statements != null)
            {
                this.statements.compile(environment,"",lbl_break,lbl_continue);
                gen.addGoto(lbl_end);

            }
            if (lbl_end != "")
            {
                gen.addLabel(lbl_end);
            }

            return null;
        }
        public override object execute(Environment_ environment)
        {
            Return val = this.value.execute(environment);
            bool exec;
            foreach(CaseList c in this.caseList)
            {
                c.setComparable(val);
                object check = c.execute(environment);
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
                    try
                    {
                        exec = (bool)check;
                        if (exec == true)
                        {
                            return null;
                        }
                    }
                    catch (Exception)
                    {
                        return check;
                    }
                    
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
