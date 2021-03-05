using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions
{
    class Statement : Instruction
    {
        private LinkedList<Instruction> code;

        public Statement(LinkedList<Instruction> code, int line, int column)
        {
            this.code = code;
            setLineColumn(line, column);
        }
        public override object execute(Environment_ environment)
        {
            foreach (Instruction instr in this.code)
            {
                try
                {
                    object ret = instr.execute(environment);
                    if (ret != null)
                        return ret;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
