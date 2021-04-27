using _OLC2__Proyecto_1.Symbol_;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace _OLC2__Proyecto_1.Gramm
{
    class Analyzer_
    {
        public static String output;
        public static List<Optimization> opList = new List<Optimization>();
        public ParseTreeNode root;
        public Analyzer_()
        {
            output = "";
        }
        public String analyze(String input)
        {
            opList.Clear();

            Gramm_ grammar = new Gramm_();
            LanguageData language = new LanguageData(grammar);
            Parser parser = new Parser(language);
            ParseTree tree = parser.Parse(input);

            root = tree.Root;
            ErrorHandler errorHandler = new ErrorHandler(tree, root);

            if (!errorHandler.hasErrors())
            {
                Analyzer.output = AST("", root);

            }
            return Analyzer.output;
        }
        private String AST(String optimization, ParseTreeNode childs)
        {
            foreach (ParseTreeNode child in childs.ChildNodes)
            {
                if (child.ChildNodes.Count == 0 && child.Token!= null)
                {
                    optimization += child.Token.Text;
                    if(child.Token.Text=="{"|| child.Token.Text == ";"||child.Token.Text == "}")
                    {
                        optimization += "\r\n";
                    }
                }
                optimization = AST(optimization,child);
            }
            return optimization;
        }
    }
}
