using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Instructions.Transfer;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Functions
{
    class Function : Instruction
    {
        private String id;
        public LinkedList<Expression> parameterList = new LinkedList<Expression>();//Lo que trae de la llamada
        public LinkedList<Instruction> argumentList = new LinkedList<Instruction>();//Lo que tiene originalmente
        private LinkedList<Instruction> declarationList = new LinkedList<Instruction>();
        private Statement statements;
        public Type_ return_;
        private bool exec = true;
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

                object val = null;
                switch (this.return_)
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
                this.environmentAux = new Environment_(null, this.id);
                this.environmentAux.saveVar(this.id, val, this.return_, "var");
                this.environmentAux.prev= environment;
            }
            else
            {
                
                foreach (Instruction i in this.declarationList)
                {
                    i.execute(this.environmentAux);
                }
                object obj = this.statements.execute(this.environmentAux);
                if(this.return_!=Type_.VOID && obj == null)
                {
                    throw new Error_(this.line, this.column, "Semantico", "La funcion debe retornar un valor de tipo:"+ Enum.GetName(typeof(Type_), this.return_));
                }
                if (obj != null && this.return_!=Type_.VOID)
                {
                    Break a = null;
                    try
                    {
                        a = (Break)obj;
                    }
                    catch(Exception e)
                    {
                        Symbol b = (Symbol)obj;
                        Literal n = new Literal(b.value, b.type, 0, 0);
                        a = new Break(0,0,"F",n);
                    }
                    if (a.type.Equals("BREAK")|| a.type.Equals("CONTINUE"))
                    {
                        throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.type);
                    }
                    else
                    {
                        if (this.return_.Equals(Type_.VOID))
                        {
                            if (a.e != null)
                            {
                                throw new Error_(a.line, a.column, "Semantico", "Los procedimientos no retornan valores" + a.type);
                            }
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
                }
                else
                {
                    throw new Error_(this.line, this.column, "Semantico", "El procedimiento no debe de retornar ningun valor" + Enum.GetName(typeof(Type_), this.return_));
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
