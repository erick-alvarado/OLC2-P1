using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using _OLC2__Proyecto_1.Instructions.Variables;
using _OLC2__Proyecto_1.Expressions;

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
                Console.WriteLine("Saber que pedo");
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
            /*LinkedList<Instruction> mainL = main(root.ChildNodes.ElementAt(4));
            foreach (var mainIns in mainL)
            {
                list.AddLast(mainIns);
            }
            */
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
                    case "var":
                        LinkedList<Instruction> list = declarationListVar(root.ChildNodes.ElementAt(1));
                        return list;
                    case "type":
                        LinkedList<Instruction> list2 = declarationListType(root.ChildNodes.ElementAt(1));
                        return list2;
                    /*case "RCONST":
                        LinkedList<Instruction> list = declaration(root.ChildNodes.ElementAt(1));
                        return list;*/
                }
            }
            else
            {
                /*
                switch (tag)
                {
                    case "functionST":
                        //LinkedList<Instruction> list = functionSt(root.ChildNodes.ElementAt(1));
                        return list;
                    case "procedureST":
                        //LinkedList<Instruction> list = procedureSt(root.ChildNodes.ElementAt(1));
                        return list;
                }*/
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
                    LinkedList<Expression> ids = idList(root.ChildNodes.ElementAt(0));
                    return new Declaration(ids, type(root.ChildNodes.ElementAt(2)), expression(root.ChildNodes.ElementAt(4)),root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
                }
                else
                {
                    throw new Error_(id.Token.Location.Line, id.Token.Location.Column, "Semantico", "Declaracion con asignacion a multiples variables");
                }

            }
            else
            {
                Type_ t = type(root.ChildNodes.ElementAt(2));
                LinkedList<Expression> ids = idList(root.ChildNodes.ElementAt(0));
                return new Declaration(ids, type(root.ChildNodes.ElementAt(2)), root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);

            }
        }
        public DeclarationType declarationType(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 4)
            {
                LinkedList<Expression> ids = idList(root.ChildNodes.ElementAt(0));
                return new DeclarationType(ids, type(root.ChildNodes.ElementAt(2)), root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
            }
            else if (root.ChildNodes.Count == 6)
            {
                LinkedList<Expression> ids = idList(root.ChildNodes.ElementAt(0));
                return new DeclarationType(ids, declarationList(root.ChildNodes.ElementAt(3)), root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
            }
            else
            {
                LinkedList<Expression> ids = idList(root.ChildNodes.ElementAt(0));
                return new DeclarationType(ids, type(root.ChildNodes.ElementAt(4)), type(root.ChildNodes.ElementAt(7)), root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
            }
        }
        public LinkedList<Expression> idList(ParseTreeNode root)
        {
            LinkedList<Expression> l = new LinkedList<Expression>();
            if(root.ChildNodes.Count == 0)
            {
                l.AddLast(access(root));
                return l;
            }
            if (root.ChildNodes.Count == 1)
            {
                l.AddLast(access(root.ChildNodes.ElementAt(0)));
                return l;
            }
            l.Concat(idList(root.ChildNodes.ElementAt(0)));
            l.AddLast(access(root.ChildNodes.ElementAt(2)));
            
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
                    //TODO arreglar para que busque el type
                    return type(root.ChildNodes.ElementAt(0));
            }
            return Type_.NULL;
        }
        public Expression expression(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                String op = root.ChildNodes.ElementAt(1).ToString().Split(' ')[0].ToLower();
                int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(1).Token.Location.Column;

                switch (op)
                {
                    case "+":
                        return new Arithmetic(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), ArithmeticOption.PLUS, line,column);
                    case "-":
                        return new Arithmetic(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), ArithmeticOption.MINUS, line,column);
                    case "*":
                        return new Arithmetic(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), ArithmeticOption.TIMES, line,column);
                    case "/":
                        return new Arithmetic(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), ArithmeticOption.DIV, line,column);
                    case "%":
                        return new Arithmetic(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), ArithmeticOption.MODULE, line,column);
                    case "<":
                        return new Relational(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), RelationalOption.LESS, line,column);
                    case ">":
                        return new Relational(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), RelationalOption.GREATER, line,column);
                    case "<=":
                        return new Relational(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), RelationalOption.LESSEQ, line,column);
                    case ">=":
                        return new Relational(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), RelationalOption.GREAEQ, line,column);
                    case "=":
                        return new Relational(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), RelationalOption.EQUALSEQUALS, line,column);
                    case "<>":
                        return new Relational(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), RelationalOption.DISTINT, line,column);
                    case "or":
                        return new Logical(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), LogicalOption.OR, line,column);
                    case "and":
                        return new Logical(expression(root.ChildNodes.ElementAt(0)), expression(root.ChildNodes.ElementAt(2)), LogicalOption.AND, line,column);
                }
            }
            else if (root.ChildNodes.Count == 2)
            {
                String op = root.ChildNodes.ElementAt(0).ToString().Split(' ')[0].ToLower();
                int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
                switch (op)
                {
                    case "-":
                        return new Arithmetic(expression(root.ChildNodes.ElementAt(1)), ArithmeticOption.MINUS, line,column);
                    case "not":
                        return new Logical(expression(root.ChildNodes.ElementAt(1)), LogicalOption.NOT, line,column);
                }
            }
            else
            {
                return finalExp(root.ChildNodes.ElementAt(0));
            }
            return null;
        }
        public Expression finalExp(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                return expression(root.ChildNodes.ElementAt(1));
            }
            else
            {
                String type = root.ChildNodes.ElementAt(0).Term.ToString();
                String value = root.ChildNodes.ElementAt(0).Token != null ? root.ChildNodes.ElementAt(0).Token.ValueString : root.ChildNodes.ElementAt(0).Term.Name;
                int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
                //TODO agregar llamada a funciones
                switch (type)
                {
                    case "access":
                        return access(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0));
                    case "INTEGER":
                        return new Literal(int.Parse(value), Type_.INTEGER, line,column);
                    case "REAL":
                        return new Literal(Double.Parse(value), Type_.REAL, line,column);
                    case "STR":
                        return new Literal(value, Type_.STRING, line,column);
                    case "TRUE":
                        return new Literal(true, Type_.BOOLEAN, line,column);
                    case "FALSE":
                        return new Literal(false, Type_.BOOLEAN, line,column);
                    case "NULL":
                        return new Literal(null, Type_.NULL, line,column);
                }
                return null;
            }
        }
        public Access access(ParseTreeNode root)
        {
            int line = root.Token.Location.Line;
            int column = root.Token.Location.Column;
            return new Access(root.Token.ValueString, line,column);
        }
    }
}
