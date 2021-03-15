using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _OLC2__Proyecto_1.Instructions.Functions
{
    //[Serializable()]
    class Function : Instruction 
    {
        public String id;
        public LinkedList<Expression> parameterList = new LinkedList<Expression>();//Lo que trae de la llamada
        public LinkedList<Instruction> argumentList = new LinkedList<Instruction>();//Lo que tiene originalmente
        public LinkedList<Instruction> declarationList = new LinkedList<Instruction>();
        public Statement statements;
        public Type_ return_;
        public bool exec = true;
        public Environment_ environmentAux;

        public Function(int line, int column,string id, LinkedList<Instruction> argumentList, LinkedList<Instruction> declarationList, Statement statements, Type_ return_)
        {
            this.id = id;
            this.argumentList = argumentList;
            this.declarationList = declarationList;
            this.statements = statements;
            this.return_ = return_;
            setLineColumn(line, column);
        }
        public Function(Function other)
        {
            this.id = other.id;
            this.parameterList = other.parameterList;
            this.argumentList = other.argumentList;
            this.declarationList = other.declarationList;
            this.statements = other.statements;
            this.return_ = other.return_;
            this.exec = other.exec;
            this.environmentAux = other.environmentAux;
            this.environmentAux.prev = other.environmentAux.prev;
        }

        public override object execute(Environment_ environment)
        {
            if (this.exec)
            {
                this.exec = false;
                if (environment.getVar(this.id) != null)
                {
                    throw new Error_(this.line, this.column, "Semantico", "La variable ya existe:" + this.id);
                }
                environment.saveVar(this.id, this, this.return_, "function");
                environmentAux = new Environment_(null, this.id);
                environmentAux.prev = environment;
            }
            else
            {
                foreach (Instruction i in this.declarationList)
                {
                    i.execute(this.environmentAux);
                }
                object obj = this.statements.execute(this.environmentAux);
                if (this.return_!=Type_.VOID)
                {
                    Break a = null;
                    try
                    {
                        a = (Break)obj;
                    }
                    catch(Exception e)
                    {
                        Symbol au = (Symbol)obj;
                        a = new Break(0, 0, "F", new Literal(au.value, au.type, 0, 0));
                    }
                    if (a== null)
                    {
                        throw new Error_(this.line, this.column, "Semantico", "La funcion debe retornar un type");
                    }
                    if (a.type.Equals("BREAK")|| a.type.Equals("CONTINUE"))
                    {
                        throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.type);
                    }
                    else
                    {
                        try
                        {
                            Return r = a.e.execute(this.environmentAux);
                            if (r.type != this.return_)
                            {
                                throw new Error_(a.line, a.column, "Semantico", "Tipo de retorno incorrecto" + a.type);
                            }
                            return r;
                        }
                        catch (Exception e)
                        {
                            throw new Error_(a.line, a.column, "Semantico", "Tipo de retorno incorrecto" + a.type);
                        }

                    }
                }
                else
                {
                    if (obj != null)
                    {
                        Break a = null;
                        try
                        {
                            a = (Break)obj;
                            if (a.type.Equals("BREAK") || a.type.Equals("CONTINUE")|| a.e!=null)
                            {
                                throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.type);
                            }
                            else
                            {
                                return a;
                            }
                        }
                        catch (Exception)
                        {
                            throw new Error_(this.line, this.column, "Semantico", "El procedimiento no debe de retornar ningun valor" + Enum.GetName(typeof(Type_), this.return_));
                        }

                    }
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
