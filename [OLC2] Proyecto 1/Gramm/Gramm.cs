using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
namespace _OLC2__Proyecto_1.Gramm
{
    class Gramm: Grammar
    {
        public Gramm() : base(caseSensitive: false)
        {
            #region RE
            StringLiteral STR = new StringLiteral("STR", "'");
            var REAL = new RegexBasedTerminal("REAL", "[0-9]+'.'[0-9]+");
            var INTEGER = new NumberLiteral("INTEGER");
            IdentifierTerminal ID = new IdentifierTerminal("ID");

            CommentTerminal lineComment = new CommentTerminal("lineComment", "//", "\n", "\r\n");
            CommentTerminal keyComment = new CommentTerminal("keyComment", "{", "}");
            CommentTerminal parComment = new CommentTerminal("parComment", "(*", "*)");
            #endregion

            #region Terminals
            var TRUE = ToTerm("true");
            var FALSE = ToTerm("false");
            var NULL = ToTerm("null");

            var TIMES = ToTerm("*");
            var DIVISION = ToTerm("/");
            var MODULE = ToTerm("%");
            var PLUS = ToTerm("+");
            var MINUS = ToTerm("-");

            var GREAT_EQ = ToTerm(">=");
            var LESS_EQ = ToTerm("<=");
            var DISTINCT = ToTerm("<>");
            var GREAT = ToTerm(">");
            var LESS = ToTerm("<");

            var OR = ToTerm("or");
            var AND = ToTerm("and");
            var NOT = ToTerm("not");

            var EQUAL = ToTerm("=");
            var P_EQUAL = ToTerm(":=");
            var SEMICOLON = ToTerm(";");
            var POINTS = ToTerm(":");
            var LEFTPAR = ToTerm("(");
            var RIGHTPAR = ToTerm(")");
            var LEFTCOR = ToTerm("[");
            var RIGHTCOR = ToTerm("]");
            var COMMA = ToTerm(",");
            var POINT = ToTerm(".");

            var RSTRING = ToTerm("string");
            var RINTEGER = ToTerm("integer");
            var RREAL = ToTerm("real");
            var RBOOLEAN = ToTerm("boolean");
            var RVOID = ToTerm("void");


            var RTYPE = ToTerm("type");
            var ROBJECT = ToTerm("object");
            var RARRAY = ToTerm("array");

            var RPROGRAM = ToTerm("program");
            var RBEGIN = ToTerm("begin");
            var REND = ToTerm("end");
            var ROF = ToTerm("of");
            var RVAR = ToTerm("var");
            var RCONST = ToTerm("const");

            var RIF = ToTerm("if");
            var RTHEN = ToTerm("then");
            var RELSE = ToTerm("else");

            var RCASE = ToTerm("case");

            var RWHILE = ToTerm("while");
            var RDO = ToTerm("do");

            var RREPEAT = ToTerm("repeat");
            var RUNTIL = ToTerm("until");

            var RFOR = ToTerm("for");
            var RTO = ToTerm("to");
            var RDOWNTO = ToTerm("downto");

            var RBREAK = ToTerm("break");
            var RCONTINUE = ToTerm("continue");

            var RFUNCTION = ToTerm("function");
            var RPROCEDURE = ToTerm("procedure");

            var RWRITELN = ToTerm("writeln");
            var RWRITE = ToTerm("write");
            var REXIT = ToTerm("exit");
            var RGRAFICARTS = ToTerm("graficar_ts");

            RegisterOperators(1, Associativity.Left, OR);
            RegisterOperators(2, Associativity.Left, AND);
            RegisterOperators(3, Associativity.Right, NOT);
            RegisterOperators(4, Associativity.Left, EQUAL, DISTINCT, LESS_EQ, GREAT_EQ, LESS, GREAT);
            RegisterOperators(5, Associativity.Left, PLUS, MINUS);
            RegisterOperators(6, Associativity.Left, TIMES, DIVISION, MODULE);

            NonGrammarTerminals.Add(lineComment);
            NonGrammarTerminals.Add(keyComment);
            NonGrammarTerminals.Add(parComment);
            #endregion

            #region NonTerminals
            NonTerminal start = new NonTerminal("start");
            NonTerminal globalVariables = new NonTerminal("globalVariables");
            NonTerminal functionList = new NonTerminal("functionList");
            NonTerminal main = new NonTerminal("main");
            NonTerminal instructionList = new NonTerminal("instructionList");
            NonTerminal instruction = new NonTerminal("instruction");

            NonTerminal statements = new NonTerminal("statements");
            NonTerminal argument = new NonTerminal("argument");
            NonTerminal argumentList = new NonTerminal("argumentList");
            NonTerminal parameterList = new NonTerminal("parameterList");



            // TYPES
            NonTerminal type = new NonTerminal("type");
            NonTerminal subrange = new NonTerminal("subrange");


            // FUNCTIONS
            NonTerminal functionST = new NonTerminal("functionST");
            NonTerminal callFuncST = new NonTerminal("callFuncST");

            //Procedures
            NonTerminal procedureST = new NonTerminal("procedureST");


            // DECLARATION
            NonTerminal declara = new NonTerminal("declara");
            NonTerminal declaration = new NonTerminal("declaration");

            NonTerminal declarationVar = new NonTerminal("declarationVar");
            NonTerminal declarationType = new NonTerminal("declarationType");

            NonTerminal declarationList = new NonTerminal("declarationList");
            NonTerminal declarationObj = new NonTerminal("declarationObj");

            NonTerminal declarationListVar = new NonTerminal("declarationListVar");
            NonTerminal declarationListType = new NonTerminal("declarationListType");



            // ASSIGNMENT
            NonTerminal assignmentST = new NonTerminal("assignmentST");

            // PRINT
            NonTerminal printST = new NonTerminal("printST");

            /* CONDITIONS */
            // IF
            NonTerminal ifST = new NonTerminal("ifST");

            // CASE
            NonTerminal caseST = new NonTerminal("caseST");
            NonTerminal caseList = new NonTerminal("caseList");

            /* LOOPS */
            // WHILE - DO
            NonTerminal whileST = new NonTerminal("whileST");

            // REPEAT - UNTIL
            NonTerminal repeatUntilST = new NonTerminal("repeatUntilST");

            // FOR - DO
            NonTerminal forDoST = new NonTerminal("forDoST");

            /* EXPRESION */
            NonTerminal expression = new NonTerminal("expression");
            NonTerminal expressionList = new NonTerminal("expressionList");
            NonTerminal finalExp = new NonTerminal("finalExp");

            /* GENERAL */
            NonTerminal access = new NonTerminal("access");
            //Extra
            NonTerminal idList = new NonTerminal("idList");

            #endregion

            #region Grammar
            start.Rule = RPROGRAM + ID + SEMICOLON + globalVariables+ main
                ;

            globalVariables.Rule = declarationList
                ;

            //DECLARATION
            declarationList.Rule = declarationList + declara
                | declara;

            declara.Rule = RTYPE + declarationListType
                | RVAR + declarationListVar
                | RCONST + declaration
                | functionST
                | procedureST
                | Empty
            ;
            declaration.Rule = ID + EQUAL + expression + SEMICOLON
                ;
            declarationListVar.Rule = declarationListVar + declarationVar + SEMICOLON
                | declarationVar + SEMICOLON
                ;
            declarationListType.Rule = declarationListType + declarationType 
                | declarationType 
                ;


            declarationVar.Rule = idList+ POINTS+ type + EQUAL + expression  //TODO validar en semantico que idList solo tenga 1 ID
                |  idList + POINTS + type 
                ;
            

            declarationType.Rule = idList + EQUAL + type + SEMICOLON
                | idList + EQUAL + ROBJECT +declarationList + REND + SEMICOLON //TODO validar no funcioens y procedimientos
                | idList + EQUAL + RARRAY + LEFTCOR + type  + RIGHTCOR + ROF +type+SEMICOLON
                | idList + EQUAL + RARRAY + LEFTCOR + type + RIGHTCOR + ROF + subrange + SEMICOLON
                | idList + EQUAL + RARRAY + LEFTCOR + subrange + RIGHTCOR + ROF + type + SEMICOLON
                | idList + EQUAL + RARRAY + LEFTCOR + subrange + RIGHTCOR + ROF + subrange + SEMICOLON
                ;

            idList.Rule = idList + COMMA + ID
                | ID
                ;
            assignmentST.Rule = ID + P_EQUAL + expression 
                ;
            statements.Rule = RBEGIN + instructionList + REND + SEMICOLON
                
                ;

            //MAIN
            main.Rule = RBEGIN + instructionList + REND + POINT
                ;


            instructionList.Rule = instructionList + instruction 
                | instruction 
                ;

            instruction.Rule = assignmentST
                | ifST
                | caseST
                | printST
                | assignmentST + SEMICOLON
                | whileST
                | forDoST
                | repeatUntilST
                | RBREAK 
                | RCONTINUE
                | callFuncST
                | RBREAK + SEMICOLON
                | RCONTINUE + SEMICOLON
                | REXIT + LEFTPAR + expression + RIGHTPAR + SEMICOLON
                | Empty
                ;

            //CONDITIONS
            ifST.Rule = RIF + expression + RTHEN + statements 
                | RIF + expression + RTHEN + statements + RELSE + statements 
                | RIF + expression + RTHEN + statements + RELSE + ifST
                ;

            caseST.Rule = RCASE + expression + ROF + caseList + RELSE + statements + REND + SEMICOLON
                | RCASE +expression+ ROF + caseList + REND + SEMICOLON
                ;

            caseList.Rule = caseList + expressionList + POINTS + statements
                | expressionList + POINTS + statements
                ;


            //EXPRESSIONS
            expressionList.Rule= expressionList + COMMA + expression  
                | expression
                ;

            expression.Rule = MINUS + expression
                | NOT + expression
                | expression + PLUS + expression
                | expression + MINUS + expression
                | expression + TIMES + expression
                | expression + DIVISION + expression
                | expression + MODULE + expression
                | expression + LESS + expression
                | expression + GREAT + expression
                | expression + LESS_EQ + expression
                | expression + GREAT_EQ + expression
                | expression + EQUAL + expression
                | expression + DISTINCT + expression
                | expression + OR + expression
                | expression + AND + expression
                | finalExp
                ;

            finalExp.Rule = LEFTPAR + expression + RIGHTPAR
                | callFuncST
                | access
                | INTEGER
                | REAL
                | STR
                | TRUE
                | FALSE
                | NULL
                ;
            type.Rule = RREAL | RINTEGER | RSTRING |  RBOOLEAN | RVOID | ID
                ;

            subrange.Rule = expression + POINT + POINT + expression
                ;
            access.Rule = ID
                ;
            // Functions
            functionST.Rule = RFUNCTION + ID + LEFTPAR + argumentList + RIGHTPAR+ POINTS + type + SEMICOLON+ declarationList + statements
                ;
            procedureST.Rule = RPROCEDURE + ID + LEFTPAR + argumentList + RIGHTPAR +  SEMICOLON + declarationList + statements 
                ;
            callFuncST.Rule = ID + LEFTPAR + parameterList + RIGHTPAR + SEMICOLON
                | RGRAFICARTS + LEFTPAR + RIGHTPAR + SEMICOLON;
                ;

            parameterList.Rule = parameterList + COMMA + expression
                | expression
                | Empty
                ;
            argumentList.Rule = argumentList + SEMICOLON + argument
               | argument
               | Empty
               ;
            argument.Rule = idList + POINTS + type
                | RVAR + idList + POINTS + type
                ;
           
            //LOOPS

            whileST.Rule = RWHILE + expression + RDO + statements 
                ;
            forDoST.Rule = RFOR + assignmentST + RTO + expression + RDO +statements
                | RFOR + assignmentST + RDOWNTO + expression + RDO + statements
                ;
            repeatUntilST.Rule = RREPEAT + statements + RUNTIL + expression + SEMICOLON
                ;
            printST.Rule = RWRITE + LEFTPAR + expressionList + RIGHTPAR + SEMICOLON
                | RWRITELN + LEFTPAR + expressionList + RIGHTPAR + SEMICOLON
                | RWRITE + LEFTPAR + expressionList + RIGHTPAR
                | RWRITELN + LEFTPAR + expressionList + RIGHTPAR
                | RWRITELN + SEMICOLON
                ;

            //ErrorRule
            main.ErrorRule = SyntaxError + REND + POINT;
            instruction.ErrorRule = SyntaxError + SEMICOLON
                | SyntaxError + REND
                ;
            #endregion

            #region Preferences
            this.Root = start;
            #endregion
        }
    }
}
