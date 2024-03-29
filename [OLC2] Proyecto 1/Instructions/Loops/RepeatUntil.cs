﻿using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Loops
{
    class RepeatUntil: Instruction
    {
        private Expression condition;
        private Statement statements;

        public RepeatUntil(int line, int column, Expression condition, Statement statements)
        {
            this.condition = condition;
            this.statements = statements;
            setLineColumn(line, column);

        }
        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {
            Generator gen = Generator.getInstance();
            String lbl1 = gen.newLabel();
            lbl_end = gen.newLabel();
            lbl_continue = lbl1;
            lbl_break = lbl_end;

            gen.addLabel(lbl1);
            this.statements.compile(environment, "", lbl_break, lbl_continue);
            Expressions.Logical log = new Expressions.Logical(this.condition,Expressions.LogicalOption.NOT,0,0);
            Return condition = log.compile(environment,lbl_end);
            gen.addGoto(lbl_end);
            gen.addLabel(condition.value.ToString());
            gen.addGoto(lbl1);
            gen.addLabel(lbl_end);
            return null;
        }
        public override object execute(Environment_ environment)
        {
            Return condition;
            bool var=false; 
            int F = 0;
            do
            {
                F++;
                object br = statements.execute(environment);
                if (br != null)
                {
                    Break a = (Break)br;
                    if (a.type.Equals("BREAK"))
                    {
                        break;
                    }
                    else if (a.type.Equals("CONTINUE"))
                    {
                        
                    }
                    else
                    {
                        return a;
                    }
                }
                condition = this.condition.execute(environment);
                var = (bool)condition.value;
                if (F > 1000)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Ciclo sin fin");
                }
                if (condition.type != Type_.BOOLEAN)
                {
                    throw new Error_(this.line, this.column, "Semantico", "La condición no es booleana");
                }
            } while (!var);
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
        
    }
}
