﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;

namespace _OLC2__Proyecto_1.Expressions
{
    class Access: Expression
    {
        public String id;
        private LinkedList<Expression> expList;
        public Access(int line, int column, String id, LinkedList<Expression> expList=null)
        {
            this.id = id;
            this.expList = expList;
            this.setLineColumn(line, column);
        }
        public override Return compile(Environment_ environment,String lbl_end)
        {
            Generator gen = Generator.getInstance();
            Symbol b = environment.getVar(this.id);
            Symbol aux = environment.getVarActual(this.id);
            if (b.type == Type_.STACK)
            {
                int position = b.position;
                String temp = gen.newTemp();

                if (environment.name== "Global$"|| aux==null)
                {
                    gen.AddExp(temp, b.position.ToString());
                }
                else
                {
                    gen.AddExp(temp,"SP + "+ b.position.ToString());
                }
                String temp2 = gen.newTemp();
                gen.AddExp(temp2, "stack[(int)","]", temp );
                return new Return(temp2, b.type, (Type_)b.value);

            }
            else
            {
                String temp = gen.newTemp();
                gen.AddExp(temp,"stack[(int)" +b.position.ToString()+"]");
                return new Return(temp, Type_.HEAP, Type_.STRING);
            }
        }
        public override Return execute(Environment_ environment)
        {
            if (expList==null || expList.Count == 0)
            {
                Symbol vari = environment.getVar(this.id);
                if (vari == null)
                {
                    throw new Error_(this.line, this.column, "Semantico", "La variable no existe: " + this.id);

                }
                return new Return(vari.value, vari.type);
            }
            else
            {
                //Asignacion objecto
                Environment_ auxEnvironment = environment;
                Return retorno = null;
                Symbol b = null;
                try
                {
                    //Buscamos el primer object y se obtiene el environment del mismo 
                    b = environment.getVar(this.id);
                    Environment_ gg = (Environment_)b.value;
                    auxEnvironment = gg;
                }
                catch (Exception)
                {
                    throw new Error_(this.line, this.column, "Semantico", "No se encuentra el object:" + this.id);
                }
                bool enciclado = false;
                //Se ejecutan las expresiones y se verifica si esta en un ambito environment o si ya finalizo
                foreach (Expression e in this.expList)
                {
                    if (b.type == Type_.ID)
                    {
                        auxEnvironment = (Environment_)b.value;
                    }
                    auxEnvironment.prev = environment;
                    try
                    {
                        Access va = (Access)e;
                        if (va.id == "$")
                        {
                            enciclado = true;
                            continue;
                        }
                        retorno = new Return(va.id, Type_.DEFAULT);
                        if (enciclado)
                        {
                            enciclado = false;
                            retorno = e.execute(auxEnvironment);
                        }
                    }
                    catch (Exception)
                    {
                        enciclado = false;
                        retorno = e.execute(auxEnvironment);//valor de retorno de ejecutar una expresion sin saber que sea
                    }
                    b = auxEnvironment.getVar(retorno.value.ToString());
                   
                    if (b == null)
                    {
                        throw new Error_(this.line, this.column, "Semantico", "No se encuentra el atributo:" + retorno.value);
                    }
                    
                }
                return new Return(b.value, b.type);
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
