﻿using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class Assignment: Instruction
    {
        private String id;
        private LinkedList<Expression> expList = new LinkedList<Expression>();
        private Expression value;

        public Assignment(int line, int column, string id, LinkedList<Expression> expList, Expression value)
        {
            this.id = id;
            this.expList = expList;
            this.value = value;
            setLineColumn(line, column);
        }
        public override object compile(Environment_ environment, String lbl_end, String lbl_break, String lbl_continue)
        {

            Generator gen = Generator.getInstance();
            gen.AddCom("Asignation");
            Symbol var_asignation = environment.getVar(this.id);

            Return val = value.compile(environment,"");//ret.type = STACK|HEAP   ret.value = temp_final | pos_heap
            
            String temp = gen.newTemp();

            Symbol aux = environment.getVarActual(this.id);

            if (environment.name == "Global$" || aux == null)
            {
                gen.AddExp(temp, var_asignation.position.ToString());
            }
            else
            {
                gen.AddExp(temp, "SP + " + var_asignation.position.ToString());
            }
            gen.SetStack(temp,val.value.ToString());

            environment.saveVar(var_asignation.id, var_asignation.value, var_asignation.type, var_asignation.type_name, 0);
            return var_asignation;
        }
        public override object execute(Environment_ environment)
        {
            Return val = this.value != null ? this.value.execute(environment) : new Return(null, Type_.DEFAULT);
            Symbol b = environment.getVar(this.id);
            if (b == null)
            {
                throw new Error_(this.line, this.column, "Semantico", "No existe la variable:" + this.id);
            }
            if (b.type_name == "type"|| b.type_name == "function")
            {
                throw new Error_(this.line, this.column, "Semantico", "Se esperaba una variable y se obtuvo type: " + this.id);
            }

            //Asignment normal
            if (expList.Count == 0)
            {
                if (val.type != b.type)
                {
                    if (!(b.type == Type_.REAL && val.type == Type_.INTEGER))
                    {
                        throw new Error_(this.line, this.column, "Semantico", "Asignacion de tipo incorrecto:" + this.id);
                    }
                }
                return environment.saveVar(b.id, val.value, b.type, b.type_name);
            }
            else
            {
                //Asignacion objecto
                Environment_ auxEnvironment = environment;
                Return retorno = null;
                b = null;
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
                return auxEnvironment.saveVar(b.id, val.value, b.type, b.type_name);
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
