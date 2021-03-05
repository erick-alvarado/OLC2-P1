using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Transfer
{
    class Break: Instruction
    {
        private String type;
        public Break(int line, int column, String type)
        {
            this.type = type;
            setLineColumn(line, column);
        }
        public override object execute(Environment_ environment)
        {
            return new Expressions.Access(this.type,this.line,this.column);
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
}
