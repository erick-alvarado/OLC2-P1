using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Symbol_;
namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class DeclarationVar : Instruction
    {
        private LinkedList<Access> idList= new LinkedList<Access>();
        private Type_ type= Type_.DEFAULT;
        private String id;
        private Expression value;

        public DeclarationVar(int line, int column, LinkedList<Access> idList, Type_ type,String id="", Expression value = null)
        {
            this.idList = idList;
            this.type = type;
            this.id = id;
            this.value = value;

            setLineColumn(line, column);
        }
        public override object execute(Environment_ environment)
        {
            Return val = this.value != null ? this.value.execute(environment) : new Return(null, type);
            if (val.value != null)
            {
                if (idList.Count > 1)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Declaracion con asignacion a multiples variables ");
                }
                if (this.type != val.type&& this.type!=Type_.ID)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Declaracion con asignacion de tipo incorrecto");
                }
            }
            if (type == Type_.ID)
            {
                Symbol temp = environment.getVar(this.id);
                if (temp == null || temp.type_name=="var")
                {
                    throw new Error_(this.line, this.column, "Semantico", "El tipo no existe: " + this.id);
                }
                if (temp.type != val.type && val.value!=null)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Declaracion con asignacion de tipo incorrecto");

                }
                //TODO Verificar todo esto
                this.type = temp.type;
                if (val.value == null) { 
                    val.value = temp.value;
                }
                else
                {
                    try
                    {
                        Environment_ env = (Environment_)val.value;
                        if (env.name != this.id)
                        {
                            throw new Error_(this.line, this.column, "Semantico", "Asignacion de distintos tipos de objects ");
                        }
                    }
                    catch(Exception e)
                    {
                        //TODO Parsear para los arrays y funciones
                        Console.WriteLine(e);
                    }
                }
            }
            foreach(Access e in idList)
            {
                if (environment.getVar(e.getId()) != null)
                {
                    throw new Error_(this.line, this.column, "Semantico", "Declaracion de una variable ya existente:"+e.getId());
                }
                else
                {
                    if (val.value == null)
                    {
                        switch (type)
                        {
                            case Type_.BOOLEAN:
                                val.value = false;
                                break;
                            case Type_.STRING:
                                val.value = "";
                                break;
                            case Type_.INTEGER:
                                val.value = 0;
                                break;
                            case Type_.REAL:
                                val.value = 0.000000000000000000000000;
                                break;
                        }
                    }
                    environment.saveVar(e.getId(), val.value, type,"var");
                }
            }
            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
}
