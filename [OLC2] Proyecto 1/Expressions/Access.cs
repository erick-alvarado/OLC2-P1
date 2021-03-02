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

        // Access id
        public Access(String id, int line, int column)
        {
            this.id = id;

            this.setLineColumn(line, column);
        }
        //Hacer un segundo execute para cuando solo se declaran variabless

        public override Return execute(Environment_ environment)
        {
            Symbol vari = environment.getVar(this.id);
            if (vari == null)
                throw new Error_(this.line, this.column, "Semantico", "La variable no existe");
            return new Return(vari.value, vari.type);
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
