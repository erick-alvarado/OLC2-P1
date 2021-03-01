using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Symbol_
{
    class Error_ : Exception
    {
        public int line, column;
        public String type, msg;

        public Error_(int line, int column, String type, String msg)
        {
            this.line = line;
            this.column = column;
            this.type = type;
            this.msg = msg;
        }
    }
}
