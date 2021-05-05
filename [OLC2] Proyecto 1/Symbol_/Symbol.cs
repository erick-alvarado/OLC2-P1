using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Instructions.Functions;
using Force.DeepCloner;

namespace _OLC2__Proyecto_1.Symbol_
{
    public class Symbol
    {
        public object value;//Temporal
        public String id;
        public Type_ type;//Stack / heap
        public String type_name;
        public object aux_value;//data
        public int position;
        public Symbol(object value, string id, Type_ type, String type_name, object aux_value, int position)
        {
            try
            {
                Environment_ pp = (Environment_)value;
                value = value.DeepClone();
            }
            catch (Exception)
            {

            }
            this.value = value;
            this.id = id;
            this.type = type;
            this.type_name = type_name;
            this.aux_value = aux_value;
            this.position = position;
        }
    }
}
