using _OLC2__Proyecto_1.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Symbol_
{
    class Main : Instruction
    {
        public LinkedList<Instruction> declara;

        public Main(LinkedList<Instruction> declara)
        {
            this.declara = declara;
        }
        public override object execute(Environment_ environment)
        {
            foreach (Instruction i in this.declara)
            {
                object check = i.execute(environment);
                if (check != null)
                {
                    Expressions.Access a = (Expressions.Access)check;
                    throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" +a.id);
                }
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
