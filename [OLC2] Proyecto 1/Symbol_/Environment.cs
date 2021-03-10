using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
namespace _OLC2__Proyecto_1.Symbol_
{
    public class Environment_
    {
        public LinkedList<Symbol> variables;
        public Environment_ prev;
        public String name;
        public Environment_(Environment_ prev = null, String name="")
        {
            this.prev = prev;
            this.variables = new LinkedList<Symbol>();
            this.name = name;
        }

        public Symbol saveVar(String id, object value, Type_ type, String type_name)
        {
            Environment_ env = this;
            while (env != null)
            {
                foreach (Symbol vari in env.variables)
                {
                    if (vari.id == id)
                    {
                        vari.value = value;
                        if (vari.id == this.name)
                        {
                            return getVar(id);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                env = env.prev;
            }
            this.variables.AddLast(new Symbol(value, id, type,type_name));
            
            return null;
        }
        public Symbol saveVarActual(String id, object value, Type_ type, String type_name)
        {
            Environment_ env = this;
            foreach (Symbol vari in env.variables)
            {
                if (vari.id == id)
                {
                    vari.value = value;
                    if (vari.id == this.name)
                    {
                        return getVar(id);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            this.variables.AddLast(new Symbol(value, id, type, type_name));
            return null;
        }

        public Symbol getVar(String id)
        {
            Environment_ env = this;
            while (env != null)
            {
                foreach (Symbol vari in env.variables)
                {
                    if (vari.id == id)
                    {
                        return vari;
                    }
                }
                env = env.prev;
            }
            return null;
        }
        public Symbol getFunc(String id)
        {
            Environment_ env = this;
            while (env != null)
            {
                foreach (Symbol vari in env.variables)
                {
                    if (vari.id == id && vari.type_name=="function")
                    {
                        return vari;
                    }
                }
                env = env.prev;
            }
            return null;
        }
    }
}
