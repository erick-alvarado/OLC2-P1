using _OLC2__Proyecto_1.Instructions.Functions;
using _OLC2__Proyecto_1.Symbol_;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Reports
{
    class Dot
    {
        private static int counter;
        private static String graph;

        public static String getDot(ParseTreeNode root)
        {
            graph = "digraph G{";
            graph += "nodo0[label=\"" + scape(root.ToString()) + "\"];\n";
            counter = 1;
            AST("nodo0", root);
            graph += "}";
            return graph;
        }
        public static String getDotVar(Environment_ e)
        {
            graph = "digraph G{"
                + "graph [ratio=fill];\n"
                + "node [label=\"\\N\", fontsize=15, shape=plaintext];\n"
                + "graph [bb=\"0,0,352,154\"];\n"
                + "arset [label=<\n"
                + "<TABLE ALIGN = \"LEFT\" > \n"
                +"<TR>\n" 
                +"<TD>Name </TD>\n"
                + "<TD>Type </TD>\n"
                + "<TD>Type_Name </TD>\n"
                + "<TD>Environment </TD>\n"
                + "<TD>Value </TD>\n"
                + " </TR\n>"
                ;
            Var(e);
            counter = 1;
            graph += "</TABLE>\n"
                    + ">, ]; "+ "}";
            return graph;
        }
        private static void Var(Environment_ e)
        {

            foreach (Symbol b in e.variables)
            {
                graph += "<TR>\n"
                + "<TD>"+b.id+" </TD>\n"
                + "<TD>" + b.type + "</TD>\n"
                + "<TD>" + b.type_name + " </TD>\n"
                + "<TD>" + e.name + "</TD>\n"
                + "<TD>" + b.value + " </TD>\n"
                + " </TR\n>";

                if (b.type_name == "function")
                {
                    Function temp2 = (Function)b.value;
                    Var(temp2.environmentAux);
                }
                if (b.type_name == "object")
                {
                    Environment_ temp2 = (Environment_)b.value;
                    Var(temp2);
                }

            }
        }
        private static void AST(String father, ParseTreeNode childs)
        {
            foreach(ParseTreeNode child in childs.ChildNodes)
            {
                String name = "nodo" + counter.ToString();
                graph += name + "[label=\"" + scape(child.ToString()) + "\"];\n";
                graph += father + "->" + name + ";\n";
                counter++;
                AST(name, child);
            }
        }
        private static String scape(String txt)
        {
            txt = txt.Replace("\\", "\\\\");
            txt = txt.Replace("\"", "\\\"");
            return txt;
        }

    }
}
