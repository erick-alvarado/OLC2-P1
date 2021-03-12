using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Instructions.Variables;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Loops
{
    class For: Instruction
    {
        private Assignment assignment;
        private Expression condition;
        private Statement statements;
        private bool up;

        public For(int line, int column,Assignment assignment, Expression condition, Statement statements, bool up)
        {
            this.condition = condition;
            this.statements = statements;
            this.assignment = assignment;
            this.up = up;
            setLineColumn(line, column);

        }
        public override object execute(Environment_ environment)
        {
            this.assignment.execute(environment);
            Symbol initial = environment.getVar(this.assignment.getId());

            Return condition = this.condition.execute(environment);
            
            if (initial.type != Type_.INTEGER)
            {
                throw new Error_(this.line, this.column, "Semantico", "El tipo de dato debe ser integer en la asignacion");
            }
            if (condition.type != Type_.INTEGER)
            {
                throw new Error_(this.line, this.column, "Semantico", "La condicion debe ser integer");
            }
            int F = 0;
            bool cond;
            if (this.up)
            {
                cond = (int)initial.value <= (int)condition.value;
            }
            else
            {
                cond = (int)initial.value >= (int)condition.value;
            }
            while (cond)
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
                if (F > 1000)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Ciclo sin fin");
                }
                initial = environment.getVar(this.assignment.getId());
                if (this.up){
                    initial.value = (int)initial.value + 1;
                    cond = (int)initial.value <= (int)condition.value;
                }
                else
                {
                    initial.value = (int)initial.value - 1;
                    cond = (int)initial.value >= (int)condition.value;
                }
                environment.saveVar(initial.id, initial.value, initial.type, initial.type_name);
                
            }
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
