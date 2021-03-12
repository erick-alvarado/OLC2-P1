using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Symbol_;
using Array = _OLC2__Proyecto_1.Symbol_.Array;

namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class DeclarationType : Instruction
    {
        private LinkedList<Access> idList = new LinkedList<Access>();
        private Type_ type= Type_.DEFAULT;
        private Type_ type2= Type_.DEFAULT;
        private String idName;
        private String idName2;
        private LinkedList<Expression> expression= new LinkedList<Expression>();
        private LinkedList<Expression> expression2= new LinkedList<Expression>();
        private LinkedList<Instruction> declarationList = new LinkedList<Instruction>();

        public DeclarationType(int line, int column, LinkedList<Access> idList, Type_ type, Type_ type2, string idName, string idName2, LinkedList<Expression> expression, LinkedList<Expression> expression2, LinkedList<Instruction> declarationList)
        {
            this.idList = idList;
            this.type = type;
            this.type2 = type2;
            this.idName = idName;
            this.idName2 = idName2;
            this.expression = expression;
            this.expression2 = expression2;
            this.declarationList = declarationList;
            setLineColumn(line, column);
        }

        public override object execute(Environment_ environment)
        {
            if (this.declarationList!=null)
            {
                //Declaracion Object
                String id = this.idList.First().id;
                if (environment.getVarActual(id) != null)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Declaracion de una variable ya existente:"+id);
                }

                Environment_ temp = new Environment_(null, id);
                foreach (Instruction i in this.declarationList)
                {
                    object check = i.execute(temp);
                    if (check != null)
                    {
                        Break a = (Break)check;
                        throw new Error_(a.line, a.column, "Semantico", "Sentencia fuera de contexto:" + a.type);
                    }
                }
                environment.saveVarActual(id, temp, type,"object");

            }
            else if(this.type!=Type_.DEFAULT && this.type2 == Type_.DEFAULT && this.expression2 == null && this.expression == null)
            {
                //Declaracion type normal
                foreach (Access e in idList)
                {
                    Symbol b = environment.getVarActual(e.getId());
                    if ( b != null)
                    {
                        throw new Error_(this.line, this.column, "Semantico", "Declaracion de una variable ya existente");
                    }
                    else
                    {
                        if (this.type == Type_.ID)
                        {
                            Symbol temp = environment.getVar(this.idName);
                            if (temp == null|| temp.type_name == "function" || temp.type_name == "var" || temp.type_name == "cons")
                            {
                                throw new Error_(this.line, this.column, "Semantico", "No existe el type:"+this.idName);
                            }
                            environment.saveVarActual(e.getId(), temp.value, temp.type, "type");
                        }
                        else
                        {
                            environment.saveVarActual(e.getId(), null, type, "type");
                        }
                    }
                }
            }
            else
            {//Declaracion array
                String id = this.idList.First().id;
                if (environment.getVarActual(id) != null)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Declaracion de una variable ya existente:" + id);
                }
                Expression value, value2;
                Return start, end;
                if (this.expression != null)
                {
                    value = this.expression.First();
                    start = value != null ? value.execute(environment) : new Return(null, Type_.INTEGER);
                    value2 = this.expression.Last();
                    end = value2 != null ? value2.execute(environment) : new Return(null, Type_.INTEGER);
                    if (start.value != null && end.value != null)
                    {
                        int s, e;
                        try
                        {
                            s = (int)start.value;
                            e = (int)end.value;
                            switch (this.type2)
                            {
                                case Type_.BOOLEAN:
                                    Array l = new Array(s, e, this.idList.First().id, false, Type_.BOOLEAN);
                                    environment.saveVarActual(this.idList.First().id, l, Type_.BOOLEAN, "array");
                                    break;
                                case Type_.STRING:
                                    Array l1 = new Array(s, e, this.idList.First().id, "", Type_.STRING);
                                    environment.saveVarActual(this.idList.First().id, l1, Type_.STRING, "array");
                                    break;
                                case Type_.INTEGER:
                                    Array l2 = new Array(s, e, this.idList.First().id, 0, Type_.INTEGER);
                                    environment.saveVarActual(this.idList.First().id, l2, Type_.INTEGER, "array");
                                    break;
                                case Type_.REAL:
                                    Array l3 = new Array(s, e, this.idList.First().id, 0, Type_.REAL);
                                    environment.saveVarActual(this.idList.First().id, l3, Type_.REAL, "array");
                                    break;
                                case Type_.ID:
                                    Symbol b = environment.getVar(this.idName2);
                                    Array l4 = new Array(s, e, this.idList.First().id, b.value, Type_.ID);
                                    environment.saveVarActual(this.idList.First().id, l4, Type_.ID, "array");
                                    break;
                                default:
                                    throw new Error_(this.line, this.column, "Semantico", "No se puede hacer arrays de tipo:" + id);
                            }
                        }
                        catch (Exception se)
                        {
                            throw new Error_(this.line, this.column, "Semantico", "Inicio y fin de array no validos");
                        }

                    }
                    else
                    {
                        throw new Error_(this.line, this.column, "Semantico", "Inicio y fin de array no validos");
                    }
                }
                
                

                
            }
            
            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
