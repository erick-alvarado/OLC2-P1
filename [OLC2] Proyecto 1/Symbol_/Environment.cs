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
                    if (vari.id.ToLower() == id.ToLower())
                    {
                        vari.value = value;
                        if (this.name == vari.id)
                        {
                            Symbol uid = new Symbol(vari);
                            return uid;
                        }
                        return null;
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
                if (vari.id.ToLower() == id.ToLower())
                {
                    vari.value = value;
                    if (this.name == vari.id)
                    {
                        Symbol uid = new Symbol(vari);
                        return uid;
                    }
                    return null;
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
                    if (vari.id.ToLower() == id.ToLower() && vari.type_name!="function")
                    {
                        Symbol uid = new Symbol(vari);
                        return uid;
                    }
                }
                env = env.prev;
            }
            return null;
        }
        public Symbol getVarActual(String id)
        {
            Environment_ env = this;
            foreach (Symbol vari in env.variables)
            {
                if (vari.id.ToLower() == id.ToLower())
                {
                    Symbol uid = new Symbol(vari);
                    return uid;
                }
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
                    if (vari.id.ToLower() == id.ToLower() && vari.type_name=="function")
                    {
                        Symbol uid = new Symbol(vari);
                        return uid;
                    }
                }
                env = env.prev;
            }
            return null;
        }
    }
}
