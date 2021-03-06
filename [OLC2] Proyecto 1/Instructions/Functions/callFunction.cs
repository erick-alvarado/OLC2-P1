using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Instructions.Functions;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions
{
    class callFunction: Instruction
    {
        private String id;
        private LinkedList<Expression> parameterList = new LinkedList<Expression>();
        public LinkedList<Argument> argumentList = new LinkedList<Argument>();
        
        public callFunction(int line, int column, string id, LinkedList<Expression> parameterList)
        {
            this.id = id;
            this.parameterList = parameterList;
            setLineColumn(line, column);
        }
        public override object execute(Environment_ environment)
        {
            Symbol b = environment.getVar(this.id);
            if (b == null)
            {
                throw new Error_(this.line, this.column, "Semantico", "No existe la variable:" + this.id);
            }
            try
            {
                Function f = (Function)b.value;
                this.argumentList = f.argumentList;
                int index = 0;
                foreach (Argument i in this.argumentList)
                {
                    foreach (Access id in i.idList)
                    {
                        Return r = this.parameterList.ElementAt(index).execute(environment);
                        if (r.type != i.type)
                        {
                            throw new Error_(i.line, i.column, "Semantico", "Tipo de parametro incorrecto:"+Enum.GetName(typeof(Type_),r.type)+" se esperaba:"+ Enum.GetName(typeof(Type_), i.type));
                        }
                        index++;
                    }
                }
                if (this.parameterList.Count != index+1)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Numero incorrecto de arguments");
                }
                f.parameterList = this.parameterList;
                object ret = f.execute(environment);
                
                if (ret != null)
                {
                    Return temp = (Return)ret;
                    if (temp.type.Equals(Type_.ID))
                    {
                        Symbol aux = f.environmentAux.getVar((String)temp.value);
                        Return n = new Return(aux.value, aux.type);
                        return n;
                    }
                    return temp;
                }
            }
            catch(Exception e)
            {
                throw new Error_(this.line, this.column, "Semantico", "No existe la funcion:"+this.id);
            }
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
