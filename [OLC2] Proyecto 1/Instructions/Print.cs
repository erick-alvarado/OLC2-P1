using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Gramm;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;

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
        public override object compile(Environment_ environment)
        {
            Generator gen = Generator.getInstance();
            gen.AddCom("Print");

            foreach (Expression e in this.value)
            {
                Return ret = e.compile(environment);
                if(ret.type== Type_.STACK)
                {
                    if ((Type_)ret.type_aux == Type_.INTEGER)
                    {
                        gen.addPrint("d", ret.value.ToString());
                    }
                    else
                    {
                        gen.addPrint("f", ret.value.ToString());

                    }
                }
                else
                {
                    if ((Type_)ret.type_aux == Type_.BOOLEAN)
                    {

                    }
                }
            }
            if (write)
            {
                gen.printSpace();
            }
            return null;
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
