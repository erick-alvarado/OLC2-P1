using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;



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
        public override Return execute(Environment_ environment)
        {
            if (expList == null)
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
                Environment_ auxEnv = environment;
                Return auxRet = null;
                //Asignacion objecto
                int final = 0;
                String ident = "";
                Symbol symp = null;
                foreach (Expression e in this.expList)
                {
                    auxRet = e.execute(auxEnv);
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

                        }
                        catch (Exception _)
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
                            catch (Exception)
                            {
                            }
                        }
                        break;
                    }
                }
                if (ident != "")
                {
                    symp = auxEnv.getVar(ident);
                    if (symp == null)
                    {
                        throw new Error_(this.line, this.column, "Semantico", "No se se encuentra el item:" + auxRet.value.ToString());
                    }
                    return new Return(symp.value, symp.type);
                }
                else if (symp != null)
                {
                    return new Return(symp.value, symp.type);
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
