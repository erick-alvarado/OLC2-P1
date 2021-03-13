using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Symbol_;
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
                Environment_ auxEnv = environment;
                Return auxRet = null;
                //Asignacion objecto
                int final = 0;
                String ident = "";
                Symbol symp = null;
                try
                {
                    Environment_ gg = (Environment_)b.value;
                    auxEnv = gg;
                }
                catch (Exception _)
                {
                    throw new Error_(this.line, this.column, "Semantico", "No se encuentra el object:" + this.id);
                }
                foreach (Expression e in this.expList)
                {
                    auxEnv.prev = environment;
                    try
                    {
                        auxRet = e.execute(auxEnv);
                        if (auxRet.type != Type_.ID)
                        {
                            Access tls = (Access)e;
                            Symbol lahostia = auxEnv.getVar(auxRet.value.ToString());
                            if (lahostia != null)
                            {
                                symp = lahostia;
                            }
                        }

                    }
                    catch (Exception)
                    {
                    }
                    if (auxRet == null)
                    {
                        Access po = (Access)e;
                        throw new Error_(this.line, this.column, "Semantico", "No se se encuentra el item:"+po.id);
                    }
                    final++;
                    if (auxRet.type == Type_.ID)
                    {
                        try
                        {
                            Environment_ gg = (Environment_)auxRet.value;
                            auxEnv = gg;
                        }
                        catch (Exception _)
                        {
                            throw new Error_(this.line, this.column, "Semantico", "No se que pedo:" + this.id);
                        }
                    }
                    else
                    {
                        try
                        {
                            Access temp = (Access)e;
                            ident = temp.id;
                            
                        }catch(Exception _)
                        {
                            symp = auxEnv.getVar(auxRet.value.ToString());
                            if (symp == null)
                            {
                                throw new Error_(this.line, this.column, "Semantico", "No se se encuentra el item:" + auxRet.value.ToString());
                            }
                            try
                            {
                                Environment_ gg = (Environment_)symp.value;
                                auxEnv = gg;
                            }
                            catch (Exception )
                            {
                            }
                        }
                        break;
                    }
                }
                if (symp != null)
                {
                    symp.value = val.value;
                    auxEnv.saveVar(symp.id, symp.value, symp.type, symp.type_name);
                    return null;
                }
                else if (ident != "")
                {
                    symp = auxEnv.getVar(ident);
                    if (symp == null)
                    {
                        throw new Error_(this.line, this.column, "Semantico", "No se se encuentra el item:" + auxRet.value.ToString());
                    }
                    if (val.type != symp.type)
                    {
                        if (!(symp.type == Type_.REAL && val.type == Type_.INTEGER))
                        {
                            throw new Error_(this.line, this.column, "Semantico", "Asignacion de tipo incorrecto:" + this.id);
                        }
                    }
                    return auxEnv.saveVar(symp.id, val.value, symp.type, symp.type_name);
                }
            }
            return null;
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
