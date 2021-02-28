using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Gramm
{
    class Error
    {
        private int line;
        private int column;
        private string type;
        private string description;
        private string scope;

        public Error(int line, int column, string type, string description, string scope)
        {
            this.line = line;
            this.column = column;
            this.type = type;
            this.description = description;
            this.scope = scope;
        }
    }
}
