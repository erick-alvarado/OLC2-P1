﻿using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Loops
{
    class While: Instruction
    {
        private Expression condition;
        private Statement statements;

        public While(int line, int column, Expression condition, Statement statements)
        {
            this.condition = condition;
            this.statements = statements;
            setLineColumn(line, column);

        }
        public override object execute(Environment_ environment)
        {
            Return condition = this.condition.execute(environment);
            if (condition.type != Type_.BOOLEAN)
            {
                throw new Error_(this.line, this.column, "Semantico", "La condición no es booleana");
            }

            bool var = (bool)condition.value;
            int F = 0;
            while (var)
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
                        continue;
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
            }
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
