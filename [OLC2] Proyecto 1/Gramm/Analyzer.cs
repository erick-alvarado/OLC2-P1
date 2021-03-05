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
using _OLC2__Proyecto_1.Instructions.Conditions;
using _OLC2__Proyecto_1.Instructions;
using _OLC2__Proyecto_1.Instructions.Loops;
using _OLC2__Proyecto_1.Instructions.Transfer;

namespace _OLC2__Proyecto_1.Gramm
{
    class Analyzer
    {
        public static String output;
        public List<Error_> errors;
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
            Environment_ environment = new Environment_(null,"Global");
            ErrorHandler errorHandler = new ErrorHandler(tree, root);

            if (!errorHandler.hasErrors())
            {
                LinkedList<Instruction> AST = start(root);
                
                foreach (Instruction ins in AST)
                {
                    try
                    {
                       var ret = ins.execute(environment);
                        if (ret != null)
                        {
                            Access a = (Access)ret;
                            throw new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.id);
                        }
                    }
                    catch (Exception e)
                    {
                        //this.errors.Add(e.Message);
                    }
                }
                
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
            Main main = new Main(instructionList(root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(1)));
            list.AddLast(main);
            return list;
        }
        //GLOBAL VARIABLES
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
        public Assignment assignmentST(ParseTreeNode root)
        {
            int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(1).Token.Location.Column;
            String id = root.ChildNodes.ElementAt(0).Token.Text;
            return new Assignment(line, column, id, expression(root.ChildNodes.ElementAt(2)));
        }
        //DECLARATION
        public LinkedList<Instruction> declarationList(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 2)
            {
                LinkedList<Instruction> list = declarationList(root.ChildNodes.ElementAt(0));
                list.AddLast(declara(root.ChildNodes.ElementAt(1)));
                return list;
            }
            else
            {
                LinkedList<Instruction> list = new LinkedList<Instruction>();
                list.AddLast(declara(root.ChildNodes.ElementAt(0)));
                return list;
            }
        }
        public Declara declara(ParseTreeNode root)
        {
            String tag = root.ChildNodes.ElementAt(0).Term.Name;
            if (root.ChildNodes.Count == 2)
            {
                switch (tag)
                {
                    case "var":
                        Declara list = new Declara(declarationListVar(root.ChildNodes.ElementAt(1)));
                        return list;
                    case "type":
                        Declara list2 = new Declara(declarationListType(root.ChildNodes.ElementAt(1)));
                        return list2;
                    case "const":
                        Declara list3 = new Declara(declaration(root.ChildNodes.ElementAt(1)));
                        return list3;
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
            if (root.ChildNodes.Count == 2)
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
        public DeclarationVar declarationVar(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 5)
            {
                LinkedList<Access> ids = idList(root.ChildNodes.ElementAt(0));
                return new DeclarationVar(ids, type(root.ChildNodes.ElementAt(2)), expression(root.ChildNodes.ElementAt(4)), root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);

            }
            else
            {
                Type_ t = type(root.ChildNodes.ElementAt(2));
                LinkedList<Access> ids = idList(root.ChildNodes.ElementAt(0));
                if ( t== Type_.ID)
                {
                    return new DeclarationVar(ids, t, root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Token.Text, root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
                }
                else
                {
                    return new DeclarationVar(ids,t, root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
                }

            }
        }
        public DeclarationType declarationType(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 4)
            {
                LinkedList<Access> ids = idList(root.ChildNodes.ElementAt(0));
                return new DeclarationType(ids, type(root.ChildNodes.ElementAt(2)), root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
            }
            else if (root.ChildNodes.Count == 6)
            {
                LinkedList<Access> ids = idList(root.ChildNodes.ElementAt(0));
                return new DeclarationType(ids, declarationList(root.ChildNodes.ElementAt(3)), root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
            }
            else
            {
                LinkedList<Access> ids = idList(root.ChildNodes.ElementAt(0));
                Type_ t1 = type(root.ChildNodes.ElementAt(4));
                Type_ t2 = type(root.ChildNodes.ElementAt(7));
                LinkedList<Expression> exp1=new LinkedList<Expression>();
                LinkedList<Expression> exp2 = new LinkedList<Expression>();
                if (t1 == Type_.SUBRANGE)
                {
                    exp1.AddLast(expression(root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(0)));
                    exp1.AddLast(expression(root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(3)));
                }
                if (t2 == Type_.SUBRANGE)
                {
                    exp2.AddLast(expression(root.ChildNodes.ElementAt(7).ChildNodes.ElementAt(0)));
                    exp2.AddLast(expression(root.ChildNodes.ElementAt(7).ChildNodes.ElementAt(3)));
                }
                return new DeclarationType(ids, t1,t2 ,exp1,exp2, root.ChildNodes.ElementAt(1).Token.Location.Line, root.ChildNodes.ElementAt(1).Token.Location.Column);
            }
        }

        public LinkedList<Instruction> declaration(ParseTreeNode root)
        {
            LinkedList<Instruction> l = new LinkedList<Instruction>();
            int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(1).Token.Location.Column;
            l.AddLast(new Declaration(root.ChildNodes.ElementAt(0).Token.Text, expression(root.ChildNodes.ElementAt(2)),line,column ));
            return l;
        }
        public LinkedList<Access> idList(ParseTreeNode root)
        {
            LinkedList<Access> l = new LinkedList<Access>();
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
            l = idList(root.ChildNodes.ElementAt(0));
            Console.WriteLine("nel");
            l.AddLast(access(root.ChildNodes.ElementAt(2)));
            
            return l;
        }
        
        //INSTRUCTIONS
        public LinkedList<Instruction> instructionList(ParseTreeNode root)
        {

            if (root.ChildNodes.Count == 2)
            {
                LinkedList<Instruction> list = instructionList(root.ChildNodes.ElementAt(0));
                list.AddLast(instruction(root.ChildNodes.ElementAt(1)));
                return list;
            }
            else
            {
                LinkedList<Instruction> list = new LinkedList<Instruction>();
                list.AddLast(instruction(root.ChildNodes.ElementAt(0)));
                return list;
            }

        }
        public Instruction instruction(ParseTreeNode root)
        {
            String tag = root.ChildNodes.ElementAt(0).Term.Name;
            switch (tag)
            {
                case "ifST":
                    Instruction list = ifST(root.ChildNodes.ElementAt(0));
                    return list;
                case "caseST":
                    Instruction list1 = caseST(root.ChildNodes.ElementAt(0));
                    return list1;
                case "printST":
                    Instruction print = printST(root.ChildNodes.ElementAt(0));
                    return print;
                case "whileST":
                    Instruction w = whileST(root.ChildNodes.ElementAt(0));
                    return w;
                case "repeatUntilST":
                    Instruction re = repeatUntilST(root.ChildNodes.ElementAt(0));
                    return re;
                case "assignmentST":
                    Instruction ass = assignmentST(root.ChildNodes.ElementAt(0));
                    return ass;
                case "break":
                    int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
                    int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
                    Break br = new Break(line,column,"BREAK");
                    return br;
                case "continue":
                    int line1 = root.ChildNodes.ElementAt(0).Token.Location.Line;
                    int column1 = root.ChildNodes.ElementAt(0).Token.Location.Column;
                    Break cc = new Break(line1, column1, "CONTINUE");
                    return cc;
                default:
                    return new Empty();
            }
        }
        public Instruction printST(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 2)
            {
                Literal fake = new Literal("", Type_.STRING, 0, 0);
                LinkedList<Expression> ee = new LinkedList<Expression>();
                ee.AddLast(fake);
                return new Print(ee, false);

            }
            String tag = root.ChildNodes.ElementAt(0).Term.Name.ToLower();
            LinkedList<Expression> e = expressionList(root.ChildNodes.ElementAt(2));
            if (tag == "write")
            {
                return new Print(e,false);
            }
            else
            {
                return new Print(e, true);
            }
        }
        public LinkedList<Expression> expressionList(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                LinkedList<Expression> list = expressionList(root.ChildNodes.ElementAt(0));
                list.AddLast(expression(root.ChildNodes.ElementAt(2)));
                return list;
            }
            else
            {
                LinkedList<Expression> list = new LinkedList<Expression>();
                list.AddLast(expression(root.ChildNodes.ElementAt(0)));
                return list;
            }
        }

        //CONDITIONS
        public Instruction ifST(ParseTreeNode root)
        {
            Expression e = expression(root.ChildNodes.ElementAt(1));
            Statement s = statements(root.ChildNodes.ElementAt(3));
            switch (root.ChildNodes.Count)
            {
                case 4:
                    return new If(e,s);
                case 6:
                    String tag = root.ChildNodes.ElementAt(5).Term.Name;
                    if (tag == "ifST")
                    {
                        If temp =(If) ifST(root.ChildNodes.ElementAt(5));
                        return new If(e, s, null,temp);
                    }
                    else
                    {
                        Statement s2 = statements(root.ChildNodes.ElementAt(5));
                        return new If(e, s, s2, null);
                    }
            }
                return null;
        }
        public Instruction caseST(ParseTreeNode root)
        {
            Expression exp = expression(root.ChildNodes.ElementAt(1));
            LinkedList<CaseList> caselist = caseList(root.ChildNodes.ElementAt(3));

            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Line;
            if (root.ChildNodes.Count == 6)
            {
                return new Case(line, column,exp, caselist);
            }
            else
            {
                Statement state = statements(root.ChildNodes.ElementAt(5));
                return new Case(line, column,exp, caselist, state);
            }
        }
        public LinkedList<CaseList> caseList(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 4)
            {
                LinkedList<CaseList> list = caseList(root.ChildNodes.ElementAt(0));
                list.AddLast(new CaseList(expressionList(root.ChildNodes.ElementAt(1)), statements(root.ChildNodes.ElementAt(3))));
                return list;
            }
            else
            {
                LinkedList<CaseList> list = new LinkedList<CaseList>();
                list.AddLast(new CaseList(expressionList(root.ChildNodes.ElementAt(0)), statements(root.ChildNodes.ElementAt(2))));
                return list;
            }
        }

        //LOOPS
        public Instruction whileST(ParseTreeNode root)
        {
            Expression e = expression(root.ChildNodes.ElementAt(1));
            Statement st = statements(root.ChildNodes.ElementAt(3));
            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
            return new While(line,column,e,st);
        }
        public Instruction repeatUntilST(ParseTreeNode root)
        {
            Expression e = expression(root.ChildNodes.ElementAt(3));
            Statement st = statements(root.ChildNodes.ElementAt(1));
            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
            return new RepeatUntil(line, column, e, st);
        }


        public Statement statements(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 1)
            {
                return new Statement(instructionList(root.ChildNodes.ElementAt(0)),0, 0);

            }
            else
            {
                return new Statement(instructionList(root.ChildNodes.ElementAt(1)), root.ChildNodes.ElementAt(0).Token.Location.Line, root.ChildNodes.ElementAt(0).Token.Location.Column);

            }
        }
        public Type_ type(ParseTreeNode root)
        {
            String tag = "";
            if (root.ChildNodes.Count == 0)
            {
                tag = root.Token.Terminal.Name.ToLower();
            }
            else if (root.ChildNodes.Count == 4)
            {
                return Type_.SUBRANGE;
            }
            else
            {
                tag = root.ChildNodes.ElementAt(0).Token.Terminal.Name.ToLower();
                //tag = root.ChildNodes.ElementAt(0).Token.ValueString.ToLower();
            }
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
                    return Type_.ID;
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
                int line=0;
                int column = 0;
                if (type != "access")
                {
                    line = root.ChildNodes.ElementAt(0).Token.Location.Line;
                    column = root.ChildNodes.ElementAt(0).Token.Location.Column;
                }
                //TODO agregar llamada a funciones
                switch (type)
                {
                    case "access":
                        return access(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0));
                    case "INTEGER":
                        try
                        {
                            return new Literal(int.Parse(value), Type_.INTEGER, line, column);
                        }
                        catch (Exception e)
                        {
                            return new Literal(Double.Parse(value), Type_.REAL, line, column);
                        }
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
