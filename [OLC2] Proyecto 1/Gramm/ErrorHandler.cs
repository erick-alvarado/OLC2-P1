using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using _OLC2__Proyecto_1.Symbol_;
namespace _OLC2__Proyecto_1.Gramm
{
    class ErrorHandler
    {
        private ParseTree tree;
        private ParseTreeNode root;
        public List<Error_> errors = new List<Error_>();

        public ErrorHandler(ParseTree tree, ParseTreeNode root)
        {
            this.tree = tree;
            this.root = root;
            ;
        }

        public bool hasErrors()
        {
            if(tree.ParserMessages.Count>0 || root == null)
            {
                foreach(var error in tree.ParserMessages)
                {
                    Analyzer.output +="Error en fila " + error.Location.Line + ", columna " + error.Location.Column + ". " + error.Message + "\n";
                    String type = error.Message[0]=='I' ? "Lex":"Syntax";
                    String expected="";
                    if (error.ParserState.ReportedExpectedSet != null)
                    {
                        foreach (String i in error.ParserState.ReportedExpectedSet)
                        {
                            expected += i + " | ";
                        }
                    }
                    
                    errors.Add(new Error_(error.Location.Line, error.Location.Column,type, error.Message, ""));
                }
                return true;
            }
            return false;
        }

    }
}

