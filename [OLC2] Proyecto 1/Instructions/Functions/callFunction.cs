using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Instructions.Functions;
using _OLC2__Proyecto_1.Reports;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _OLC2__Proyecto_1.Instructions
{
    class callFunction : Expression
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
        public override Return compile(Environment_ environment)
        {
            if (this.line == -1010)
            {
                return new Return(0, Type_.INTEGER);
            }

            Generator gen = Generator.getInstance();
            gen.AddCom("CallFunction");
            Symbol b = environment.getFunc(this.id);
            Function f = (Function)b.value;
            Environment_ aux = f.environmentAux.prev;

            f.environmentAux = new Environment_(null, this.id);

            object aux_value = null;
            Type_ type = Type_.STACK;
            if (f.return_ == Type_.STRING)
            {
                type = Type_.HEAP;
                aux_value = "";
            }

            String temp_stack = gen.newTemp();
            int pos = this.parameterList.Count();

            gen.AddExp(temp_stack, "SP");
            gen.addSP();

            f.environmentAux.saveVar(f.id, f.return_, type, "var",-pos-1);
            f.environmentAux.prev = aux;


            this.argumentList = f.argumentList;
            int index = 0;

            foreach (Expression id in this.parameterList)
            {
                Return r = this.parameterList.ElementAt(index).compile(environment);
                f.environmentAux.saveVarActual(this.argumentList.ElementAt(index).compile(environment).ToString(), r.type_aux, r.type, "var", index - pos);
                index++;
                gen.AddStack(r.value);
            }

            gen.addCall(f.id);

            f.parameterList = this.parameterList;
            object ret = f.compile(f.environmentAux);

            String return_ = gen.newTemp();
            gen.AddExp(return_, "stack[(int)" + temp_stack + "]");
            gen.AddExp("SP", temp_stack);

            b = f.environmentAux.getVar(this.id);
            return new Return(return_,b.type,(Type_)b.value);
        }
        public override Return execute(Environment_ environment)
        {
            if(this.line== -1010)
            {
                throw new Error_(this.line, this.column, "Semantico", "No se puede declarar graficar_ts en una expresion:" + this.id);
            }
            Symbol b = environment.getFunc(this.id);
            if (b == null)
            {
                throw new Error_(this.line, this.column, "Semantico", "No existe la variable:" + this.id);
            }
            Function f = null;
            try
            {
                f = (Function)b.value;
                Environment_ aux = f.environmentAux.prev;

                f.environmentAux = new Environment_(null, this.id);

                if (f.return_ != Type_.VOID)
                {
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
                    f.environmentAux.saveVar(f.id, val, f.return_, "var");
                }
                f.environmentAux.prev = aux;
            }
            catch (Exception)
            {
                throw new Error_(this.line, this.column, "Semantico", "No existe la funcion:" + this.id);
            }
            this.argumentList = f.argumentList;
            int index = 0;
            foreach (Argument i in this.argumentList)
            {
                foreach (Access id in i.idList)
                {
                    Return r = this.parameterList.ElementAt(index).execute(environment);
                    if (r.type != i.type)
                    {
                        throw new Error_(i.line, i.column, "Semantico", "Tipo de parametro incorrecto:" + Enum.GetName(typeof(Type_), r.type) + " se esperaba:" + Enum.GetName(typeof(Type_), i.type));
                    }
                    f.environmentAux.saveVarActual(id.id, r.value, r.type, "var");
                    index++;
                }
            }
            if (this.parameterList.Count != index)
            {
                throw new Error_(this.line, this.column, "Semantico", "Numero incorrecto de arguments");
            }
            f.parameterList = this.parameterList;
            object ret = f.execute(f.environmentAux);
            index = 0;
            //Este metodo solo diosito y yo sabemos lo que hicimos a las 3:57am con desesperacion
            foreach (Argument i in this.argumentList)
            {
                foreach (Access id in i.idList)
                {
                    if (i.rvar)
                    {
                        foreach (Access a in i.idList)
                        {
                            Symbol value = f.environmentAux.getVar(a.getId());
                            try
                            {
                                if (value != null)
                                {
                                    Access atemp = (Access)this.parameterList.ElementAt(index);
                                    environment.saveVar(atemp.getId(), value.value, value.type, value.type_name);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    index++;
                }
            }

            if (ret != null)
            {
                Return temp = (Return)ret;
                if (temp.type.Equals(Type_.ID))
                {
                    try
                    {
                        Symbol aux = f.environmentAux.getVar((String)temp.value);
                        Return n = new Return(aux.value, aux.type);
                        return n;
                    }
                    catch (Exception)
                    {
                        return temp;
                    }
                    
                }
                return temp;
            }

            return new Return(0, Type_.INTEGER);
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
