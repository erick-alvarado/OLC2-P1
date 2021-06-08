using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using Force.DeepCloner;

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

        public Symbol saveVar(String id, object value, Type_ type, String type_name, int position=0)
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
                            return vari;
                        }
                        return null;
                    }
                }
                env = env.prev;
            }
            this.variables.AddLast(new Symbol(value, id, type,type_name,position));
            
            return null;
        }
        public Symbol saveVarActual(String id, object value, Type_ type, String type_name, int position=0)
        {
            Environment_ env = this;
            foreach (Symbol vari in env.variables)
            {
                if (vari.id.ToLower() == id.ToLower())
                {
                    vari.value = value;
                    if (this.name == vari.id)
                    {
                        return vari;
                    }
                    return null;
                }
            }
            this.variables.AddLast(new Symbol(value, id, type, type_name,position));
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
                        return vari;
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
                    return vari;
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
                        return vari;
                    }
                }
                env = env.prev;
            }
            return null;
        }

        public int getVarCount()
        {
            Environment_ env = this;
            int ret = 0;
            foreach (Symbol vari in env.variables)
            {
                if (vari.type_name != "function")
                {
                    ret++;
                }
            }
            return ret;

        }
    }
}
