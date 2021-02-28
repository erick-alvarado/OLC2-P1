using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
namespace _OLC2__Proyecto_1.Symbol
{
    class Symbol
    {
        public object value;
        public String id;
        public Type_ type;

        public Symbol(object value, string id, Type_ type)
        {
            this.value = value;
            this.id = id;
            this.type = type;
        }
    }
}
