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
    class callFunction2 : Instruction
    {
        
        private String id;
        private LinkedList<Expression> parameterList = new LinkedList<Expression>();
        public LinkedList<Instruction> argumentList = new LinkedList<Instruction>();


        public callFunction2(int line, int column, string id, LinkedList<Expression> parameterList)
        {
            this.id = id;
            this.parameterList = parameterList;
            setLineColumn(line, column);
        }
        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
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

            Type_ type = Type_.STACK;
            if (f.return_ == Type_.STRING)
            {
                type = Type_.HEAP;
            }

            LinkedList<String> tempsAux = gen.tempsAux;

            if (tempsAux.Count > 0)
            {
                gen.AddCom("Save temps");
                for (int i = 0; i < tempsAux.Count; i++)
                {
                    String tt = gen.newTemp();
                    gen.AddExp(tt, "SP", (environment.getVarCount() + i).ToString(), "+");
                    gen.SetStack(tt, tempsAux.ElementAt(i));
                }
                gen.tempsAux.Clear();
            }


            f.environmentAux.saveVar(f.id, f.return_, type, "var", 0);
            gen.addSP();

            f.environmentAux.prev = aux;
            this.argumentList = f.argumentList;

            int index = 0;
            String temp = gen.newTemp();
            foreach (Argument i in this.argumentList)
            {
                foreach (Access id in i.idList)
                {
                    index++;
                    Return r = this.parameterList.ElementAt(index).compile(environment, lbl_end);
                    f.environmentAux.saveVarActual(id.id, r.type_aux, r.type, "var", index);
                    gen.AddExp(temp, index.ToString());
                    gen.SetStack(temp, r.value);
                }
            }


            //Mover environment 
            int var_count = environment.getVarCount();
            gen.AddExp("SP", "SP", var_count.ToString(), "+");
            gen.addSP(var_count);

            //Call function
            gen.addCall(f.id);
            f.parameterList = this.parameterList;
            object ret = f.compile(f.environmentAux, "", "", "");


            //Retornar environment
            gen.AddExp("SP", "SP", environment.getVarCount().ToString(), "-");
            gen.addSP(-var_count);


            String return_ = gen.newTemp();
            gen.AddExp(return_, "stack[(int)SP]");

            if (tempsAux.Count > 0)
            {
                gen.AddCom("Return temps");
                for (int i = 0; i < tempsAux.Count; i++)
                {
                    String tt = gen.newTemp();
                    gen.AddExp(tt, "SP", (environment.getVarCount() + i).ToString(), "+");
                    gen.AddExp(tempsAux.ElementAt(i), "stack[(int)" + tt + "]");
                    gen.reduceSP();
                }
            }

            b = f.environmentAux.getVar(this.id);
            return new Return(return_, b.type, (Type_)b.value);
        }
        public override object execute(Environment_ environment)
        {
            if (this.line == -1010)
            {
                //Graficar tabla de simbolos en tiempo de ejecucion
                Environment_ tt = environment;
                while (tt.prev != null)
                {
                    tt = tt.prev;
                }
                Graph.generateVar(tt);
                return null;
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
            return null;
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}

