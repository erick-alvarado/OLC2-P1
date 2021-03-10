using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Symbol_
{
    public class Error_ : Exception
    {
        public int line, column;
        public String type, msg,scope;

        public Error_(int line, int column, String type, String msg, String scope="")
        {
            this.line = line;
            this.column = column;
            this.type = type;
            this.msg = msg;
            this.scope = scope;
        }
    }
}
