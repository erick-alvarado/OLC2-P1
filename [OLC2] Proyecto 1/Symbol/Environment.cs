using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
namespace _OLC2__Proyecto_1.Symbol
{
    public class Environment_
    {
        private LinkedList<Symbol> variables;
        public Environment_ prev;
        public Environment_(Environment_ prev = null)
        {
            this.prev = prev;
            this.variables = new LinkedList<Symbol>();
        }


    }
}
