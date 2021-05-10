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
        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {
            Generator gen = Generator.getInstance();

            foreach (Expression e in this.value)
            {
                Return ret = e.compile(environment,"");
                gen.AddCom("Print");
                if (ret.type== Type_.STACK)
                {
                    switch((Type_)ret.type_aux)
                    {
                        case Type_.INTEGER:
                            gen.addPrint("d", ret.value.ToString());
                            break;
                        case Type_.REAL:
                            gen.addPrint("f", ret.value.ToString());
                            break;
                        case Type_.BOOLEAN:
                            String lbl = gen.newLabel();
                            String lbl_salida = gen.newLabel();
                            gen.addIf(ret.value.ToString(), "==", "0", lbl);
                            gen.printTrue();
                            gen.addGoto(lbl_salida);
                            gen.addLabel(lbl);
                            gen.printFalse();
                            gen.addLabel(lbl_salida);
                            break;
                    }
                }
                else
                {
                    //Mover environment 
                    int var_count = environment.getVarCount();
                    gen.AddExp("SP", "SP", var_count.ToString(), "+");
                    gen.addSP(var_count);
                    
                    //Add heap value
                    String temp = gen.newTemp();
                    gen.AddExp(temp, "SP+1");
                    gen.SetStack(temp, ret.value.ToString());

                    gen.addCode("printString();");
                    
                    //Retornar environment
                    gen.AddExp("SP", "SP", var_count.ToString(), "-");
                    gen.addSP(-var_count);

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
