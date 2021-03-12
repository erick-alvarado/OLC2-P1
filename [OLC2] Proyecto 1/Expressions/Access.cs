using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;



namespace _OLC2__Proyecto_1.Expressions
{
    class Access: Expression
    {
        public String id;
        public String id2;
        public Access(int line, int column, String id,String id2="" )
        {
            this.id = id;
            this.id2 = id2;
            this.setLineColumn(line, column);
        }
        public override Return execute(Environment_ environment)
        {
            Symbol vari = environment.getVar(this.id);
            if (vari == null)
            {
                throw new Error_(this.line, this.column, "Semantico", "La variable no existe: " + this.id);

            }
            if (id2 == "")
            {
                return new Return(vari.value, vari.type);
            }
            else
            {
                try
                {
                    Environment_ e = (Environment_)vari.value;
                    Symbol aux = e.getVar(id2);
                    if (aux == null)
                    {
                        throw new Error_(this.line, this.column, "Semantico", "La variable no existe: " + this.id2);
                    }
                    if (aux.type_name == "type")
                    {
                        throw new Error_(this.line, this.column, "Semantico", "Se esperaba una variable y se obtuvo type: " + this.id2);
                    }
                    return new Return(aux.value, aux.type);

                }
                catch (Exception t)
                {
                    throw new Error_(this.line, this.column, "Semantico", "La variable no existe no posee atributos: " + this.id);
                }
            }
            
        }
        public String getId()
        {
            return this.id;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
