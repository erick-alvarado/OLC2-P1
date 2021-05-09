using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Instructions.Variables;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;
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

        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {
            Generator gen = Generator.getInstance();
            Symbol b = (Symbol)this.assignment.compile(environment, "", "", "");

            String lbl_inicio = gen.newLabel();
            lbl_end = gen.newLabel();
            lbl_continue = gen.newLabel();
            lbl_break = lbl_end;

            gen.addLabel(lbl_inicio);

            Relational exp =  this.up? new Relational(new Access(0, 0, b.id),this.condition,RelationalOption.LESSEQ,0,0): new Relational(new Access(0, 0, b.id), this.condition, RelationalOption.GREAEQ, 0, 0);
            Return ret = exp.compile(environment,"");
            gen.addGoto(lbl_end);

            gen.addLabel(ret.value.ToString());
            this.statements.compile(environment, "", lbl_break, lbl_continue);

            gen.addLabel(lbl_continue);
            Assignment ass = this.up ? new Assignment(0, 0, b.id, null, new Arithmetic(new Access(0, 0, b.id), new Literal(1, Type_.INTEGER, 0, 0), ArithmeticOption.PLUS, 0, 0)) : new Assignment(0, 0, b.id, null, new Arithmetic(new Access(0, 0, b.id), new Literal(1, Type_.INTEGER, 0, 0), ArithmeticOption.MINUS, 0, 0));
            ass.compile(environment, "", "", "");
            gen.addGoto(lbl_inicio);

            gen.addLabel(lbl_end);
            return null;
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
