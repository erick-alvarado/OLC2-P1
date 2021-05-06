using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Instructions.Transfer;
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
        private LinkedList<Instruction> code = new LinkedList<Instruction>();

        public Statement(LinkedList<Instruction> code, int line, int column)
        {
            this.code = code;
            setLineColumn(line, column);
        }

        public override object compile(Environment_ environment)
        {
            foreach(Instruction i in this.code)
            {
                i.compile(environment);
            }
            return null;
        }
        public override object execute(Environment_ environment)
        {
            foreach (Instruction instr in this.code)
            {
                try
                {
                    object check = instr.execute(environment);
                    if (check != null) 
                    { 
                        return check;
                    }
                }
                catch (Exception e)
                {
                    Gramm.Analyzer.errors.Add((Error_)e);
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
