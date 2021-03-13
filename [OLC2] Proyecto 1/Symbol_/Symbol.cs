using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
namespace _OLC2__Proyecto_1.Symbol_
{
    public class Symbol
    {
        public object value;
        public String id;
        public Type_ type;
        public String type_name;

        public Symbol(object value, string id, Type_ type,String type_name)
        {
            this.value = value;
            this.id = id;
            this.type = type;
            this.type_name = type_name;
        }
        public Symbol(Symbol fake)
        {
            this.value = fake.value;
            this.id = fake.id;
            this.type = fake.type;
            this.type_name = fake.type_name;
        }
    }
}
