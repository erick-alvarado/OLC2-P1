using _OLC2__Proyecto_1.Abstract;
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
        public override object compile(Environment_ environment)
        {
            Generator gen = Generator.getInstance();
            Symbol b = environment.getVar(this.id);
            gen.AddCom("Asignation");
            Return ret = value.compile(environment);//ret.type = STACK|HEAP   ret.value = temp_final | pos_heap
            gen.AddExp(b.value.ToString(), ret.value.ToString());

            String pos = b.value.ToString().Split('T')[1];
            String temp = gen.newTemp();
            gen.AddExp(temp, pos);
            
            gen.SetStack(temp,ret.value.ToString());
            return null;
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
