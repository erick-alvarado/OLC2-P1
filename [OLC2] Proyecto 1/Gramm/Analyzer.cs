using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using _OLC2__Proyecto_1.Instructions.Variables;

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
                LinkedList<Instruction> AST = start(root);
                /*Environment_ environment = new Environment_();
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
        public LinkedList<Instruction> start(ParseTreeNode root)
        {
            LinkedList<Instruction> list = globalVariables(root.ChildNodes.ElementAt(3));
            LinkedList<Instruction> mainL = main(root.ChildNodes.ElementAt(4));
            foreach (var mainIns in mainL)
            {
                list.AddLast(mainIns);
            }

            return list;
        }
        public LinkedList<Instruction> globalVariables(ParseTreeNode root)
        {
            String tag = root.ChildNodes.ElementAt(0).Term.Name;
            switch (tag)
            {
                case "declarationList":
                    LinkedList<Instruction> list = declarationList(root.ChildNodes.ElementAt(0));
                    return list;
                default:
                    return new LinkedList<Instruction>();
            }
        }
        public LinkedList<Instruction> declarationList(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 2)
            {
                LinkedList<Instruction> list = declarationList(root.ChildNodes.ElementAt(0));
                list.Concat(declara(root.ChildNodes.ElementAt(1)));
                return list;
            }
            else
            {
                LinkedList<Instruction> list = declara(root.ChildNodes.ElementAt(0));
                return list;
            }
        }
        public LinkedList<Instruction> declara(ParseTreeNode root)
        {
            String tag = root.ChildNodes.ElementAt(0).Term.Name;
            if (root.ChildNodes.Count == 2)
            {
                switch (tag)
                {
                    case "RVAR":
                        LinkedList<Instruction> list = declarationListVar(root.ChildNodes.ElementAt(1));
                        return list;
                    case "RTYPE":
                        LinkedList<Instruction> list = declarationListType(root.ChildNodes.ElementAt(1));
                        return list;
                    case "RCONST":
                        LinkedList<Instruction> list = declaration(root.ChildNodes.ElementAt(1));
                        return list;
                }
            }
            else
            {
                switch (tag)
                {
                    case "functionST":
                        LinkedList<Instruction> list = functionSt(root.ChildNodes.ElementAt(1));
                        return list;
                    case "procedureST":
                        LinkedList<Instruction> list = procedureSt(root.ChildNodes.ElementAt(1));
                        return list;
                }
            }
            return null;
        }
        public LinkedList<Instruction> declarationListVar(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                LinkedList<Instruction> list = declarationListVar(root.ChildNodes.ElementAt(0));
                list.AddLast(declarationVar(root.ChildNodes.ElementAt(1)));
                return list;
            }
            else
            {
                LinkedList<Instruction> list = new LinkedList<Instruction>();
                list.AddLast(declarationVar(root.ChildNodes.ElementAt(0)));
                return list;
            }
        }
        public LinkedList<Instruction> declarationListType(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                LinkedList<Instruction> list = declarationListType(root.ChildNodes.ElementAt(0));
                list.AddLast(declarationType(root.ChildNodes.ElementAt(1)));
                return list;
            }
            else
            {
                LinkedList<Instruction> list = new LinkedList<Instruction>();
                list.AddLast(declarationType(root.ChildNodes.ElementAt(0)));
                return list;
            }
        }
        public Declaration declarationVar(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 5)
            {
                ParseTreeNode id = root.ChildNodes.ElementAt(0);
                if(id.ChildNodes.Count== 1)
                {
                    return new Declaration(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.ValueString, type(root.ChildNodes.ElementAt(2)), expression(root.ChildNodes.ElementAt(4)),root.Token.Location.Line,root.Token.Location.Column);
                }
                else
                {
                    throw new Error_(id.Token.Location.Line, id.Token.Location.Column, "Semantico", "Declaracion con asignacion a multiples variables");
                }

            }
            else
            {
                Type_ t = type(root.ChildNodes.ElementAt(2));
                List<String> ids = idList(root.ChildNodes.ElementAt(0));
                return new Declaration(ids, type(root.ChildNodes.ElementAt(2)), root.Token.Location.Line, root.Token.Location.Column);

            }
        }
        public Declaration declarationType(ParseTreeNode root)
        {

        }
        public List<String> idList(ParseTreeNode root)
        {
            List<String> l = new List<String>();
            for(int i =0; i< root.ChildNodes.Count; i++)
            {
                l.Add(root.ChildNodes.ElementAt(i).Token.ValueString);
            }
            return l;
        }
        public Type_ type(ParseTreeNode root)
        {
            String tag = root.ChildNodes.ElementAt(0).Token.ValueString.ToLower();
            switch (tag)
            {
                case "integer":
                    return Type_.INTEGER;
                case "string":
                    return Type_.STRING;
                case "real":
                    return Type_.REAL;
                case "boolean":
                    return Type_.BOOLEAN;
                case "id":
                    return type(root.ChildNodes.ElementAt(0));
            }
            return Type_.NULL;
        }

    }
}
