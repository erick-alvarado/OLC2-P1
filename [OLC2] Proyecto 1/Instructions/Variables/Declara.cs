using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class Declara:Instruction
    {
        public LinkedList<Instruction> declara;

        public Declara(LinkedList<Instruction> declara)
        {
            this.declara = declara;
        }
        public override object execute(Environment_ environment)
        {
            foreach(Instruction i in this.declara)
            {
                i.execute(environment);
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
