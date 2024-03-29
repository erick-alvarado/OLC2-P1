﻿using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Instructions.Transfer;
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
        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {
            foreach (Instruction i in this.declara)
            {
                i.compile(environment,"","","");
            }
            return null;
        }
        public override object execute(Environment_ environment)
        {
            foreach(Instruction i in this.declara)
            {
                try
                {
                    object check = i.execute(environment);
                    if (check != null)
                    {
                        Break a = (Break)check;
                        Gramm.Analyzer.errors.Add(new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.type));
                        //throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.type);
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
            this.line = line;
            this.column = column;
        }
    }
}
