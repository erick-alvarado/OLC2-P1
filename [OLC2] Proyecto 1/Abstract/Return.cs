using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Abstract
{
    public class Return
    {
        public object value;
        public Type_ type;

        public Return(object value, Type_ type)
        {
            this.value = value;
            this.type = type;
        }
    }
    public enum Type_
    {
        INTEGER = 0,
        REAL = 1,
        STRING = 2,
        BOOLEAN =3,
        NULL=4,
        BREAK=5,
        CONTINUE=6,
        ID = 7,
        SUBRANGE = 8,
        DEFAULT =9

    }
}
