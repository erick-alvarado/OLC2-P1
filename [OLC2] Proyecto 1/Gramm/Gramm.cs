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
        public Gramm(): base(caseSensitive: false)
        {
            #region RE
            StringLiteral STR = new StringLiteral("STR", "'");
            var INTEGER = new NumberLiteral("INTEGER");
            var REAL = new RegexBasedTerminal("REAL", "[0-9]+'.'[0-9]+");
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
            var EQ_EQ = ToTerm("==");
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



            // TYPES
            NonTerminal type = new NonTerminal("type");

            // FUNCTIONS
            NonTerminal functionST = new NonTerminal("functionST");
            NonTerminal callFuncST = new NonTerminal("callFunST");

            // DECLARATION
            NonTerminal declara = new NonTerminal("declara");
            NonTerminal declarationVar = new NonTerminal("declarationVar");
            NonTerminal declarationType = new NonTerminal("declarationType");
            NonTerminal declarationObj = new NonTerminal("declarationObj");

            NonTerminal declarationList = new NonTerminal("declarationList");

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

            /* LOOPS */
            // WHILE - DO
            NonTerminal whileST = new NonTerminal("whileST");

            // REPEAT - UNTIL
            NonTerminal repeatUntilST = new NonTerminal("repeatUntilST");

            // FOR - DO
            NonTerminal forDoST = new NonTerminal("forDoST");

            /* EXPRESION */
            NonTerminal expression = new NonTerminal("expression");
            NonTerminal finalExp = new NonTerminal("finalExp");

            /* GENERAL */
            NonTerminal access = new NonTerminal("access");
            //Extra
            NonTerminal idList = new NonTerminal("idList");

            #endregion

            #region Grammar
            start.Rule = RPROGRAM + ID + SEMICOLON + globalVariables /*+ functionList */+ main
                ;

            globalVariables.Rule = Empty
                | declarationList 
                ;

            declarationList.Rule = declarationList + declara
                | declara;

            declara.Rule = RTYPE + ID + EQUAL + ROBJECT + RVAR + declarationObj + REND
                | RTYPE + declarationListType
                | RVAR + declarationListVar
                ;

            declarationListVar.Rule = declarationListVar + declarationVar
                | declarationVar
                ;
            declarationListType.Rule = declarationListType + declarationType
                | declarationType
                ;

            declarationVar.Rule = argument + SEMICOLON
                | argument + EQUAL + expression + SEMICOLON
                | idList + POINTS + ID + SEMICOLON
                ;

            declarationType.Rule = idList + EQUAL + type + SEMICOLON
                ;

            declarationObj.Rule = declarationObj + argument + SEMICOLON
                | argument + SEMICOLON
                ;
            argument.Rule = idList + POINTS + type
                ;
            idList.Rule = idList +COMMA + ID
                | ID
                ;
            type.Rule = RINTEGER | RSTRING | RREAL | RBOOLEAN | RVOID;

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
                | access
                | INTEGER
                | REAL
                | STR
                | TRUE
                | FALSE
                | NULL
                ;

            access.Rule = ID
                ;


            //    ;
            //functionList.Rule = Empty /*| */
            //    ;
            main.Rule = RBEGIN + instructionList + REND + POINT
                ;
            main.ErrorRule = SyntaxError + REND + POINT;

            instructionList.Rule = instructionList + instruction
                | instruction
                ;

            instruction.Rule = assignmentST
                //| printST
                //| functionST
                //| ifST
                //| caseST
                //| whileST
                //| repeatUntilST
                //| forDoST
                //| callFuncST
                //| RGRAFICARTS + LEFTPAR + RIGHTPAR + SEMICOLON COLOCAR graficarTS dentro de callFunc
                //| RBREAK + SEMICOLON
                //| RCONTINUE + SEMICOLON
                | statements + SEMICOLON
                ;
            instruction.ErrorRule = SyntaxError + SEMICOLON
                | SyntaxError + REND
                ;

            


            assignmentST.Rule = ID + P_EQUAL + expression + SEMICOLON
                ;

            statements.Rule = RBEGIN + instructionList + REND
                ;

            ifST.Rule = RIF + expression + RTHEN + statements + SEMICOLON
                | RIF + expression + RTHEN + statements + RELSE + statements + SEMICOLON
                | RIF + expression + RTHEN + statements + RELSE + ifST
                ;

            whileST.Rule = RWHILE + expression + RDO + statements + SEMICOLON
                ;

            printST.Rule = RWRITE + LEFTPAR + expression + RIGHTPAR + SEMICOLON
                | RWRITELN + LEFTPAR + expression + RIGHTPAR + SEMICOLON
                ;

            
                ;
            #endregion

            #region Preferences
            this.Root = start;
            #endregion
        }
    }
}
