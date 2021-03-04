using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class Declaration : Instruction
    {
        private String id;
        private Expression value;

        public Declaration(string id, Expression value, int line, int column)
        {
            this.id = id;
            this.value = value;
            setLineColumn(line, column);
        }

        public override object execute(Environment_ environment)
        {
            if (environment.getVar(this.id) != null)
            {
                throw new Error_(this.line, this.column, "Semantico", "Declaracion de una variable ya existente:" + this.id);
            }
            else
            {
                Return val = this.value != null ? this.value.execute(environment) : new Return(null, Type_.INTEGER);
                environment.saveVar(this.id, val.value, val.type, "cons");
            }
            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }

    }
}
