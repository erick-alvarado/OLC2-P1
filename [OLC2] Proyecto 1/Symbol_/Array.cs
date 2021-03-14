using _OLC2__Proyecto_1.Abstract;
using Force.DeepCloner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Symbol_
{
    public class Array_
    {
        private int start, end;
        private Type_ type;
        public Environment_ environment;
        private String type_name;
        public Array_(int start, int end,String name, object val, Type_ type, String type_name)
        {
            this.start = start;
            this.end = end;
            this.type = type;
            this.environment = new Environment_(null, name);
            this.type_name = type_name;
            fillArray(val);
        }
        private void fillArray(object val)
        {
            for(int i = this.start; i<=end; i++)
            {
                this.environment.saveVar(i.ToString(), val.DeepClone(), this.type, this.type_name);
            }
        }
    }
}
