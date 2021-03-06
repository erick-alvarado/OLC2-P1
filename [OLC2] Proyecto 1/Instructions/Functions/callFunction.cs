﻿using _OLC2__Proyecto_1.Abstract;
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
    class callFunction: Expression
    {
        private String id;
        private LinkedList<Expression> parameterList = new LinkedList<Expression>();
        public LinkedList<Instruction> argumentList = new LinkedList<Instruction>();
        
        public callFunction(int line, int column, string id, LinkedList<Expression> parameterList)
        {
            this.id = id;
            this.parameterList = parameterList;
            setLineColumn(line, column);
        }
        public override Return execute(Environment_ environment)
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
                object val = null;
                switch (f.return_)
                {
                    case Type_.BOOLEAN:
                        val = false;
                        break;
                    case Type_.STRING:
                        val = "";
                        break;
                    case Type_.INTEGER:
                        val = 0;
                        break;
                    case Type_.REAL:
                        val = 0.000000000000000000000000;
                        break;
                }
                f.environmentAux.saveVar(this.id, val, f.return_, "var");
                foreach (Argument i in this.argumentList)
                {
                    foreach (Access id in i.idList)
                    {
                        Return r = this.parameterList.ElementAt(index).execute(environment);
                        if (r.type != i.type)
                        {
                            throw new Error_(i.line, i.column, "Semantico", "Tipo de parametro incorrecto:"+Enum.GetName(typeof(Type_),r.type)+" se esperaba:"+ Enum.GetName(typeof(Type_), i.type));
                        }
                        f.environmentAux.saveVar(id.id,r.value,r.type,"var");
                        index++;
                    }
                }
                if (this.parameterList.Count != index)
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
            return new Return(0,Type_.INTEGER);
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
