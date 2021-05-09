﻿using _OLC2__Proyecto_1.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Symbol_
{
    class Empty: Instruction
    {
        public Empty()
        {

        }
        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {
            return null;
        }
        public override object execute(Environment_ environment)
        {
            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
}
