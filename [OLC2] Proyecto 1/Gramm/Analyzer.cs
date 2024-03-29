﻿using System;
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
using _OLC2__Proyecto_1.Instructions.Functions;
using Compilador.Generator;

namespace _OLC2__Proyecto_1.Gramm
{
    class Analyzer
    {
        public static String output;
        public static List<Error_> errors= new List<Error_>();
        public ParseTreeNode root;
        public Environment_ environment= new Environment_(null,"nel shavo");
        public Analyzer()
        {
            output = "";
        }
        public String analyze(String input)
        {
            errors =new List<Error_>();
            Gramm grammar = new Gramm();
            LanguageData language = new LanguageData(grammar);
            Parser parser = new Parser(language);
            ParseTree tree = parser.Parse(input);
            root = tree.Root;
            environment = new Environment_(null,"Global$");
            ErrorHandler errorHandler = new ErrorHandler(tree, root);

            if (!errorHandler.hasErrors())
            {
                LinkedList<Instruction> AST = start(root);
                
                foreach (Instruction ins in AST)
                {
                    object ret = null;
                    try
                    {
                        ret = ins.execute(environment);
                        if (ret != null)
                        {
                            Break a = (Break)ret;
                            Analyzer.errors.Add( new Error_(a.line, a.column, "Semantico", "Sentencia de transferencia fuera de contexto:" + a.type));
                        }
                    }
                    catch (Exception e)
                    {
                        Analyzer.errors.Add((Error_)e);
                    }
                }
                if (Analyzer.errors.Count == 0)
                {
                    Generator generator = Generator.getInstance();
                    generator.clearCode();
                    environment = new Environment_(null, "Global$");
                    foreach (Instruction ins in AST)
                    {
                        ins.compile(environment);
                    }
                    return generator.getCode();
                }

            }
            return output;
        }
        
        private LinkedList<Instruction> start(ParseTreeNode root)
        {
            LinkedList<Instruction> list = globalVariables(root.ChildNodes.ElementAt(3));
            Main main = new Main(instructionList(root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(1)));
            list.AddLast(main);
            return list;
        }
        //GLOBAL VARIABLES
        private LinkedList<Instruction> globalVariables(ParseTreeNode root)
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
        private Assignment assignmentST(ParseTreeNode root)
        {
            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
            String id = root.ChildNodes.ElementAt(0).Token.Text;
            LinkedList<Expression> exp = expList(root.ChildNodes.ElementAt(1));
            return new Assignment(line,column,id,exp,expression(root.ChildNodes.ElementAt(3)));
        }

        private LinkedList<Expression> expList(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 4)
            {
                LinkedList<Expression> list = expList(root.ChildNodes.ElementAt(0));
                list.AddLast(new Access(0, 0, "$"));
                Expression e = expression(root.ChildNodes.ElementAt(2));
                list.AddLast(e);
                return list;
            }
            else if (root.ChildNodes.Count == 3)
            {
                String tag = root.ChildNodes.ElementAt(0).Term.Name;
                if(tag != "[")
                {
                    LinkedList<Expression> list = expList(root.ChildNodes.ElementAt(0));
                    Access a = access(root.ChildNodes.ElementAt(2));
                    list.AddLast(a);
                    return list;
                }
                else
                {
                    LinkedList<Expression> list = new LinkedList<Expression>();
                    list.AddLast(new Access(0, 0, "$"));
                    Expression e = expression(root.ChildNodes.ElementAt(1));
                    list.AddLast(e);
                    return list;
                }
            }
            else if (root.ChildNodes.Count == 0)
            {
                LinkedList<Expression> list = new LinkedList<Expression>();
                return list;
            }
            else
            {
                LinkedList<Expression> list = new LinkedList<Expression>();
                Access e = access(root.ChildNodes.ElementAt(1));
                list.AddLast(e);
                return list;
            }
        }
        //DECLARATION
        private LinkedList<Instruction> declarationList(ParseTreeNode root)
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
        private Instruction declara(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 2)
            {
                String tag = root.ChildNodes.ElementAt(0).Term.Name;
                switch (tag)
                {
                    case "var":
                        Declara list = new Declara(declarationListVar(root.ChildNodes.ElementAt(1)));
                        return list;
                    case "type":
                        Declara list2 = new Declara(declarationListType(root.ChildNodes.ElementAt(1)));
                        return list2;
                    case "const":
                        Declara list3 = new Declara(declarationListCons(root.ChildNodes.ElementAt(1)));
                        return list3;
                }
            }
            if (root.ChildNodes.Count == 1)
            {
                String tag = root.ChildNodes.ElementAt(0).Term.Name;
                switch (tag)
                {
                    case "functionST":
                        Function func = functionST(root.ChildNodes.ElementAt(0));
                        return func;
                    case "procedureST":
                        Function proc = procedureST(root.ChildNodes.ElementAt(0));
                        return proc;

                }
            }
            return new Empty();
        }
        private LinkedList<Instruction> declarationListVar(ParseTreeNode root)
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
        private LinkedList<Instruction> declarationListType(ParseTreeNode root)
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
        private LinkedList<Instruction> declarationListCons(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 2)
            {
                LinkedList<Instruction> list = declarationListCons(root.ChildNodes.ElementAt(0));
                list.AddLast(declaration(root.ChildNodes.ElementAt(1)));
                return list;
            }
            else
            {
                LinkedList<Instruction> list = new LinkedList<Instruction>();
                list.AddLast(declaration(root.ChildNodes.ElementAt(0)));
                return list;
            }
        }
        private DeclarationVar declarationVar(ParseTreeNode root)
        {
            int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(1).Token.Location.Line;
            if (root.ChildNodes.Count == 5)
            {
                LinkedList<Access> ids = idList(root.ChildNodes.ElementAt(0));
                Type_ t = type(root.ChildNodes.ElementAt(2));
                Expression exp = expression(root.ChildNodes.ElementAt(4));
                if(t == Type_.ID)
                {
                    String id = root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Token.Text;
                    return new DeclarationVar(line, column, ids, t, id, exp);
                }
                else
                {
                    return new DeclarationVar(line, column, ids, t,"" ,exp);
                }
            }
            else
            {
                Type_ t = type(root.ChildNodes.ElementAt(2));
                LinkedList<Access> ids = idList(root.ChildNodes.ElementAt(0));
                if ( t== Type_.ID)
                {
                    String id = root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Token.Text;
                    return new DeclarationVar(line, column, ids, t, id);
                }
                else
                {
                    return new DeclarationVar(line, column, ids, t);
                }

            }
        }
        private DeclarationType declarationType(ParseTreeNode root)
        {
            LinkedList<Access> ids = idList(root.ChildNodes.ElementAt(0));
            int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(1).Token.Location.Column;
            //Type normal
            if (root.ChildNodes.Count == 4)
            {
                Type_ t = type(root.ChildNodes.ElementAt(2));
                if (t == Type_.ID)
                {
                    String id = root.ChildNodes.ElementAt(2).Token.Text;
                    return new DeclarationType(line, column, ids, t, Type_.DEFAULT, id, "", null, null, null);
                }
                else
                {
                    return new DeclarationType(line, column, ids, t, Type_.DEFAULT, "", "", null, null, null);
                }
            }//Type object
            else if (root.ChildNodes.Count == 6)
            {
                LinkedList<Instruction> dl = declarationList(root.ChildNodes.ElementAt(3));
                return new DeclarationType(line,column,ids,Type_.ID,Type_.DEFAULT,"","",null,null,dl);
            }
            else
            {
                Type_ t1 = type(root.ChildNodes.ElementAt(4));
                Type_ t2 = type(root.ChildNodes.ElementAt(7));
                LinkedList<Expression> exp1=new LinkedList<Expression>();
                LinkedList<Expression> exp2 = new LinkedList<Expression>();
                String id1="";
                String id2="";
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
                if (t1 == Type_.ID)
                {
                    id1 = root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(0).Token.Text;
                }
                if (t2 == Type_.ID)
                {
                    id2 = root.ChildNodes.ElementAt(7).ChildNodes.ElementAt(0).Token.Text;
                }
                return new DeclarationType(line,column,ids,t1,t2,id1,id2,exp1,exp2,null);
            }
        }

        private Declaration declaration(ParseTreeNode root)
        {
            int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(1).Token.Location.Column;
            String id = root.ChildNodes.ElementAt(0).Token.Text;
            Type_ typ = type(root.ChildNodes.ElementAt(2));
            Expression e = expression(root.ChildNodes.ElementAt(4));

            return new Declaration(id,e ,typ,line,column );
        }
        private LinkedList<Access> idList(ParseTreeNode root)
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
            l.AddLast(access(root.ChildNodes.ElementAt(2)));
            
            return l;
        }
        
        //INSTRUCTIONS
        private LinkedList<Instruction> instructionList(ParseTreeNode root)
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
        private Instruction instruction(ParseTreeNode root)
        {
            String tag = "";
            if (root.ChildNodes.Count != 0)
            {
                tag = root.ChildNodes.ElementAt(0).Term.Name;
            }
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
                case "forDoST":
                    Instruction fo = forDoST(root.ChildNodes.ElementAt(0));
                    return fo;
                case "repeatUntilST":
                    Instruction re = repeatUntilST(root.ChildNodes.ElementAt(0));
                    return re;
                case "assignmentST":
                    Instruction ass = assignmentST(root.ChildNodes.ElementAt(0));
                    return ass;
                case "callFuncST":
                    callFunction2 callf = callFuncST2(root.ChildNodes.ElementAt(0));
                    return callf;
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
                case "exit":
                    if (root.ChildNodes.Count == 5)
                    {
                        int line2 = root.ChildNodes.ElementAt(0).Token.Location.Line;
                        int column2 = root.ChildNodes.ElementAt(0).Token.Location.Column;
                        Expression e = expression(root.ChildNodes.ElementAt(2));
                        Break cc2 = new Break(line2, column2, "EXIT", e);
                        return cc2;

                    }
                    else
                    {
                        int line2 = root.ChildNodes.ElementAt(0).Token.Location.Line;
                        int column2 = root.ChildNodes.ElementAt(0).Token.Location.Column;
                        Break cc2 = new Break(line2, column2, "EXIT", null);
                        return cc2;
                    }
                default:
                    return new Empty();
            }
        }
        private Instruction printST(ParseTreeNode root)
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
        private LinkedList<Expression> expressionList(ParseTreeNode root)
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
        private Instruction ifST(ParseTreeNode root)
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
                return new Empty();
        }
        private Instruction caseST(ParseTreeNode root)
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
        private LinkedList<CaseList> caseList(ParseTreeNode root)
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
        private Instruction whileST(ParseTreeNode root)
        {
            Expression e = expression(root.ChildNodes.ElementAt(1));
            Statement st = statements(root.ChildNodes.ElementAt(3));
            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
            return new While(line,column,e,st);
        }
        private Instruction repeatUntilST(ParseTreeNode root)
        {
            Expression e = expression(root.ChildNodes.ElementAt(3));
            Statement st = statements(root.ChildNodes.ElementAt(1));
            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
            return new RepeatUntil(line, column, e, st);
        }
        private Instruction forDoST(ParseTreeNode root)
        {
            Assignment ass = assignmentST(root.ChildNodes.ElementAt(1));
            Expression e = expression(root.ChildNodes.ElementAt(3));
            Statement st = statements(root.ChildNodes.ElementAt(5));
            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
            String tag = root.ChildNodes.ElementAt(2).Term.Name;
            if (tag == "to")
            {
                return new For(line, column, ass, e, st,true);
            }
            else
            {
                return new For(line, column, ass, e, st,false);
            }
        }


        //FUNCTIONS
        private Function functionST(ParseTreeNode root)
        {
            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
            String id = root.ChildNodes.ElementAt(1).Token.Text;
            LinkedList<Instruction> argument = argumentList(root.ChildNodes.ElementAt(3));
            LinkedList<Instruction> declaration = declarationList(root.ChildNodes.ElementAt(8));
            Statement statement = statements(root.ChildNodes.ElementAt(9));
            Type_ typ = type(root.ChildNodes.ElementAt(6));
            return new Function(line,column,id,argument,declaration,statement,typ);
        }
        private Function procedureST(ParseTreeNode root)
        {
            int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
            int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
            String id = root.ChildNodes.ElementAt(1).Token.Text;
            LinkedList<Instruction> argument = argumentList(root.ChildNodes.ElementAt(3));
            LinkedList<Instruction> declaration = declarationList(root.ChildNodes.ElementAt(6));
            Statement statement = statements(root.ChildNodes.ElementAt(7));
            return new Function(line, column, id, argument, declaration, statement, Type_.VOID);
        }
        private callFunction callFuncST(ParseTreeNode root)
        {
            String tag = root.ChildNodes.ElementAt(0).Term.Name;

            if (tag=="ID")
            {
                String id = root.ChildNodes.ElementAt(0).Token.Text;
                LinkedList<Expression> parameter = parameterList(root.ChildNodes.ElementAt(2));
                int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(1).Token.Location.Column;
                return new callFunction(line, column,id,parameter);

            }
            else
            {
                return new callFunction(-1010,0,"",null);
            }
        }
        private callFunction2 callFuncST2(ParseTreeNode root)
        {
            String tag = root.ChildNodes.ElementAt(0).Term.Name;

            if (tag == "ID")
            {
                String id = root.ChildNodes.ElementAt(0).Token.Text;
                LinkedList<Expression> parameter = parameterList(root.ChildNodes.ElementAt(2));
                int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(1).Token.Location.Column;
                return new callFunction2(line, column, id, parameter);

            }
            else
            {
                return new callFunction2(-1010, 0, "", null);
            }
        }
        private LinkedList<Expression> parameterList(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                LinkedList<Expression> list = parameterList(root.ChildNodes.ElementAt(0));
                list.AddLast(expression(root.ChildNodes.ElementAt(2)));
                return list;
            }
            else
            {
                if (root.ChildNodes.Count != 0)
                {
                    LinkedList<Expression> list = new LinkedList<Expression>();
                    list.AddLast(expression(root.ChildNodes.ElementAt(0)));
                    return list;
                }
                else
                {
                    LinkedList<Expression> list = new LinkedList<Expression>();
                    return list;
                }
            }
        }
        
        private LinkedList<Instruction> argumentList(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                LinkedList<Instruction> list = argumentList(root.ChildNodes.ElementAt(0));
                list.AddLast(argument(root.ChildNodes.ElementAt(2)));
                return list;
            }
            else
            {
                if (root.ChildNodes.Count != 0)
                {
                    LinkedList<Instruction> list = new LinkedList<Instruction>();
                    list.AddLast(argument(root.ChildNodes.ElementAt(0)));
                    return list;
                }
                else
                {
                    LinkedList<Instruction> list = new LinkedList<Instruction>();
                    return list;
                }
            }
        }
        private Argument argument(ParseTreeNode root)
        {
            
            if (root.ChildNodes.Count == 3)
            {
                int line = root.ChildNodes.ElementAt(1).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(1).Token.Location.Column;
                LinkedList<Access> idlist = idList(root.ChildNodes.ElementAt(0));

                Argument ar = new Argument(line, column, idlist, type(root.ChildNodes.ElementAt(2)), false);
                return ar;
            }
            else
            {

                int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
                LinkedList<Access> idlist = idList(root.ChildNodes.ElementAt(1));
                Argument ar = new Argument(line, column, idlist, type(root.ChildNodes.ElementAt(3)), true);
                return ar;
            }
        }

        private Statement statements(ParseTreeNode root)
        {
                return new Statement(instructionList(root.ChildNodes.ElementAt(1)), root.ChildNodes.ElementAt(0).Token.Location.Line, root.ChildNodes.ElementAt(0).Token.Location.Column);
        }
        private Type_ type(ParseTreeNode root)
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
        private Expression expression(ParseTreeNode root)
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
        private Expression finalExp(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                return expression(root.ChildNodes.ElementAt(1));
            }
            else
            {
               
                String type = root.ChildNodes.ElementAt(0).Term.ToString().ToUpper();
                String value = root.ChildNodes.ElementAt(0).Token != null ? root.ChildNodes.ElementAt(0).Token.ValueString : root.ChildNodes.ElementAt(0).Term.Name;
                int line=0;
                int column = 0;
                if (type != "ACCESS" && type!= "CALLFUNCST")
                {
                    line = root.ChildNodes.ElementAt(0).Token.Location.Line;
                    column = root.ChildNodes.ElementAt(0).Token.Location.Column;
                }
                //TODO agregar llamada a funciones
                switch (type)
                {
                    case "ACCESS":
                        return access(root.ChildNodes.ElementAt(0));
                    case "INTEGER":
                        try
                        {
                            return new Literal(int.Parse(value), Type_.INTEGER, line, column);
                        }
                        catch (Exception)
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
                    case "CALLFUNCST":
                        callFunction callf = callFuncST(root.ChildNodes.ElementAt(0));
                        return callf;
                }
                return null;
            }
        }
        private Access access(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 0)
            {
                int line = root.Token.Location.Line;
                int column = root.Token.Location.Column;
                String id = root.Token.ValueString;
                return new Access(line, column, id);
            }
            if (root.ChildNodes.Count == 1)
            {
                int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
                String id = root.ChildNodes.ElementAt(0).Token.ValueString;
                return new Access(line, column, id);
            }
            else
            {
                int line = root.ChildNodes.ElementAt(0).Token.Location.Line;
                int column = root.ChildNodes.ElementAt(0).Token.Location.Column;
                String id = root.ChildNodes.ElementAt(0).Token.ValueString;
                LinkedList<Expression> exp = expList(root.ChildNodes.ElementAt(1));
                return new Access(line, column,id,exp);
            }

        }

       

    }
}
