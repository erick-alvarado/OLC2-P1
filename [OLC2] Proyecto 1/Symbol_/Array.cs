using _OLC2__Proyecto_1.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Symbol_
{
    public class Array
    {
        public String name;
        public int start, end;
        public Type_ type;
        public object[] value= new object[1];

        public Array(int start, int end,String name, object val, Type_ type)
        {
            this.start = start;
            this.end = end;
            this.name = name;
            this.type = type;
            int range = System.Math.Abs(start) + System.Math.Abs(end);
            this.value = new object[range];
            fillArray(val);
        }
        private void fillArray(object val)
        {
            for(int i = this.start; i<end; i++)
            {
                this.value[i] = val;
            }
        }
        public void saveVar(int pos, object value, int line , int column)
        {
            if (pos < this.start || pos > this.end)
            {
                throw new Error_(line, column, "Semantico", "Asignacion fuera de limites de la lista:"+this.name);
            }
            else
            {
                try{
                    object v = value;
                    this.value[pos] = value;
                }
                catch (Exception e)
                {
                    throw new Error_(line, column, "Semantico", "Asignacion de tipo incorrecto a la lista:" + this.name);
                }

            }

        }
        public Return getVar(int pos)
        {
            return new Return(this.value[pos],this.type);
        }
    }
}
