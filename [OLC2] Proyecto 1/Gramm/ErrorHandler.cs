using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace _OLC2__Proyecto_1.Gramm
{
    class ErrorHandler
    {
        private ParseTree tree;
        private ParseTreeNode root;
        public List<Error> errors;

        public ErrorHandler(ParseTree tree, ParseTreeNode root)
        {
            this.tree = tree;
            this.root = root;
        }

        public bool hasErrors()
        {
            if(tree.ParserMessages.Count>0 || root == null)
            {
                foreach(var error in tree.ParserMessages)
                {
                    Analyzer.output +="Error en fila " + error.Location.Line + ", columna " + error.Location.Column + ". " + error.Message + "\n";
                    errors.Add(new Error(error.Location.Line, error.Location.Column, "", "Se esperaba: " + error.ParserState.ExpectedTerminals, ""));
                }
                return true;
            }
            return false;
        }

    }
}
