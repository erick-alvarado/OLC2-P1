using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Gramm;
using _OLC2__Proyecto_1.Symbol_;


namespace _OLC2__Proyecto_1.Instructions
{
    class Print : Instruction
    {
        private bool write;
        private LinkedList<Expression> value;
        public Print(LinkedList<Expression> value, bool write)
        {
            this.write = write;
            this.value = value;
        }

        public override object execute(Environment_ environment)
        {
            String texto = "";
            foreach(Expression e in this.value)
            {
                Return result = e.execute(environment);
                texto += result.value;
            }
            Analyzer.output += texto + (write ? "\r\n" : "");
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
