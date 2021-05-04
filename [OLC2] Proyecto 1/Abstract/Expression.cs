using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Symbol_;

namespace _OLC2__Proyecto_1.Abstract
{
    public abstract class Expression
    {
        public int line, column;
        public abstract Return compile(Environment_ environment);
        public abstract Return execute(Environment_ environment);
        public abstract void setLineColumn(int line, int column);
    }
}
