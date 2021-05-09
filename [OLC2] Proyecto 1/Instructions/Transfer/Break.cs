using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Transfer
{
    class Break: Instruction
    {
        public String type;
        public Expression e;
        public Break(int line, int column, String type, Expression e = null)
        {
            this.type = type;
            this.e = e;
            setLineColumn(line, column);
        }
        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {
            Generator gen = Generator.getInstance();
            if (this.type == "BREAK")
            {
                gen.addGoto(lbl_break);
            }
            else
            {
                gen.addGoto(lbl_continue);
            }
            return null;
        }
        public override object execute(Environment_ environment)
        {
            return new Break(this.line,this.column,this.type,this.e);
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
}
