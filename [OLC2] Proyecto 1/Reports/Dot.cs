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
