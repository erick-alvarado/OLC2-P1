using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol;


namespace _OLC2__Proyecto_1.Gramm
{
    class Analyzer
    {
        public static String output;
        public List<Error> errors;
        public Analyzer()
        {
            output = "";
        }
        public String analyze(String input)
        {
            Gramm grammar = new Gramm();
            LanguageData language = new LanguageData(grammar);
            Parser parser = new Parser(language);

            ParseTree tree = parser.Parse(input);
            ParseTreeNode root = tree.Root;

            ErrorHandler errorHandler = new ErrorHandler(tree, root);
            if (!errorHandler.hasErrors())
            {
                /*LinkedList<Instruction> AST = start(root);
                Environment_ environment = new Environment_();
                foreach (Instruction ins in AST)
                {
                    try
                    {
                        var ret = ins.execute(environment);
                        if (ret != null)
                        {
                            // Error semantico.
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }*/
            }
            else
            {
                this.errors = errorHandler.errors;
            }
            return output;
        }
        
    }
}
