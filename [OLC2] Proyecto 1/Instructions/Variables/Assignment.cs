using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class Assignment: Instruction
    {
        private String id;
        private Expression expression;

        public Assignment(int line, int column, string id, Expression expression)
        {
            this.id = id;
            this.expression = expression;
            setLineColumn(line, column);

        }
        public override object execute(Environment_ environment)
        {
            Symbol b = environment.getVar(this.id);
            if (b == null)
            {
                throw new Error_(this.line, this.column, "Semantico", "No existe la variable:"+this.id);
            }
            Return newVal = this.expression.execute(environment);
            if (newVal.type != b.type)
            {
                throw new Error_(this.line, this.column, "Semantico", "Asignacion de tipo incorrecto:" + this.id);
            }
            b.value = newVal.value;
            environment.saveVar(b.id, b.value, b.type, b.type_name);
            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
