using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions
{
    class Argument: Instruction
    {
        public LinkedList<Access> idList = new LinkedList<Access>();
        public Type_ type;
        public bool rvar;

        public Argument(int line, int column, LinkedList<Access> idList, Type_ type, bool rvar)
        {
            this.idList = idList;
            this.type = type;
            this.rvar = rvar;
            setLineColumn(line, column);
        }
        public override object compile(Environment_ environment)
        {
            return this.idList.ElementAt(0).id;
        }
        public override object execute(Environment_ environment)
        {
            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
       
    }
}
