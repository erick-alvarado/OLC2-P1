using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
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
        public override object execute(Environment_ environment)
        {
            Return condition;
            bool var; 
            int F = 0;
            do
            {
                F++;
                object br = statements.execute(environment);
                if (br != null)
                {
                    Expressions.Access a = (Expressions.Access)br;
                    if (a.id.Equals("BREAK"))
                    {
                        break;
                    }
                    else if (a.id.Equals("CONTINUE"))
                    {
                    }
                    else
                    {
                        throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.id);
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
